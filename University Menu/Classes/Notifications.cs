using Microsoft.Win32;
using System;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using University_Menu.UserControls;
using WUApiLib;
using static System.String;
using static System.Environment;

namespace University_Menu
{
    public partial class MainWindow
    {
        public static void CheckNotifications(bool showNotifications)
        {
            if (!timerFirstLoop)
            {
                CheckOSUpgrade(showNotifications);
                CheckRoamingProfile();
                CheckRebootPending(showNotifications);
                CheckCheckup(showNotifications);
                CheckNetworkSpace(showNotifications);
            }
        }

        #region Checkup
        public static void CheckCheckup(bool showNotification, int days = -1)
        {
            if (!allowCheckup) { return; }

            Variable var = new Variable();
            string input = var.WinUpdateSearch;
            double comparison;

            if (!DateTime.TryParse(input, out DateTime output)) { output = DateTime.Now; }

            if (days < 0) { comparison = (DateTime.Now - output).TotalDays; }
            else { comparison = days; }

            double comparisonLimit = checkupMinDays + ((checkupMaxDays - checkupMinDays) / 3 * 2);

            if (comparison >= checkupMinDays)
            {
                if (comparison > checkupMaxDays)
                {
                    ChangeNotificationIcon(IconType.Red, Properties.Resources.DefaultCheckup, ref checkupIconState);
                    return;
                }
                else
                {
                    if (comparison <= comparisonLimit)
                    { ChangeNotificationIcon(IconType.Blue, Properties.Resources.DefaultCheckup, ref checkupIconState); }
                    else
                    { ChangeNotificationIcon(IconType.Yellow, Properties.Resources.DefaultCheckup, ref checkupIconState); }
                }

                if (showNotification && !checkupExclude)
                { CheckNotificationStatus(ref lastCheckupPopup, ref lastCheckupBalloon, checkupBalloonInterval, checkupPopupInterval, checkupBalloon, Popup.Checkup); }
            }
            else
            { ResetNotificationStatus(ref checkupIconState, Properties.Resources.DefaultCheckup, ref checkupExclude, ref checkupBalloon, ref lastCheckupBalloon, ref lastCheckupPopup); }
        }
        #endregion

        #region NetworkSpace
        public static void CheckNetworkSpace(bool showNotification, int space = -1)
        {
            if (!allowNetwork) { return; }

            Variable var = new Variable();
            long freeSpace;

            if (space < 0) { freeSpace = var.HomeDriveFreeSpace; }
            else { freeSpace = space; }

            if (freeSpace >=0 && freeSpace <= networkLimitMB * 1024 * 1024)
            {
                ChangeNotificationIcon(IconType.Yellow, Properties.Resources.DefaultNetwork, ref networkIconState);

                if (showNotification && !networkExclude)
                { CheckNotificationStatus(ref lastNetworkPopup, ref lastNetworkBalloon, networkBalloonInterval, networkPopupInterval, networkBalloon, Popup.NetworkSpace); }
            }
            else
            { ResetNotificationStatus(ref networkIconState, Properties.Resources.DefaultNetwork, ref networkExclude, ref networkBalloon, ref lastNetworkBalloon, ref lastNetworkPopup); }
        }
        #endregion

        #region Welcome
        public static void WelcomeMessage()
        {
            if (!allowWelcome || welcomeCount == -2) { return; }

            if (welcomeCount == -1)
            {
                ++welcomeCount;
                WriteXML();
            }
            else
            {
                if (welcomeCount >= 0 && welcomeCount < welcomeShowCount && (DateTime.Now - lastShown).TotalDays >= 1)
                {
                    ++welcomeCount;
                    WriteXML();
                    ShowNotification(false, Popup.Welcome);
                }
            }
        }
        #endregion

        #region RebootPending
        public static void CheckRebootPending(bool showNotification, bool activate = false)
        {
            if (!allowReboot) { return; }
            bool bootRequired = false, bootRequiredWU = false;

            // Windows Updates
            try
            {
                SystemInformation si = new SystemInformation();
                if (si.RebootRequired) { bootRequiredWU = true; }
            }
            catch { }

            // PendingFileRenameOperations
            try
            {
                string[] values = (string[])Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager",
                    "PendingFileRenameOperations", null);

                if (values != null && values.Length > 0)
                { bootRequired = true; }
            }
            catch { }

            // Component-Based Servicing
            if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing", "RebootPending", null) != null)
            { bootRequired = true; }

            // SCCM 2012 Client Reboot Pending Status
            try
            {
                ConnectionOptions co = new ConnectionOptions();
                ManagementScope ms = new ManagementScope(@"\\.\ROOT\ccm\ClientSDK", co);

                using (ManagementClass mc = new ManagementClass(ms.Path.Path, "CCM_ClientUtilities", null))
                using (ManagementBaseObject mbo = mc.InvokeMethod("DetermineIfRebootPending", null, null))
                {
                    if ((bool)mbo["RebootPending"] || (bool)mbo["IsHardRebootPending"]) { bootRequired = true; }
                }
            }
            catch { }

            if (bootRequired || bootRequiredWU || activate)
            {
                ChangeNotificationIcon(IconType.Blue, Properties.Resources.DefaultReboot, ref rebootIconState);

                if (showNotification && !rebootExclude)
                {
                    if ((DateTime.Now - lastShown).TotalDays >= 1)
                    {
                        Variable var = new Variable();
                        DateTime bootTime = var.SystemBootTime;
                        double bootTimeDays = (DateTime.Now - bootTime).TotalDays;
                        int wait = rebootWait;

                        if (bootRequiredWU) {wait = rebootWaitWU; }

                        if (bootTimeDays >= wait && wait >= 0)
                        {
                            int balloonDays = wait + rebootBalloonShowtime;
                            int popupDays = balloonDays + rebootPopupShowtime;

                            if (balloonShowTime < 0 || bootTimeDays <= balloonDays) { ShowNotification(false, Popup.RebootPending); }
                            else if (rebootPopupShowtime < 0 || bootTimeDays <= popupDays) { ShowNotification(true, Popup.RebootPending); }
                        }
                    }
                }
            }
            else
            {
                bool rebootBoolValue = false;
                DateTime rebootDateValue = DateTime.MinValue;

                ResetNotificationStatus(ref rebootIconState, Properties.Resources.DefaultReboot, ref rebootExclude, ref rebootBoolValue, ref rebootDateValue, ref rebootDateValue);
            }
        }
        #endregion

        #region RoamingProfile
        [DllImport("Userenv.dll", EntryPoint = "GetProfileType", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool GetProfileType(ref uint pdwflags);

        [Flags]
        enum Win32ProfileType : uint
        {
            Local = 0x00,
            Temporary = 0x01,
            Roaming = 0x02,
            Mandatory = 0x04
        }

        public static void CheckRoamingProfile(uint status = 9)
        {
            if (!allowRoaming) { return; }

            if (status == 9)
            {
                try { GetProfileType(ref status); }
                catch { return; }
            }

            switch (status)
            {
                case 2:
                    if (DateTime.Now < roamingStartDate || DateTime.Now > roamingEndDate) { break; }

                    ChangeNotificationIcon(IconType.Blue, Properties.Resources.DefaultRoaming, ref roamingIconState);

                    if (roamingCount >= roamingShowCount) { return; }
                    if (roamingExclude) { return; }
                    if ((DateTime.Now - lastShown).TotalDays < 1) { return; }

                    roamingCount = ++roamingCount;

                    ShowNotification(true, Popup.RoamingProfile);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region WarrantyExpired
        public static void CheckWarrantyExpired(DateTime test)
        {
            if (!allowWarranty) { return; }

            Variable var = new Variable();
            string input = var.Warranty;

            if (!DateTime.TryParse(input, out DateTime output)) { output = DateTime.Now; }

            double lastPopup = (DateTime.Now - lastWarrantyPopup).TotalDays;

            if (lastPopup < 30 && DateTime.Now > output)
            {
                Random rnd = new Random();
                int end = 99;

                if (test != defaultDate) { end = DateTime.DaysInMonth(test.Year, test.Month) - test.Day + 1; }
                else { end = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day + 1; }

                int random = rnd.Next(0, end);

                if (random != 0) { return; }
            }
            else { return; }

            ChangeNotificationIcon(IconType.Yellow, Properties.Resources.DefaultWarranty, ref warrantyIconState);

            ShowNotification(true, Popup.WarrantyExpired);
        }
        #endregion

        #region OSUpgrade
        public static void CheckOSUpgrade(bool showNotification, bool activate = false)
        {
            if (!allowOSUpgrade) { return; }
            if (OSVersion.Version.Major >= 10) { return; } // Windows 10 or above

            using (DirectoryEntry entry = new DirectoryEntry("LDAP://ad.helsinki.fi:636")
            { AuthenticationType = AuthenticationTypes.Secure | AuthenticationTypes.SecureSocketsLayer | AuthenticationTypes.ServerBind })
            using (DirectorySearcher search = new DirectorySearcher(entry)
            { Filter = Format("(&(objectClass=computer)(CN={0}))", MachineName) })
            {
                SearchResult result = search.FindOne();

                OSGroup = OSUpgradeGroups.None;

                if (result != null)
                {
                    using (DirectoryEntry ws = result.GetDirectoryEntry())
                    {
                        PropertyValueCollection pvc = ws.Properties["memberOf"];

                        foreach (string property in pvc)
                        {
                            if (property.Contains("CN=w10mig_inplaceupgrade,"))
                            {
                                OSGroup = OSUpgradeGroups.InPlace;
                                break;
                            }
                            else if (property.Contains("CN=w10mig_poistuvat,"))
                            {
                                OSGroup = OSUpgradeGroups.Poistuvat;
                                break;
                            }
                        }
                    }
                }
            }

            if (OSGroup != OSUpgradeGroups.None || activate)
            {
                DateTime red = new DateTime(2019, 9, 12);

                if (DateTime.Now >= red)
                {
                    ChangeNotificationIcon(IconType.Red, Properties.Resources.DefaultOSUpgrade, ref osupgradeIconState);

                    if (showNotification)
                    {
                        if ((DateTime.Now - lastOSUpgradePopup).TotalDays >= osupgradePopupInterval && (DateTime.Now - lastShown).TotalDays >= 1)
                        { ShowNotification(true, Popup.OSUpgrade); }
                    }
                }
                else
                { ChangeNotificationIcon(IconType.Blue, Properties.Resources.DefaultOSUpgrade, ref osupgradeIconState); }
            }
            else
            { ChangeNotificationIcon(IconType.Normal, Properties.Resources.DefaultOSUpgrade, ref osupgradeIconState); }
        }
        #endregion

        private static void CheckNotificationStatus(ref DateTime popup, ref DateTime balloon, int intervalBalloon, int intervalPopup, bool balloonStatus, Popup source)
        {
            if (popup <= defaultDate) { popup = DateTime.Now; }
            if (balloon <= defaultDate) { balloon = (DateTime.Now.AddDays(-intervalBalloon)); }

            double popupTime = (DateTime.Now - popup).TotalDays;
            double balloonTime = (DateTime.Now - balloon).TotalDays;

            if ((DateTime.Now - lastShown).TotalDays >= 1)
            {
                if (popupTime >= intervalPopup && balloonStatus) { ShowNotification(true, source); }
                else if (balloonTime >= intervalBalloon) { ShowNotification(false, source); }
            }
        }

        private static void ResetNotificationStatus(ref IconType iconState, string menuItemName, ref bool exclude, ref bool balloon,
            ref DateTime lastBalloon, ref DateTime lastPopup)
        {
            bool change = false;

            if (iconState != IconType.Normal)
            {
                ChangeNotificationIcon(IconType.Normal, menuItemName, ref iconState);
                change = true;
            }

            if (exclude)
            {
                exclude = false;
                change = true;
            }
            if (balloon)
            {
                balloon = false;
                change = true;
            }
            if (lastBalloon != defaultDate)
            {
                lastBalloon = defaultDate;
                change = true;
            }
            if (lastPopup != defaultDate)
            {
                lastPopup = defaultDate;
                change = true;
            }

            if (change) { WriteXML(); }
        }

        public static void ChangeMenuItemNotification(ref MenuItem mi, IconType iconState)
        {
            switch (iconState)
            {
                case IconType.Normal:
                    mi.Icon = null;
                    mi.Foreground = themeFore;
                    mi.Background = System.Windows.Media.Brushes.Transparent;
                    mi.ClearValue(ForegroundProperty);
                    break;
                case IconType.Blue:
                    mi.Icon = MenuIcon(Properties.Resources.VectorInfo, 1, 1, 1);
                    mi.Foreground = (darkTheme ? themeForeOpposite : themeFore);
                    mi.Background = System.Windows.Media.Brushes.DeepSkyBlue;
                    if (!darkTheme) { mi.ClearValue(ForegroundProperty); }
                    break;
                case IconType.Yellow:
                    mi.Icon = MenuIcon(Properties.Resources.VectorHazard, 1, 1, 1);
                    mi.Foreground = (darkTheme ? themeForeOpposite : themeFore);
                    mi.Background = (darkTheme ? System.Windows.Media.Brushes.DarkOrange : System.Windows.Media.Brushes.Orange);
                    if (!darkTheme) { mi.ClearValue(ForegroundProperty); }
                    break;
                case IconType.Red:
                    mi.Icon = MenuIcon(Properties.Resources.VectorHazard, 1, 1, 1);
                    mi.Foreground = (darkTheme ? themeFore : themeForeOpposite);
                    mi.Background = System.Windows.Media.Brushes.Red;
                    if (darkTheme) { mi.ClearValue(ForegroundProperty); }
                    break;
                default:
                    break;
            }
        }

        private static void ChangeNotificationIcon(IconType icon, string menuItemName, ref IconType state, bool module = false, bool force = false)
        {
            if (!module)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    MenuItem mi = new MenuItem();

                    try { mi = DefaultMenuItemList.Find(item => item.Name == menuItemName); }
                    catch { return; }

                    mi.Visibility = (icon == IconType.Normal ? Visibility.Collapsed : Visibility.Visible);

                    ChangeMenuItemNotification(ref mi, icon);
                });

                state = icon;
            }

            notifyStatus = new int[] { checkupIconState.GetHashCode(), networkIconState.GetHashCode(), rebootIconState.GetHashCode(), roamingIconState.GetHashCode(), warrantyIconState.GetHashCode(), osupgradeIconState.GetHashCode() };
            moduleStatus = new int[] { moduleUserIconState.GetHashCode(), moduleCompIconState.GetHashCode() };

            int status = (notifyStatus.Max() > moduleStatus.Max() ? notifyStatus.Max() : moduleStatus.Max());

            if (status != iconStatus || force)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    if (status > 0) { UMicon.ToolTipText = GetTranslation(Properties.Resources.UMIconTextNotify); }
                    else { UMicon.ToolTipText = GetTranslation(Properties.Resources.UMIconText); }

                    switch (status)
                    {
                        case 1:
                            UMicon.Icon = new Icon(Properties.Resources.NotifyIcon_Blue, iconFixedSize, iconFixedSize);
                            break;
                        case 2:
                            UMicon.Icon = new Icon(Properties.Resources.NotifyIcon_Orange, iconFixedSize, iconFixedSize);
                            break;
                        case 3:
                            UMicon.Icon = new Icon(Properties.Resources.NotifyIcon_Red, iconFixedSize, iconFixedSize);
                            break;
                        default:
                            UMicon.Icon = new Icon(Properties.Resources.NotifyIcon_Black, iconFixedSize, iconFixedSize);
                            break;
                    }
                });

                iconStatus = status;
            }
        }

        public static void ShowNotification(bool popup, Popup source, bool test = false)
        {
            if (!test) { lastShown = DateTime.Now; }

            if (popup)
            {
                string filePath = Empty, fileParameters = Empty;

                switch (source)
                {
                    case Popup.Checkup:
                        lastCheckupPopup = DateTime.Now;
                        filePath = checkupPopupFilePath;
                        fileParameters = checkupPopupFileParameters;
                        break;
                    case Popup.NetworkSpace:
                        lastNetworkPopup = DateTime.Now;
                        filePath = networkPopupFilePath;
                        fileParameters = networkPopupFileParameters;
                        break;
                    case Popup.RebootPending:
                        filePath = rebootPopupFilePath;
                        fileParameters = rebootPopupFileParameters;
                        break;
                    case Popup.RoamingProfile:
                        break;
                    case Popup.WarrantyExpired:
                        filePath = warrantyPopupFilePath;
                        fileParameters = warrantyPopupFileParameters;
                        break;
                    case Popup.OSUpgrade:
                        lastOSUpgradePopup = DateTime.Now;
                        break;
                    default:
                        return;
                }

                if (!IsNullOrWhiteSpace(filePath))
                {
                    string path = VariableConvert(filePath);

                    if (File.Exists(path))
                    {
                        Execute(path, VariableConvert(fileParameters));
                        return;
                    }
                    else { OpenWindow(source); }
                }
                else { OpenWindow(source); }
            }
            else
            {
                bool standard = false;
                string title = GetTranslation(Properties.Resources.NotifyBalloonTitle), text = Empty;

                switch (source)
                {
                    case Popup.Checkup:
                        lastCheckupBalloon = DateTime.Now;
                        checkupBalloon = true;
                        text = GetTranslation(Properties.Resources.NotifyCheckupBalloonText);
                        break;
                    case Popup.NetworkSpace:
                        lastNetworkBalloon = DateTime.Now;
                        networkBalloon = true;
                        text = GetTranslation(Properties.Resources.NotifyNetworkBalloonText);
                        break;
                    case Popup.RebootPending:
                        text = GetTranslation(Properties.Resources.NotifyRebootBalloonText);
                        break;
                    case Popup.Welcome:
                        standard = true;
                        title = GetTranslation(Properties.Resources.NotifyWelcomeTitle);
                        text = GetTranslation(Properties.Resources.NotifyWelcomeText);
                        break;
                    default:
                        return;
                }

                ShowBalloon(standard, title, text, source);
            }

            if (!test) { WriteXML(); }
        }

        private static void ShowBalloon(bool standard, string title, string text, Popup source)
        {
            if (standard)
            { UMicon.ShowBalloonTip(title, text, new Icon(Properties.Resources.NotifyIcon, 48, 48), true); }
            else
            {
                FancyBalloon balloon = new FancyBalloon(title, text, source);
                UMicon.ShowCustomBalloon(balloon, PopupAnimation.Fade, balloonShowTime);
            }
        }

        public enum IconType
        {
            Normal = 0,
            Blue = 1,
            Yellow = 2,
            Red = 3
        }

        public enum Popup
        {
            Checkup,
            NetworkSpace,
            RebootPending,
            RoamingProfile,
            WarrantyExpired,
            Welcome,
            SupportRequest,
            Chat,
            Settings,
            OSUpgrade
        }

        public enum OSUpgradeGroups
        {
            None,
            InPlace,
            Poistuvat
        }
    }
}
