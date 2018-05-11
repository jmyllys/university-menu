using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.Management;
using static System.String;

namespace University_Menu
{
    public partial class MainWindow
    {
        public static void BuildModules(bool updateOnly = false)
        {
            if (!updateOnly) { ModuleList.Clear(); }
            UserPrincipal user = null;

            try { user = UserPrincipal.Current; }
            catch { }

            #region UserInformation
            IconType moduleUIValue = IconType.Normal;
            MenuItem ui = null;

            if (!updateOnly) { ui = new MenuItem { Name = Properties.Resources.ModuleUserInfo, Header = GetTranslation(Properties.Resources.ModuleUserInfo) }; }
            else
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    { ui = ModuleList.Find(x => x.Name == Properties.Resources.ModuleUserInfo); });
                }
                catch { goto PassUser; }
            }
            
            if (!allowUserInfo)
            {
                Application.Current.Dispatcher.Invoke(delegate
                { ui.IsEnabled = false; });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(delegate
                { ui.IsEnabled = true; });

                Variable var = new Variable();
                bool separatorCheck = false, italic = false;

                moduleUIDisplayName = user?.DisplayName ?? moduleUIDisplayName;

                moduleUIEmail = user?.EmailAddress ?? moduleUIEmail;
                moduleUIMailbox = GetMailboxType(user);

                try { moduleUILastPWSet = user.LastPasswordSet.Value; }
                catch { }
                try { moduleUIPWExpires = moduleUILastPWSet.AddDays(GetMaxPasswordAge().Days); }
                catch { }

                try { moduleUIAccountExpires = user.AccountExpirationDate.Value; }
                catch { }

                try { moduleUIHomeDirectory = user?.HomeDirectory ?? moduleUIHomeDirectory; }
                catch { }

                try { moduleUIHomeDrive = user?.HomeDrive ?? moduleUIHomeDrive; }
                catch { }

                try
                {
                    if (IsNullOrWhiteSpace(user?.DisplayName)) { italic = true; }
                    else { italic = false; }
                }
                catch { italic = true; }
                AddModuleItem(ref ui, Properties.Resources.ModuleUserFullName, moduleUIDisplayName, ref separatorCheck, updateOnly, fontItalic: italic,
                    icon: MenuIcon("F1 M 38,19C 43.5417,19 45.9167,22.1667 45.1174,28.8134C 45.8315,29.2229 46.3125,29.9928 46.3125,30.875C 46.3125,31.9545 45.5923,32.8658 44.6061,33.1546C 44.1941,34.623 43.5543,35.9229 42.75,36.9628L 42.75,41.9583C 45.3889,42.4861 47.5,42.75 50.6667,44.3333C 53.8333,45.9167 54.8889,47.3681 57,49.4792L 57,57L 19,57L 19,49.4792C 21.1111,47.3681 22.1667,45.9167 25.3333,44.3333C 28.5,42.75 30.6111,42.4861 33.25,41.9583L 33.25,36.9628C 32.4457,35.9229 31.8059,34.623 31.3939,33.1546C 30.4077,32.8658 29.6875,31.9545 29.6875,30.875C 29.6875,29.9928 30.1685,29.2229 30.8826,28.8134C 30.0833,22.1667 32.4583,19 38,19 Z"));

                AddModuleItem(ref ui, Properties.Resources.ModuleUserName, var.Username, ref separatorCheck, updateOnly, fontItalic: false);

                try
                {
                    if (IsNullOrWhiteSpace(user?.EmailAddress)) { italic = true; }
                    else { italic = false; }
                }
                catch { italic = true; }
                AddModuleItem(ref ui, Properties.Resources.ModuleUserEmail, moduleUIEmail, ref separatorCheck, updateOnly, fontItalic: italic);
                AddModuleItem(ref ui, Properties.Resources.ModuleUserMailbox, MailboxType(moduleUIMailbox), ref separatorCheck, updateOnly, fontItalic: italic);

                if (!updateOnly) { AddModuleItem(ref ui, ref separatorCheck); }

                AddModuleItem(ref ui, Properties.Resources.ModuleUserPWChanged, moduleUILastPWSet.ToShortDateString(), ref separatorCheck, updateOnly, fontItalic: italic,
                    icon: MenuIcon("F1 M 30.0833,20.5833C 36.2045,20.5833 41.1667,25.5455 41.1667,31.6667C 41.1667,32.9121 40.9612,34.1096 40.5824,35.2271L 41.2098,35.6999L 42.75,38.3958L 45.9562,37.294L 45.0696,43.0703L 50.0614,42.37L 49.8929,47.9923L 55.4166,45.125L 56.941,46.6616L 58.5833,54.7394L 51.9312,55.6743L 33.8897,42.0791C 32.7027,42.5131 31.4207,42.75 30.0833,42.75C 23.9622,42.75 19,37.7878 19,31.6667C 19,25.5455 23.9622,20.5833 30.0833,20.5833 Z M 26.9167,26.125C 25.605,26.125 24.5417,27.1883 24.5417,28.5C 24.5417,29.8117 25.605,30.875 26.9167,30.875C 28.2283,30.875 29.2917,29.8117 29.2917,28.5C 29.2917,27.1883 28.2283,26.125 26.9167,26.125 Z"));
                AddModuleItem(ref ui, Properties.Resources.ModuleUserPWExpires, moduleUIPWExpires, ref separatorCheck, updateOnly, moduleNotifyDateUser, ref moduleUIValue, fontItalic: italic);
                AddModuleItem(ref ui, Properties.Resources.ModuleUserAccountExpires, moduleUIAccountExpires, ref separatorCheck, updateOnly, moduleNotifyDateUser, ref moduleUIValue, fontItalic: italic);

                if (!updateOnly) { AddModuleItem(ref ui, ref separatorCheck); }

                AddModuleItem(ref ui, Properties.Resources.ModuleUserHomeDir, moduleUIHomeDirectory, ref separatorCheck, updateOnly, fontItalic: italic,
                    icon: MenuIcon("F1 M 40,44L 39.9999,51L 44,51C 45.1046,51 46,51.8954 46,53L 46,57C 46,58.1046 45.1045,59 44,59L 32,59C 30.8954,59 30,58.1046 30,57L 30,53C 30,51.8954 30.8954,51 32,51L 36,51L 36,44L 40,44 Z M 47,53L 57,53L 57,57L 47,57L 47,53 Z M 29,53L 29,57L 19,57L 19,53L 29,53 Z M 19,22L 57,22L 57,31L 19,31L 19,22 Z M 55,24L 53,24L 53,29L 55,29L 55,24 Z M 51,24L 49,24L 49,29L 51,29L 51,24 Z M 47,24L 45,24L 45,29L 47,29L 47,24 Z M 21,27L 21,29L 23,29L 23,27L 21,27 Z M 19,33L 57,33L 57,42L 19,42L 19,33 Z M 55,35L 53,35L 53,40L 55,40L 55,35 Z M 51,35L 49,35L 49,40L 51,40L 51,35 Z M 47,35L 45,35L 45,40L 47,40L 47,35 Z M 21,38L 21,40L 23,40L 23,38L 21,38 Z"));
                AddModuleItem(ref ui, Properties.Resources.ModuleUserHomeDrive, moduleUIHomeDrive, ref separatorCheck, updateOnly, fontItalic: italic);
                AddModuleItem(ref ui, Properties.Resources.ModuleUserDiskSpace, var.HomeDriveTotalSize, ref separatorCheck, updateOnly, ref moduleUIValue, fontItalic: false);

                if (!updateOnly) { AddModuleItem(ref ui, ref separatorCheck); }

                AddModuleItem(ref ui, Properties.Resources.ModuleUserPrinter, var.DefaultPrinter, ref separatorCheck, updateOnly, fontItalic: false,
                    icon: MenuIcon("F1 M 25,27L 25,17L 51,17L 51,27L 47,27L 47,21L 29,21L 29,27L 25,27 Z M 16,28L 60,28L 60,51L 52,51L 52,46L 55,46L 55,33L 21,33L 21,46L 24,46L 24,51L 16,51L 16,28 Z M 25,39L 28,39L 28,50L 35,50L 35,57L 48,57L 48,39L 51,39L 51,60L 33,60L 25,52L 25,39 Z"));

                if (!updateOnly) { AddModuleItem(ref ui, ref separatorCheck); }

                // Copy texts to clipboard
                if (!updateOnly) { ui.Items.Add(AddClipboardItem(Properties.Resources.ModuleUserInfo)); }
                else
                {
                    foreach (var subItem in ui.Items)
                    {
                        MenuItem sub;

                        try { sub = (MenuItem)subItem; }
                        catch { continue; }

                        Application.Current.Dispatcher.Invoke(delegate
                        {
                            if (sub.Name == Properties.Resources.ModuleCopyClipboard)
                            {
                                string input = GetTranslation(Properties.Resources.ModuleCopyClipboard);
                                if (sub.Header.ToString() != input) { sub.Header = input; }
                            }
                        });
                    }
                }

                // Additional slot
                if (!IsNullOrWhiteSpace(moduleUIAddHeader))
                {
                    if (!updateOnly) { ui.Items.Add(ModuleAdditional(Properties.Resources.ModuleUserAdd, moduleUIAddIcon)); }
                    else
                    {
                        foreach (var subItem in ui.Items)
                        {
                            MenuItem sub;

                            try { sub = (MenuItem)subItem; }
                            catch { continue; }

                            Application.Current.Dispatcher.Invoke(delegate
                            {
                                if (sub.Name == Properties.Resources.ModuleUserAdd)
                                {
                                    string header = VariableConvert(GetTranslation(Properties.Resources.ModuleUserAdd));
                                    string tooltip = VariableConvert(GetTranslation(Properties.Resources.ModuleUserAdd + Properties.Resources.TooltipTag));
                                    bool enabled = System.IO.File.Exists(VariableConvert(moduleUIAddExecute));

                                    if (sub.Header.ToString() != header) { sub.Header = header; }

                                    if (sub.ToolTip != null && !IsNullOrWhiteSpace(tooltip)) { sub.ToolTip = (!IsNullOrWhiteSpace(tooltip) ? tooltip : null); }
                                    if (sub.IsEnabled != enabled) { sub.IsEnabled = enabled; }
                                }
                            });
                        }
                    }
                }
            }

            if (moduleChangeUserIcon && allowUserInfo) { moduleUserIconState = moduleUIValue; }
            else { moduleUserIconState = IconType.Normal; }

            Application.Current.Dispatcher.Invoke(delegate
            { ChangeModuleIconState(ref ui, moduleUserIconState); });

            if (!updateOnly) { ModuleList.Add(ui); }
            PassUser:
            #endregion

            #region ComputerInfo
            IconType moduleCIvalue = IconType.Normal;
            MenuItem ci = null;

            if (!updateOnly) { ci = new MenuItem { Name = Properties.Resources.ModuleCompInfo, Header = GetTranslation(Properties.Resources.ModuleCompInfo) }; }
            else
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    { ci = ModuleList.Find(x => x.Name == Properties.Resources.ModuleCompInfo); });
                }
                catch { goto PassComp; }
            }

            if (!allowCompInfo)
            {
                Application.Current.Dispatcher.Invoke(delegate
                { ci.IsEnabled = false; });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(delegate
                { ci.IsEnabled = true; });

                Variable var = new Variable();
                bool separatorCheck = false;
                DateTime output;

                AddModuleItem(ref ci, Properties.Resources.ModuleCompHostname, var.Hostname, ref separatorCheck, updateOnly, true,
                    MenuIcon("F1 M 20,23.0002L 55.9998,23.0002C 57.1044,23.0002 57.9998,23.8956 57.9998,25.0002L 57.9999,46C 57.9999,47.1046 57.1045,48 55.9999,48L 41,48L 41,53L 45,53C 46.1046,53 47,53.8954 47,55L 47,57L 29,57L 29,55C 29,53.8954 29.8955,53 31,53L 35,53L 35,48L 20,48C 18.8954,48 18,47.1046 18,46L 18,25.0002C 18,23.8956 18.8954,23.0002 20,23.0002 Z M 21,26.0002L 21,45L 54.9999,45L 54.9998,26.0002L 21,26.0002 Z"));
                AddModuleItem(ref ci, Properties.Resources.ModuleCompModel, var.Model, ref separatorCheck, updateOnly, true);
                AddModuleItem(ref ci, Properties.Resources.ModuleCompSN, var.SerialNumber, ref separatorCheck, updateOnly, true);
                AddModuleItem(ref ci, Properties.Resources.ModuleCompCPU, var.Processor, ref separatorCheck, updateOnly, true);
                AddModuleItem(ref ci, Properties.Resources.ModuleCompMemory, var.Memory, ref separatorCheck, updateOnly, ref moduleCIvalue, true);

                if (!updateOnly) { AddModuleItem(ref ci, ref separatorCheck); }

                AddModuleItem(ref ci, Properties.Resources.ModuleCompAddress, var.Address, ref separatorCheck, updateOnly, true,
                    MenuIcon("F1 M 36.4167,19C 44.2867,19 50.6667,24.6711 50.6667,31.6667C 50.6667,32.7601 50.5108,33.8212 50.2177,34.8333L 36.4167,57L 22.6156,34.8333C 22.3225,33.8212 22.1667,32.7601 22.1667,31.6667C 22.1667,24.6711 28.5466,19 36.4167,19 Z M 36.4167,27.7083C 34.2305,27.7083 32.4583,29.4805 32.4583,31.6667C 32.4583,33.8528 34.2305,35.625 36.4167,35.625C 38.6028,35.625 40.375,33.8528 40.375,31.6667C 40.375,29.4805 38.6028,27.7083 36.4167,27.7083 Z"));
                AddModuleItem(ref ci, Properties.Resources.ModuleCompBuilding, var.Building, ref separatorCheck, updateOnly, true);
                AddModuleItem(ref ci, Properties.Resources.ModuleCompRoom, var.Room, ref separatorCheck, updateOnly, true);
                AddModuleItem(ref ci, Properties.Resources.ModuleCompProfit, var.ProfitCenter, ref separatorCheck, updateOnly, true);

                if (DateTime.TryParse(var.Warranty, out output))
                { AddModuleItem(ref ci, Properties.Resources.ModuleCompWarranty, output, ref separatorCheck, updateOnly, moduleNotifyDateComp, ref moduleCIvalue, reverseNotify: true); }
                else
                { AddModuleItem(ref ci, Properties.Resources.ModuleCompWarranty, var.Warranty, ref separatorCheck, updateOnly, true); }

                if (!updateOnly) { AddModuleItem(ref ci, ref separatorCheck); }

                string icon = "F1 M 17,23L 34,20.7738L 34,37L 17,37L 17,23 Z M 34,55.2262L 17,53L 17,39L 34,39L 34,55.2262 Z M 59,17.5L 59,37L 36,37L 36,20.5119L 59,17.5 Z M 59,58.5L 36,55.4881L 36,39L 59,39L 59,58.5 Z";
                byte r = 0, g = 124, b = 221;

                try
                {
                    if (Environment.OSVersion.Version.Major <= 6 && Environment.OSVersion.Version.Minor <= 1)
                    {
                        icon = "F1 M 23.75,19.7917C 23.75,19.7917 25.3333,16.625 33.25,16.625C 37.1223,16.625 39.4162,17.9403 41.4185,19.4987L 36.8968,35.0898C 34.7402,33.6397 32.5299,32.4583 30.0833,32.4583C 23.75,32.4583 20.5833,34.0417 20.5833,34.0417L 23.75,19.7917 Z M 52.25,24.5417C 60.1667,24.5417 61.75,21.375 61.75,21.375L 57,37.2083C 57,37.2083 53.8333,40.375 47.5,40.375C 44.6133,40.375 42.0555,38.7303 39.5268,36.9402L 43.9792,21.588C 46.0059,23.181 48.3116,24.5417 52.25,24.5417 Z M 19,38.7917C 19,38.7917 20.5833,35.625 28.5,35.625C 31.9766,35.625 34.181,36.6853 36.0442,38.0298L 31.5082,53.6702C 29.5528,52.4186 27.5391,51.4583 25.3333,51.4583C 19,51.4583 15.8333,53.0417 15.8333,53.0417L 19,38.7917 Z M 47.5,43.5417C 55.4167,43.5417 57,40.375 57,40.375L 52.25,56.2083C 52.25,56.2083 49.0833,59.375 42.75,59.375C 39.6233,59.375 36.8825,57.4455 34.1466,55.4916L 38.6129,40.0916C 40.8001,41.8876 43.1576,43.5417 47.5,43.5417 Z";
                        r = 14;
                        g = 76;
                        b = 133;
                    }
                }
                catch { }

                AddModuleItem(ref ci, Properties.Resources.ModuleCompOS, var.OperatingSystem, ref separatorCheck, updateOnly, true, MenuIcon(icon, r, g, b));
                AddModuleItem(ref ci, Properties.Resources.ModuleCompVersion, var.OSVersion, ref separatorCheck, updateOnly, true);
                AddModuleItem(ref ci, Properties.Resources.ModuleCompWinUpd, var.WinUpdateTime, ref separatorCheck, updateOnly, true);
                
                if (!updateOnly) { AddModuleItem(ref ci, ref separatorCheck); }


                // Collect all USB drives
                ManagementObjectCollection diskdrives = new ManagementObjectSearcher("SELECT Caption, DeviceID FROM Win32_DiskDrive WHERE InterfaceType='USB'").Get();
                List<string> usbDrives = new List<string>();

                foreach (ManagementObject diskdrive in diskdrives)
                {
                    foreach (ManagementObject disk in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + diskdrive["DeviceID"] + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                    {
                        foreach (ManagementObject partition in new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + disk["DeviceID"] + "'} WHERE AssocClass = Win32_LogicalDiskToPartition").Get())
                        { usbDrives.Add(partition["CAPTION"].ToString()); }
                    }
                }
                //

                foreach (var drive in System.IO.DriveInfo.GetDrives())
                {
                    if (drive.DriveType == System.IO.DriveType.Fixed)
                    {
                        bool skip = false;
                        foreach (string usb in usbDrives) { if (usb.Substring(0,1) == drive.RootDirectory.ToString().Substring(0,1)) { skip = true; } }
                        if (skip) { break; }

                        IShellProperty prop = ShellObject.FromParsingName(drive.Name).Properties.GetProperty("System.Volume.BitLockerProtection");
                        int? encrypt = (prop as ShellProperty<int?>).Value;

                        if (encrypt.HasValue && (encrypt == 1 || encrypt == 3 || encrypt == 5))
                        { AddModuleItem(ref ci, Properties.Resources.ModuleCompIsEncrypt, drive.Name, ref separatorCheck, updateOnly, true,
                                MenuIcon(Properties.Resources.VecktorLock)); }
                        else
                        { AddModuleItem(ref ci, Properties.Resources.ModuleCompNotEncrypt, drive.Name, ref separatorCheck, updateOnly, true,
                                MenuIcon(Properties.Resources.VecktorLockOpen)); }
                    }
                }

                if (!updateOnly) { AddModuleItem(ref ci, ref separatorCheck); }

                // Copy texts to clipboard
                if (!updateOnly) { ci.Items.Add(AddClipboardItem(Properties.Resources.ModuleCompInfo)); }
                else
                {
                    foreach (var subItem in ci.Items)
                    {
                        MenuItem sub;

                        try { sub = (MenuItem)subItem; }
                        catch { continue; }

                        Application.Current.Dispatcher.Invoke(delegate
                        {
                            if (sub.Name == Properties.Resources.ModuleCopyClipboard)
                            {
                                string input = GetTranslation(Properties.Resources.ModuleCopyClipboard);
                                if (sub.Header.ToString() != input) { sub.Header = input; }
                            }
                        });
                    }
                }

                // Additional slot
                if (!IsNullOrWhiteSpace(moduleCIAddHeader))
                {
                    if (!updateOnly) { ci.Items.Add(ModuleAdditional(Properties.Resources.ModuleCompAdd, moduleCIAddIcon)); }
                    else
                    {
                        foreach (var subItem in ci.Items)
                        {
                            MenuItem sub;

                            try { sub = (MenuItem)subItem; }
                            catch { continue; }

                            Application.Current.Dispatcher.Invoke(delegate
                            {
                                if (sub.Name == Properties.Resources.ModuleCompAdd)
                                {
                                    string header = VariableConvert(GetTranslation(Properties.Resources.ModuleCompAdd));
                                    string tooltip = VariableConvert(GetTranslation(Properties.Resources.ModuleCompAdd + Properties.Resources.TooltipTag));
                                    string execute = VariableConvert(GetTranslation(Properties.Resources.ModuleCompAdd + Properties.Resources.ExecuteTag));
                                    bool enabled = System.IO.File.Exists(execute);

                                    if (sub.Header.ToString() != header) { sub.Header = header; }
                                    if (sub.ToolTip != null && !IsNullOrWhiteSpace(tooltip)) { sub.ToolTip = (!IsNullOrWhiteSpace(tooltip) ? tooltip : null); }
                                    if (sub.IsEnabled != enabled) { sub.IsEnabled = enabled; }
                                }
                            });
                        }
                    }
                }
            }

            if (moduleChangeCompIcon && allowCompInfo) { moduleCompIconState = moduleCIvalue; }
            else { moduleCompIconState = IconType.Normal; }

            Application.Current.Dispatcher.Invoke(delegate
            { ChangeModuleIconState(ref ci, moduleCompIconState); });

            if (!updateOnly) { ModuleList.Add(ci); }
            PassComp:
            #endregion

            #region OWA
            MenuItem owa = null;

            if (!updateOnly)
            {
                owa = new MenuItem
                {
                    Name = Properties.Resources.ModuleOWA,
                    Header = GetTranslation(Properties.Resources.ModuleOWA),
                    Icon = MenuIcon(Properties.Resources.VectorOWA, 0, 113, 197)
                };
            }
            else
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    { owa = ModuleList.Find(x => x.Name == Properties.Resources.ModuleOWA); });
                }
                catch { goto PassOWA; }
            }

            if (!allowOWA)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    owa.IsEnabled = false;
                    owa.Icon = MenuIcon(Properties.Resources.VectorOWA, 128, 128, 128);
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    owa.IsEnabled = true;
                    owa.Icon = MenuIcon(Properties.Resources.VectorOWA, 0, 113, 197);
                });

                string tagValue = Empty;
                tagValue = MailboxType(GetMailboxType(user), true);

                if (tagValue == Properties.Resources.NA) { MailboxType(moduleUIMailbox, true); }
                if (tagValue == Properties.Resources.NA) { tagValue = mailboxLocal; }

                if (!updateOnly)
                {
                    owa.Tag = tagValue;
                    owa.Click += OWA_Click;
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (owa.Tag.ToString() != tagValue)
                        { owa.Tag = tagValue; }
                    });
                }
            }

            if (!updateOnly) { ModuleList.Add(owa); }
            PassOWA:
            #endregion

            #region Chat
            MenuItem ch = null;

            if (!updateOnly) { ch = new MenuItem { Name = Properties.Resources.ModuleChat, Header = GetTranslation(Properties.Resources.ModuleChat) }; }
            else { goto PassChat; }

            if (!allowChat)
            {
                Application.Current.Dispatcher.Invoke(delegate
                { ch.IsEnabled = false; });
            }
            else { ch.Click += Chat_Click; }

            ModuleList.Add(ch);
            PassChat:
            #endregion

            #region SupportRequest
            MenuItem sr = null;

            if (!updateOnly) { sr = new MenuItem { Name = Properties.Resources.ModuleSupportRequest, Header = GetTranslation(Properties.Resources.ModuleSupportRequest) }; }
            else { goto PassSupport; }

            if (!allowSupport)
            {
                Application.Current.Dispatcher.Invoke(delegate
                { sr.IsEnabled = false; });
            }
            else { sr.Click += SupportRequest_Click; }

            ModuleList.Add(sr);
            PassSupport:;
            #endregion

            // Support Request settings
            try
            {
                sendSettings.Email = user?.EmailAddress;
                sendSettings.Name = user?.DisplayName;
                sendSettings.Signature = VariableConvert(GetTranslation(Properties.Resources.UIMessageInfo));

                if (!IsNullOrWhiteSpace(sendSettings.Email) && !IsNullOrWhiteSpace(sendSettings.Name)) { sendSettings.Verified = true; }
                else { sendSettings.Verified = false; }
            }
            catch
            {
                sendSettings.Email = Empty;
                sendSettings.Name = Empty;
                sendSettings.Signature = Empty;
                sendSettings.Verified = false;
            }
            //

            WriteXML();
            if (user != null) { user.Dispose(); }
        }

        private static MenuItem AddClipboardItem(string module)
        {
            MenuItem mi = new MenuItem
            {
                Name = Properties.Resources.ModuleCopyClipboard,
                Header = GetTranslation(Properties.Resources.ModuleCopyClipboard),
                Icon = MenuIcon(Properties.Resources.VectorCopy),
                Tag = module,
                StaysOpenOnClick = true
            };

            mi.Click += ModuleClipboard_Click;

            return mi;
        }

        private static void ModuleClipboard_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender, module;
            StringBuilder sb = new StringBuilder();

            try
            {
                if (!IsNullOrWhiteSpace(mi.Tag.ToString())) { module = ModuleList.Find(item => item.Name == mi.Tag.ToString()); }
                else { return; }
            }
            catch { return; }

            foreach (var mod in module.Items)
            {
                MenuItem item;

                try
                {
                    item = (MenuItem)mod;
                    if (!item.IsVisible) { continue; }

                    if (item.Name != mi.Name) { sb.AppendLine(item.Header.ToString()); }
                    else { break; }
                }
                catch
                {
                    Separator separator;

                    try
                    {
                        separator = (Separator)mod;
                        if (separator.IsVisible) { sb.Append(Environment.NewLine); }
                    }
                    catch { continue; }
                }
            }

            try
            {
                Clipboard.SetText(sb.ToString().Trim() + Environment.NewLine);
                mi.Icon = MenuIcon("F1 M 23.7501,33.25L 34.8334,44.3333L 52.2499,22.1668L 56.9999,26.9168L 34.8334,53.8333L 19.0001,38L 23.7501,33.25 Z", 0, 127, 14);
            }
            catch
            {
                mi.Icon = MenuIcon("F1 M 26.9166,22.1667L 37.9999,33.25L 49.0832,22.1668L 53.8332,26.9168L 42.7499,38L 53.8332,49.0834L 49.0833,53.8334L 37.9999,42.75L 26.9166,53.8334L 22.1666,49.0833L 33.25,38L 22.1667,26.9167L 26.9166,22.1667 Z", 255, 0, 0);
            }
            finally
            {
                if (ClipboardIcon.IsEnabled) { ClipboardIcon.Stop(); }
                ClipboardIcon.Start();
            }
        }

        private void ClipboardIcon_Tick(object sender, EventArgs e)
        {
            ClipboardIcon.Stop();

            ChangeClipboardIcon(Properties.Resources.ModuleUserInfo);
            ChangeClipboardIcon(Properties.Resources.ModuleCompInfo);
        }

        private static void ChangeClipboardIcon(string moduleName)
        {
            try
            {
                MenuItem mod = ModuleList.Find(module => module.Name == moduleName);

                foreach (var sub in mod.Items)
                {
                    MenuItem item;

                    try
                    {
                        item = (MenuItem)sub;
                        if (item.Name == Properties.Resources.ModuleCopyClipboard) { item.Icon = MenuIcon(Properties.Resources.VectorCopy); }
                    }
                    catch { continue; }
                }
            }
            catch { return; }
        }

        private static MenuItem ModuleAdditional(string name, string icon)
        {
            string tooltip = VariableConvert(GetTranslation(name + Properties.Resources.TooltipTag));
            string execute = VariableConvert(GetTranslation(name + Properties.Resources.ExecuteTag));
            string parameters = VariableConvert(GetTranslation(name + Properties.Resources.ParametersTag));

            MenuItem mi = new MenuItem
            {
                Name = name,
                Header = VariableConvert(GetTranslation(name)),
                ToolTip = (!IsNullOrWhiteSpace(tooltip) ? tooltip : null)
            };

            if (!IsNullOrWhiteSpace(icon)) { if (System.IO.File.Exists(icon)) { mi.Icon = MenuIconFromFile(icon); } }

            if (System.IO.File.Exists(execute))
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = execute,
                    Arguments = parameters
                };

                mi.Tag = psi;
            }
            else
            { mi.IsEnabled = false; }

            mi.Click += ModuleAdditional_Click;
            return mi;
        }

        private static void ModuleAdditional_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem mi = (MenuItem)sender;
                ProcessStartInfo psi = (ProcessStartInfo)mi.Tag;

                Execute(psi);
            }
            catch { return; }
        }

        private static void AddModuleItem(ref MenuItem mi, string name, string header, ref bool check, bool update, bool showNA = false, Path icon = null, bool fontItalic = false)
        {
            if (IsNullOrWhiteSpace(header)) { return; }

            bool enabled = (header == Properties.Resources.NA ? false : true);
            bool visible = (!enabled && !showNA ? false : true);

            string[] input = GetTranslation(name).Split(';');
            string headerText = Empty;

            if (input.Length > 1) { headerText = input[0] + header + input[1]; }
            else { headerText = input[0] + header; }

            System.Windows.Media.Brush fore = null, back = null;
            bool clearValue = false;

            if (name == Properties.Resources.ModuleCompIsEncrypt)
            {
                fore = themeFore;
                back = (darkTheme ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.LawnGreen);
                clearValue = true;
            }
            if (name == Properties.Resources.ModuleCompNotEncrypt)
            {
                fore = (darkTheme ? themeFore : themeForeOpposite);
                back = System.Windows.Media.Brushes.Red;
                clearValue = (darkTheme ? true : false);
            }

            if (!IsNullOrWhiteSpace(header)) { HandleModuleItem(ref mi, name, headerText, ref check, enabled, visible, update, icon, fore, back, clearValue, fontItalic: fontItalic); }
        }

        private static void AddModuleItem(ref MenuItem mi, string name, double header, ref bool check, bool update, ref IconType iconType, bool showNA = false, Path icon = null, bool fontItalic = false)
        {
            System.Windows.Media.Brush fore = null, back = null;

            bool enabled = (header < 0 ? false : true);
            bool visible = (!enabled && !showNA ? false : true);
            bool clearValue = false;

            string headerText = DiskSpaceUnit(GetTranslation(name), header);

            if (name == Properties.Resources.ModuleUserDiskSpace && header >= 0)
            {
                Variable var = new Variable();
                double free = var.HomeDriveFreeSpace, total = var.HomeDriveTotalSize, percent = 0;

                headerText = headerText + " (" + DiskSpaceUnit(GetTranslation(Properties.Resources.ModuleUserDiskSpaceFree), var.HomeDriveFreeSpace, true) + ")";
                percent = (free / total) * 100;

                if (percent > 5)
                {
                    fore = themeFore;
                    back = (darkTheme ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.LawnGreen);
                    clearValue = true;
                }
                else if (percent <= 5 && percent > 1)
                {
                    fore = (darkTheme ? themeForeOpposite : themeFore);
                    back = (darkTheme ? System.Windows.Media.Brushes.DarkOrange : System.Windows.Media.Brushes.Orange);
                    clearValue = (darkTheme ? false : true);
                    if (iconType.GetHashCode() < 2) { iconType = IconType.Yellow; }
                }
                else
                {
                    fore = (darkTheme ? themeFore : themeForeOpposite);
                    back = System.Windows.Media.Brushes.Red;
                    clearValue = (darkTheme ? true : false);
                    if (iconType.GetHashCode() < 3) { iconType = IconType.Red; }
                }
            }

            HandleModuleItem(ref mi, name, headerText, ref check, enabled, visible, update, icon, fore, back, clearValue, fontItalic: fontItalic);
        }

        private static void AddModuleItem(ref MenuItem mi, string name, DateTime header, ref bool check, bool update, int notifyDate, ref IconType iconType, bool showNA = false, Path icon = null, bool fontItalic = false, bool reverseNotify = false)
        {
            System.Windows.Media.Brush fore = null, back = null;

            bool enabled = (header == defaultDate ? false : true);
            bool visible = (!enabled && !showNA ? false : true);
            bool clearValue = false;

            string headerText = (!enabled ? Properties.Resources.NA : GetTranslation(name) + header.ToShortDateString());
            string tooltipAdd = Empty;

            if (!reverseNotify)
            {
                if (header < DateTime.Today && enabled)
                {
                    fore = (darkTheme ? themeFore : themeForeOpposite);
                    back = System.Windows.Media.Brushes.Red;
                    clearValue = (darkTheme ? true : false);

                    if (iconType.GetHashCode() < 3) { iconType = IconType.Red; }
                }
                else if ((header - DateTime.Today).Days <= notifyDate && enabled)
                {
                    fore = (darkTheme ? themeForeOpposite : themeFore);
                    back = (darkTheme ? System.Windows.Media.Brushes.DarkOrange : System.Windows.Media.Brushes.Orange);
                    clearValue = (darkTheme ? false : true);

                    if (iconType.GetHashCode() < 2) { iconType = IconType.Yellow; }
                }
                else
                {
                    fore = themeFore;
                    back = System.Windows.Media.Brushes.Transparent;
                    clearValue = true;
                }
            }
            else
            {
                if ((DateTime.Today - header).Days > notifyDate && enabled)
                {
                    fore = (darkTheme ? themeFore : themeForeOpposite);
                    back = System.Windows.Media.Brushes.Red;
                    clearValue = (darkTheme ? true : false);

                    tooltipAdd = GetTranslation(Properties.Resources.ModuleCompWarrantyTooltipMore) + "\n" + 
                        GetTranslation(Properties.Resources.ModuleUserInstructionsTooltip);
                    if (iconType.GetHashCode() < 3) { iconType = IconType.Red; }
                }
                else if (DateTime.Today > header && enabled)
                {
                    fore = (darkTheme ? themeForeOpposite : themeFore);
                    back = (darkTheme ? System.Windows.Media.Brushes.DarkOrange : System.Windows.Media.Brushes.Orange);
                    clearValue = (darkTheme ? false : true);

                    tooltipAdd = GetTranslation(Properties.Resources.ModuleCompWarrantyTooltipLess) + "\n" + 
                        GetTranslation(Properties.Resources.ModuleUserInstructionsTooltip);
                    if (iconType.GetHashCode() < 2) { iconType = IconType.Yellow; }
                }
                else
                {
                    fore = themeFore;
                    back = System.Windows.Media.Brushes.Transparent;
                    clearValue = true;

                    tooltipAdd = GetTranslation(Properties.Resources.ModuleUserInstructionsTooltip);
                }
            }

            HandleModuleItem(ref mi, name, headerText, ref check, enabled, visible, update, icon, fore, back, clearValue, fontItalic: fontItalic, tooltipAdd: tooltipAdd);
        }

        private static void AddModuleItem(ref MenuItem mi, ref bool check)
        {
            if (check)
            {
                mi.Items.Add(new Separator());
                check = false;
            }
        }

        private static void HandleModuleItem(ref MenuItem mi, string name, string header, ref bool check, bool enabled, bool visible, bool update, Path icon = null,
            System.Windows.Media.Brush fore = null, System.Windows.Media.Brush back = null, bool clearValue = false, bool fontItalic = false, string tooltipAdd = "")
        {
            MenuItem item = null;

            if (!update)
            {
                item = new MenuItem { Name = name, Header = header, ToolTip = GetTranslation(Properties.Resources.ModuleTooltip) };

                if (!SpecialModuleItems(ref item, name, MenuItemHandling.New, tooltipAdd)) { item.Click += ModuleItem_Click; }
            }
            else
            {
                foreach (var subItem in mi.Items)
                {
                    MenuItem sub;

                    try { sub = (MenuItem)subItem; }
                    catch { continue; }

                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (sub.Name == name)
                        { item = sub; }
                    });
                }

                string tooltip = (!IsNullOrWhiteSpace(tooltipAdd) ? tooltipAdd : GetTranslation(Properties.Resources.ModuleTooltip));

                Application.Current.Dispatcher.Invoke(delegate
                {
                    if (item == null) { return; }

                    if (item.Header.ToString() != header) { item.Header = header; }
                });

                if (!SpecialModuleItems(ref item, name, MenuItemHandling.Update, tooltip) || !IsNullOrWhiteSpace(tooltipAdd))
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        try { if (item.ToolTip.ToString() != tooltip) { item.ToolTip = tooltip; } }
                        catch { item.ToolTip = null; }
                    });
                }
            }

            Application.Current.Dispatcher.Invoke(delegate
            {
                item.IsEnabled = enabled;
                item.Visibility = (visible ? Visibility.Visible : Visibility.Collapsed);
                if (icon != null) { item.Icon = icon; }

                if (fontItalic) { item.FontStyle = FontStyles.Italic; }
                else { item.FontStyle = FontStyles.Normal; }

                if (enabled)
                {
                    if (fore != null) { item.Foreground = fore; }
                    if (back != null) { item.Background = back; }
                }
                else
                {
                    if (item.Visibility == Visibility.Visible)
                    {
                        item.Foreground = System.Windows.Media.Brushes.Gray;
                        item.Background = defaultBack;
                    }
                }

                if (clearValue) { item.ClearValue(ForegroundProperty); }
            });

            if (!update) { mi.Items.Add(item); }
            if (!check) { if (visible) { check = true; } }
        }

        private static bool SpecialModuleItems(ref MenuItem mi, string name, MenuItemHandling handling, string tooltip = "")
        {
            if (name == Properties.Resources.ModuleUserHomeDir || name == Properties.Resources.ModuleUserHomeDrive || name == Properties.Resources.ModuleUserDiskSpace)
            {
                SpecialModuleItemsHandle(ref mi, GetTranslation(Properties.Resources.ModuleUserHomeTooltip), moduleUIHomeDrive, handling);
                return true;
            }
            else if (name == Properties.Resources.ModuleUserPWChanged || name == Properties.Resources.ModuleUserPWExpires)
            {
                SpecialModuleItemsHandle(ref mi, GetTranslation(Properties.Resources.ModuleUserInstructionsTooltip), Properties.Resources.ModuleUserPasswordUrl, handling);
                return true;
            }
            else if (name == Properties.Resources.ModuleUserAccountExpires)
            {
                SpecialModuleItemsHandle(ref mi, GetTranslation(Properties.Resources.ModuleUserInstructionsTooltip), Properties.Resources.ModuleUserAccountUrl, handling);
                return true;
            }
            else if (name == Properties.Resources.ModuleCompWarranty)
            {
                string tooltipText = GetTranslation(Properties.Resources.ModuleUserInstructionsTooltip);

                if (!IsNullOrWhiteSpace(tooltip)) { tooltipText = tooltip; }

                SpecialModuleItemsHandle(ref mi, tooltipText, Properties.Resources.ModuleCompWarrantyUrl, handling);
                return true;
            }
            else { return false; }
        }

        private static void SpecialModuleItemsHandle(ref MenuItem mi, string tooltip, string execute, MenuItemHandling handling)
        {
            switch (handling)
            {
                case MenuItemHandling.New:
                    mi.ToolTip = tooltip;
                    mi.PreviewMouseLeftButtonDown += SpecialModuleItem_MouseLeftButtonDown;
                    mi.PreviewMouseRightButtonDown += ModuleItem_Click;
                    break;
                case MenuItemHandling.Update:
                    MenuItem item = mi;

                    Application.Current.Dispatcher.Invoke(delegate { if (item.ToolTip.ToString() != tooltip) { item.ToolTip = tooltip; } });
                    break;
                case MenuItemHandling.Execute:
                    if (System.IO.Directory.Exists(execute)) { Execute(execute, Empty); }
                    else { Execute(GetTranslation(execute), Empty); }
                    break;
                default:
                    return;
            }

        }

        private static void ModuleItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi;

            try
            {
                mi = (MenuItem)sender;
                Clipboard.SetText(mi.Header.ToString());
            }
            catch
            { return; }
        }

        private static void SpecialModuleItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;

            SpecialModuleItems(ref mi, mi.Name, MenuItemHandling.Execute);
        }

        private static void ChangeModuleIconState(ref MenuItem module, IconType iconState)
        {
            ChangeMenuItemNotification(ref module, iconState);
            ChangeNotificationIcon(iconState, Empty, ref iconState, true, true);
        }

        private static TimeSpan GetMaxPasswordAge()
        {
            //xparrot @ http://houseofderek.blogspot.fi/2008/07/password-expiration-email-utility.html

            using (Domain d = Domain.GetCurrentDomain())
            using (DirectoryEntry domain = d.GetDirectoryEntry())
            {
                domain.AuthenticationType = AuthenticationTypes.Secure;

                DirectorySearcher ds = new DirectorySearcher(domain, "(objectClass=*)", null, SearchScope.Base);
                SearchResult sr = ds.FindOne();
                TimeSpan maxPwdAge = TimeSpan.MinValue;
                if (sr.Properties.Contains("maxPwdAge"))
                    maxPwdAge = TimeSpan.FromTicks((long)sr.Properties["maxPwdAge"][0]);
                return maxPwdAge.Duration();
            }
        }

        private static double GetMailboxType(UserPrincipal user)
        {
            try
            {
                using (DirectoryEntry entry = user.GetUnderlyingObject() as DirectoryEntry)
                {
                    entry.AuthenticationType = AuthenticationTypes.Secure;

                    return GetAttribute(entry, "msExchRecipientTypeDetails");
                };
            }
            catch { return -1; }
        }

        private static string MailboxType(double type, bool returnAddress = false)
        {
            try
            {
                switch (Convert.ToUInt32(type))
                {
                    case 1:
                        return (!returnAddress ? GetTranslation(Properties.Resources.ModuleUserMailboxLocal) : mailboxLocal);
                    case 2147483648:
                        return (!returnAddress ? GetTranslation(Properties.Resources.ModuleUserMailboxCloud) : mailboxCloud);
                    default:
                        return Properties.Resources.NA;
                }
            }
            catch { return Properties.Resources.NA; }
        }

        private static long GetAttribute(DirectoryEntry entry, string attr)
        {
            //dunnry @ http://forums.asp.net/t/999913.aspx
            //we will use the marshaling behavior of
            //the searcher

            using (DirectorySearcher ds = new DirectorySearcher(
                entry, Format("({0}=*)", attr), new string[] { attr }, SearchScope.Base))
            {
                SearchResult sr;

                try
                { sr = ds.FindOne(); }
                catch
                { sr = null; }

                if (sr != null)
                {
                    if (sr.Properties.Contains(attr)) return (long)sr.Properties[attr][0];
                }
                return -1;
            }
        }

        private static void OWA_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;

            if (!IsNullOrWhiteSpace(mi.Tag.ToString())) { Execute(mi.Tag.ToString(), Empty); }
        }

        private static void Chat_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(Popup.Chat);
        }

        private static void SupportRequest_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(Popup.SupportRequest);
        }

        public static List<MenuItem> ModuleList = new List<MenuItem>();

        private static DispatcherTimer ClipboardIcon = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };

        private enum MenuItemHandling
        {
            New,
            Update,
            Execute
        };
    }
}
