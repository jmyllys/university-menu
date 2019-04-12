using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.Management;
using static System.String;
using static System.Environment;

namespace University_Menu
{
    public partial class MainWindow
    {
        public static void BuildModules(bool updateOnly = false)
        {
            if (!updateOnly) { ModuleList.Clear(); }
            SearchResult user = null, other = null;
            
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry("LDAP://ad.helsinki.fi:636")
                { AuthenticationType = AuthenticationTypes.Secure | AuthenticationTypes.SecureSocketsLayer | AuthenticationTypes.ServerBind })
                using (DirectorySearcher search = new DirectorySearcher(entry)
                { Filter = "(&(objectClass=user)(objectCategory=person)(samaccountname=" + UserName + "))" })
                {
                    search.PropertiesToLoad.Add("*");
                    user = search.FindOne();

                    search.Filter = "maxPwdAge=*";
                    other = search.FindOne();
                }
            }
            catch
            { }

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

                moduleUIDisplayName = user?.Properties["displayname"][0].ToString() ?? moduleUIDisplayName;

                moduleUIEmail = user?.Properties["mail"][0].ToString() ?? moduleUIEmail;
                try { moduleUIMailbox = Convert.ToDouble(user.Properties["msExchRecipientTypeDetails"][0]); }
                catch { }

                try
                {
                    long value = (long)user?.Properties["pwdlastset"][0];
                    moduleUILastPWSet = DateTime.FromFileTimeUtc(value);
                }
                catch { }
                try
                {
                    TimeSpan maxPwdAge = TimeSpan.MinValue;
                    if (other.Properties.Contains("maxpwdage")) { maxPwdAge = TimeSpan.FromTicks((long)other.Properties["maxpwdage"][0]).Duration(); }

                    moduleUIPWExpires = moduleUILastPWSet.AddDays(maxPwdAge.Days);
                }
                catch { }

                try
                {
                    long value = (long)user?.Properties["accountexpires"][0];
                    moduleUIAccountExpires = DateTime.FromFileTimeUtc(value);
                }
                catch { }

                try { moduleUIHomeDirectory = user?.Properties["homedirectory"][0].ToString() ?? moduleUIHomeDirectory; }
                catch { }

                try { moduleUIHomeDrive = user?.Properties["homedrive"][0].ToString() ?? moduleUIHomeDrive; }
                catch { }

                try
                {
                    if (IsNullOrWhiteSpace(user?.Properties["displayname"][0].ToString())) { italic = true; }
                    else { italic = false; }
                }
                catch { italic = true; }
                AddModuleItem(ref ui, Properties.Resources.ModuleUserFullName, moduleUIDisplayName, ref separatorCheck, updateOnly, fontItalic: italic,
                    icon: MenuIcon("M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z"));

                AddModuleItem(ref ui, Properties.Resources.ModuleUserName, var.Username, ref separatorCheck, updateOnly, fontItalic: false);

                try
                {
                    if (IsNullOrWhiteSpace(user?.Properties["mail"][0].ToString())) { italic = true; }
                    else { italic = false; }
                }
                catch { italic = true; }
                AddModuleItem(ref ui, Properties.Resources.ModuleUserEmail, moduleUIEmail, ref separatorCheck, updateOnly, fontItalic: italic);
                AddModuleItem(ref ui, Properties.Resources.ModuleUserMailbox, MailboxType(moduleUIMailbox), ref separatorCheck, updateOnly, fontItalic: italic);

                if (!updateOnly) { AddModuleItem(ref ui, ref separatorCheck); }

                AddModuleItem(ref ui, Properties.Resources.ModuleUserPWChanged, moduleUILastPWSet.ToShortDateString(), ref separatorCheck, updateOnly, fontItalic: italic,
                    icon: MenuIcon("M22,18V22H18V19H15V16H12L9.74,13.74C9.19,13.91 8.61,14 8,14A6,6 0 0,1 2,8A6,6 0 0,1 8,2A6,6 0 0,1 14,8C14,8.61 13.91,9.19 13.74,9.74L22,18M7,5A2,2 0 0,0 5,7A2,2 0 0,0 7,9A2,2 0 0,0 9,7A2,2 0 0,0 7,5Z"));
                AddModuleItem(ref ui, Properties.Resources.ModuleUserPWExpires, moduleUIPWExpires, ref separatorCheck, updateOnly, moduleNotifyDateUser, ref moduleUIValue, fontItalic: italic);
                AddModuleItem(ref ui, Properties.Resources.ModuleUserAccountExpires, moduleUIAccountExpires, ref separatorCheck, updateOnly, moduleNotifyDateUser, ref moduleUIValue, fontItalic: italic);

                if (!updateOnly) { AddModuleItem(ref ui, ref separatorCheck); }

                AddModuleItem(ref ui, Properties.Resources.ModuleUserHomeDir, moduleUIHomeDirectory, ref separatorCheck, updateOnly, fontItalic: italic,
                    icon: MenuIcon("M15,20A1,1 0 0,0 14,19H13V17H19A2,2 0 0,0 21,15V7A2,2 0 0,0 19,5H13L11,3H5A2,2 0 0,0 3,5V15A2,2 0 0,0 5,17H11V19H10A1,1 0 0,0 9,20H2V22H9A1,1 0 0,0 10,23H14A1,1 0 0,0 15,22H22V20H15M5,15V5H10.17L11.59,6.41L12.17,7H13L19,7V15H5Z"));
                AddModuleItem(ref ui, Properties.Resources.ModuleUserHomeDrive, moduleUIHomeDrive, ref separatorCheck, updateOnly, fontItalic: italic);
                AddModuleItem(ref ui, Properties.Resources.ModuleUserDiskSpace, var.HomeDriveTotalSize, ref separatorCheck, updateOnly, ref moduleUIValue, fontItalic: false);

                if (!updateOnly) { AddModuleItem(ref ui, ref separatorCheck); }

                AddModuleItem(ref ui, Properties.Resources.ModuleUserPrinter, var.DefaultPrinter, ref separatorCheck, updateOnly, fontItalic: false,
                    icon: MenuIcon("M18,3H6V7H18M19,12A1,1 0 0,1 18,11A1,1 0 0,1 19,10A1,1 0 0,1 20,11A1,1 0 0,1 19,12M16,19H8V14H16M19,8H5A3,3 0 0,0 2,11V17H6V21H18V17H22V11A3,3 0 0,0 19,8Z"));

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

                AddModuleItem(ref ci, Properties.Resources.ModuleCompHostname, var.Hostname, ref separatorCheck, updateOnly, true,
                    MenuIcon("F1 M 20,23.0002L 55.9998,23.0002C 57.1044,23.0002 57.9998,23.8956 57.9998,25.0002L 57.9999,46C 57.9999,47.1046 57.1045,48 55.9999,48L 41,48L 41,53L 45,53C 46.1046,53 47,53.8954 47,55L 47,57L 29,57L 29,55C 29,53.8954 29.8955,53 31,53L 35,53L 35,48L 20,48C 18.8954,48 18,47.1046 18,46L 18,25.0002C 18,23.8956 18.8954,23.0002 20,23.0002 Z M 21,26.0002L 21,45L 54.9999,45L 54.9998,26.0002L 21,26.0002 Z"));
                AddModuleItem(ref ci, Properties.Resources.ModuleCompModel, var.Model, ref separatorCheck, updateOnly, true);
                AddModuleItem(ref ci, Properties.Resources.ModuleCompSN, var.SerialNumber, ref separatorCheck, updateOnly, true);
                AddModuleItem(ref ci, Properties.Resources.ModuleCompCPU, var.Processor, ref separatorCheck, updateOnly, true);
                AddModuleItem(ref ci, Properties.Resources.ModuleCompMemory, var.Memory, ref separatorCheck, updateOnly, ref moduleCIvalue, true);
                AddModuleItem(ref ci, Properties.Resources.ModuleCompWifiMAC, var.MACAddressWireless, ref separatorCheck, updateOnly, false,
                    MenuIcon("M12,21L15.6,16.2C14.6,15.45 13.35,15 12,15C10.65,15 9.4,15.45 8.4,16.2L12,21M12,3C7.95,3 4.21,4.34 1.2,6.6L3,9C5.5,7.12 8.62,6 12,6C15.38,6 18.5,7.12 21,9L22.8,6.6C19.79,4.34 16.05,3 12,3M12,9C9.3,9 6.81,9.89 4.8,11.4L6.6,13.8C8.1,12.67 9.97,12 12,12C14.03,12 15.9,12.67 17.4,13.8L19.2,11.4C17.19,9.89 14.7,9 12,9Z"));

                if (!updateOnly) { AddModuleItem(ref ci, ref separatorCheck); }

                AddModuleItem(ref ci, Properties.Resources.ModuleCompAddress, var.Address, ref separatorCheck, updateOnly, true,
                    MenuIcon("F1 M 36.4167,19C 44.2867,19 50.6667,24.6711 50.6667,31.6667C 50.6667,32.7601 50.5108,33.8212 50.2177,34.8333L 36.4167,57L 22.6156,34.8333C 22.3225,33.8212 22.1667,32.7601 22.1667,31.6667C 22.1667,24.6711 28.5466,19 36.4167,19 Z M 36.4167,27.7083C 34.2305,27.7083 32.4583,29.4805 32.4583,31.6667C 32.4583,33.8528 34.2305,35.625 36.4167,35.625C 38.6028,35.625 40.375,33.8528 40.375,31.6667C 40.375,29.4805 38.6028,27.7083 36.4167,27.7083 Z"));
                AddModuleItem(ref ci, Properties.Resources.ModuleCompBuilding, var.Building, ref separatorCheck, updateOnly, true);
                AddModuleItem(ref ci, Properties.Resources.ModuleCompRoom, var.Room, ref separatorCheck, updateOnly, true);
                AddModuleItem(ref ci, Properties.Resources.ModuleCompProfit, var.ProfitCenter, ref separatorCheck, updateOnly, true);

                if (DateTime.TryParse(var.Warranty, out DateTime outputWarranty))
                { AddModuleItem(ref ci, Properties.Resources.ModuleCompWarranty, outputWarranty, ref separatorCheck, updateOnly, moduleNotifyDateComp, ref moduleCIvalue, reverseNotify: true); }
                else
                { AddModuleItem(ref ci, Properties.Resources.ModuleCompWarranty, var.Warranty, ref separatorCheck, updateOnly, true); }

                if (!updateOnly) { AddModuleItem(ref ci, ref separatorCheck); }

                string icon = "F1 M 17,23L 34,20.7738L 34,37L 17,37L 17,23 Z M 34,55.2262L 17,53L 17,39L 34,39L 34,55.2262 Z M 59,17.5L 59,37L 36,37L 36,20.5119L 59,17.5 Z M 59,58.5L 36,55.4881L 36,39L 59,39L 59,58.5 Z";
                byte r = 0, g = 124, b = 221;

                try
                {
                    if (OSVersion.Version.Major <= 6 && OSVersion.Version.Minor <= 1)
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

                if (DateTime.TryParse(var.WinUpdateTime, out DateTime outputWinUpd))
                { AddModuleItem(ref ci, Properties.Resources.ModuleCompWinUpd, Convert.ToDateTime(outputWinUpd), ref separatorCheck, updateOnly, checkupMinDays, ref moduleCIvalue, reverseNotify:true); }
                else
                { AddModuleItem(ref ci, Properties.Resources.ModuleCompWinUpd, var.WinUpdateTime, ref separatorCheck, updateOnly, true); }

                AddModuleItem(ref ci, Properties.Resources.ModuleCompCert, var.CompCertValid, ref separatorCheck, updateOnly, moduleNotifyDateComp, ref moduleCIvalue, reverseNotify: false);

                if (!updateOnly) { AddModuleItem(ref ci, ref separatorCheck); }

                foreach (var disk in GetCryptedDisks())
                {
                    if (disk.Crypted)
                    { AddModuleItem(ref ci, Properties.Resources.ModuleCompIsEncrypt, disk.Name, ref separatorCheck, updateOnly, true, MenuIcon(Properties.Resources.VecktorLock));}
                    else
                    { AddModuleItem(ref ci, Properties.Resources.ModuleCompNotEncrypt, disk.Name, ref separatorCheck, updateOnly, true, MenuIcon(Properties.Resources.VecktorLockOpen));}
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
                tagValue = MailboxType(moduleUIMailbox, true);

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

            // Support Request settings
            try
            {
                sendSettings.Email = moduleUIEmail;
                sendSettings.Name = moduleUIDisplayName;
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
            #endregion

            WriteXML();
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
                if (name == Properties.Resources.ModuleCompWinUpd)
                {
                    Variable var = new Variable();

                    if (!DateTime.TryParse(var.WinUpdateSearch, out DateTime output)) { output = DateTime.Now; }
                    double comparison = (DateTime.Now - output).TotalDays;

                    if (comparison >= checkupMinDays)
                    {
                        if (comparison > checkupMaxDays)
                        {
                            fore = (darkTheme ? themeFore : themeForeOpposite);
                            back = System.Windows.Media.Brushes.Red;
                            clearValue = (darkTheme ? true : false);

                            if (iconType.GetHashCode() < 3) { iconType = IconType.Red; }
                        }
                        else
                        {
                            fore = (darkTheme ? themeForeOpposite : themeFore);
                            back = (darkTheme ? System.Windows.Media.Brushes.DarkOrange : System.Windows.Media.Brushes.Orange);
                            clearValue = (darkTheme ? false : true);

                            if (iconType.GetHashCode() < 2) { iconType = IconType.Yellow; }
                        }
                    }
                    else
                    {
                        fore = themeFore;
                        back = System.Windows.Media.Brushes.Transparent;
                        clearValue = true;

                        tooltipAdd = GetTranslation(Properties.Resources.ModuleUserInstructionsTooltip);
                    }
                }
                else if ((DateTime.Today - header).Days > notifyDate && enabled)
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

        public static List<DiskCryptInfo> GetCryptedDisks()
        {
            List<DiskCryptInfo> diskList = new List<DiskCryptInfo>();
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

            foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
            {
                if (drive.DriveType == System.IO.DriveType.Fixed)
                {
                    bool skip = false;
                    foreach (string usb in usbDrives) { if (usb.Substring(0, 1) == drive.RootDirectory.ToString().Substring(0, 1)) { skip = true; } }
                    if (skip) { continue; }

                    IShellProperty prop = ShellObject.FromParsingName(drive.Name).Properties.GetProperty("System.Volume.BitLockerProtection");
                    int? encrypt = (prop as ShellProperty<int?>).Value;

                    DiskCryptInfo diskInfo = new DiskCryptInfo { Name = drive.Name };

                    if (encrypt.HasValue && (encrypt == 1 || encrypt == 3 || encrypt == 5)) { diskInfo.Crypted = true; }
                    else { diskInfo.Crypted = false; }

                    diskList.Add(diskInfo);
                }
            }

            return diskList;
        }

        public class DiskCryptInfo
        {
            public string Name { get; set; }
            public bool Crypted = false;
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
