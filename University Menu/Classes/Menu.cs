using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using static System.String;

namespace University_Menu
{
    public partial class MainWindow
    {
        public void BuildMenu(bool skipDefaultItems = true, bool skipBuildModules = true)
        {
            ReadXML(xmlFile, Properties.Resources.NodeLocalSettings, XML.Settings);

            TranslationItems.Clear();
            GatherTranslations();

            IconType reset = IconType.Normal;
            ChangeNotificationIcon(IconType.Normal, Empty, ref reset, true);

            if (!skipDefaultItems)
            {
                DefaultMenuItems();
                BuildModules();
            }
            if (!skipBuildModules) { BuildModules(); }

            MenuItemObjects.Clear();
            menuItemCount = 0;
            menuItemSubTag = 0;
            zero = 0;
            GatherMenu();

            MenuItemList.Clear();
            UMmenu.Items.Clear();
            ConstructMenu();
        }

        #region Defaults
        private void DefaultMenuItems()
        {
            PopupItemObjects.Clear();
            int i = 10;

            // Hostname
            MenuItem hostname = new MenuItem
            {
                Header = TBHostname,
                Name = Properties.Resources.DefaultHost,
                Tag = SortClass.abc.ToString() + i,
                StaysOpenOnClick = true,
                Icon = MenuIcon("F1 M 20,23.0002L 55.9998,23.0002C 57.1044,23.0002 57.9998,23.8956 57.9998,25.0002L 57.9999,46C 57.9999,47.1046 57.1045,48 55.9999,48L 41,48L 41,53L 45,53C 46.1046,53 47,53.8954 47,55L 47,57L 29,57L 29,55C 29,53.8954 29.8955,53 31,53L 35,53L 35,48L 20,48C 18.8954,48 18,47.1046 18,46L 18,25.0002C 18,23.8956 18.8954,23.0002 20,23.0002 Z M 21,26.0002L 21,45L 54.9999,45L 54.9998,26.0002L 21,26.0002 Z")
            };
            TBHostname.Foreground = themeFore;
            hostname.MouseDoubleClick += Hostname_MouseDoubleClick;
            defaultFore = hostname.Foreground;
            defaultBack = hostname.Background;

            //
            string tag = SortClass.cab.ToString();

            // Notifications
            MenuItem checkup = new MenuItem
            {
                Header = GetTranslation(Properties.Resources.DefaultCheckup),
                Name = Properties.Resources.DefaultCheckup,
                Tag = tag + i++,
                Visibility = Visibility.Collapsed,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            checkup.Click += Notification_Click;

            MenuItem network = new MenuItem
            {
                Header = GetTranslation(Properties.Resources.DefaultNetwork),
                Name = Properties.Resources.DefaultNetwork,
                Tag = tag + i++,
                Visibility = Visibility.Collapsed,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            network.Click += Notification_Click;

            MenuItem reboot = new MenuItem
            {
                Header = GetTranslation(Properties.Resources.DefaultReboot),
                Name = Properties.Resources.DefaultReboot,
                Tag = tag + i++,
                Visibility = Visibility.Collapsed,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            reboot.Click += Notification_Click;

            MenuItem roaming = new MenuItem
            {
                Header = GetTranslation(Properties.Resources.DefaultRoaming),
                Name = Properties.Resources.DefaultRoaming,
                Tag = tag + i++,
                Visibility = Visibility.Collapsed,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            roaming.Click += Notification_Click;

            MenuItem warranty = new MenuItem
            {
                Header = GetTranslation(Properties.Resources.DefaultWarranty),
                Name = Properties.Resources.DefaultWarranty,
                Tag = tag + i++,
                Visibility = Visibility.Collapsed,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            warranty.Click += Notification_Click;

            // Settings
            MenuItem lang = new MenuItem
            {
                Header = GetTranslation(Properties.Resources.DefaultLang),
                Name = Properties.Resources.DefaultLang,
                Tag = tag + i++,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            lang.Items.Add(DefaultMenuItemsSubmenu(Properties.Resources.DefaultLangEN, "English", Languages.English, "pack://application:,,,/Resources/MenuEN.ico"));
            lang.Items.Add(DefaultMenuItemsSubmenu(Properties.Resources.DefaultLangFI, "Suomi", Languages.Suomi, "pack://application:,,,/Resources/MenuFI.ico"));
            lang.Items.Add(DefaultMenuItemsSubmenu(Properties.Resources.DefaultLangSV, "Svenska", Languages.Svenska, "pack://application:,,,/Resources/MenuSV.ico"));
            lang.Items.Add(new Separator { Name = Properties.Resources.DefaultLangSeparator });

            MenuItem appearance = new MenuItem
            {
                Header = GetTranslation(Properties.Resources.UISettingsAppearance),
                Name = Properties.Resources.UISettingsAppearance,
                Tag = tag + i++,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            appearance.Click += Notification_Click;
            lang.Items.Add(appearance);

            // Add items to list
            DefaultMenuItemList.Add(hostname);
            DefaultMenuItemList.Add(checkup);
            DefaultMenuItemList.Add(network);
            DefaultMenuItemList.Add(reboot);
            DefaultMenuItemList.Add(roaming);
            DefaultMenuItemList.Add(warranty);
            DefaultMenuItemList.Add(lang);

            PopupItemObjects.Add(new PopupItemObject { Tag = checkup.Tag.ToString(), Popup = Popup.Checkup });
            PopupItemObjects.Add(new PopupItemObject { Tag = network.Tag.ToString(), Popup = Popup.NetworkSpace });
            PopupItemObjects.Add(new PopupItemObject { Tag = reboot.Tag.ToString(), Popup = Popup.RebootPending });
            PopupItemObjects.Add(new PopupItemObject { Tag = appearance.Tag.ToString(), Popup = Popup.Settings });
            PopupItemObjects.Add(new PopupItemObject { Tag = roaming.Tag.ToString(), Popup = Popup.RoamingProfile });
            PopupItemObjects.Add(new PopupItemObject { Tag = warranty.Tag.ToString(), Popup = Popup.WarrantyExpired });

            LanguageMenuItemCheck(true);
        }

        private MenuItem DefaultMenuItemsSubmenu(string name, string text, Languages tag, string iconPath)
        {
            Image icon = new Image
            { Source = new BitmapImage(new Uri(iconPath, UriKind.Absolute)) };
            RenderOptions.SetBitmapScalingMode(icon, BitmapScalingMode.NearestNeighbor);

            MenuItem sub = new MenuItem
            {
                Header = text,
                StaysOpenOnClick = true,
                Name = name,
                Tag = tag,
                VerticalContentAlignment = VerticalAlignment.Center,
                Icon = icon
            };
            sub.Click += Language_Click;

            return sub;
        }

        public static void LanguageMenuItemCheck(bool skipUpdateTranslations = false)
        {
            MenuItem lang = DefaultMenuItemList.Find(item => item.Name == Properties.Resources.DefaultLang);
            MenuItem en = (MenuItem)lang.Items[Languages.English.GetHashCode()];
            MenuItem fi = (MenuItem)lang.Items[Languages.Suomi.GetHashCode()];
            MenuItem sv = (MenuItem)lang.Items[Languages.Svenska.GetHashCode()];
            Separator separator = (Separator)lang.Items[Languages.Svenska.GetHashCode() + 1];

            string path;

            switch (uiLanguage)
            {
                case Languages.Suomi:
                    en.IsChecked = false;
                    fi.IsChecked = true;
                    sv.IsChecked = false;
                    path = "pack://application:,,,/Resources/MenuFI.ico";
                    break;
                case Languages.Svenska:
                    en.IsChecked = false;
                    fi.IsChecked = false;
                    sv.IsChecked = true;
                    path = "pack://application:,,,/Resources/MenuSV.ico";
                    break;
                default:
                    en.IsChecked = true;
                    fi.IsChecked = false;
                    sv.IsChecked = false;
                    path = "pack://application:,,,/Resources/MenuEN.ico";
                    break;
            }

            if (forceLanguage >= 0)
            {
                en.Visibility = Visibility.Collapsed;
                fi.Visibility = Visibility.Collapsed;
                sv.Visibility = Visibility.Collapsed;
                separator.Visibility = Visibility.Collapsed;
                lang.Icon = null;
            }
            else
            {
                en.Visibility = Visibility.Visible;
                fi.Visibility = Visibility.Visible;
                sv.Visibility = Visibility.Visible;
                separator.Visibility = Visibility.Visible;

                Image img = new Image { Source = new BitmapImage(new Uri(path, UriKind.Absolute)) };
                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.NearestNeighbor);
                lang.Icon = img;
            }

            if (!skipUpdateTranslations) { ExecuteWorker(true); }
        }

        private static void UpdateDefaultMenuItems()
        {
            MenuItem mi;

            Application.Current.Dispatcher.Invoke(delegate
            {
                foreach (var item in DefaultMenuItemList)
                {
                    try
                    {
                        mi = (MenuItem)item;

                        string value = GetTranslation(mi.Name);
                        if (value == Properties.Resources.NA || value == GetTranslation(Properties.Resources.NameErrorText)) { continue; }
                        else { mi.Header = value; }
                        
                        if (mi.HasItems)
                        {
                            MenuItem sub;

                            foreach (var child in mi.Items)
                            {
                                try
                                {
                                    sub = (MenuItem)child;

                                    string check = GetTranslation(sub.Name);
                                    if (check == Properties.Resources.NA || check == GetTranslation(Properties.Resources.NameErrorText)) { continue; }
                                    else { sub.Header = check; }
                                }
                                catch { continue; }
                            }
                        }
                    }
                    catch { continue; }
                }
            });
        }
        #endregion

        #region Gather
        public static void GatherMenu()
        {
            ReadXML(xmlFile, Properties.Resources.NodeLocalMenu, XML.Local);

            GetherAdditionalMenu(Directory.GetFiles(xmlFolder, Properties.Resources.XMLFileFilter, SearchOption.TopDirectoryOnly));
            GetherAdditionalMenu(Directory.GetFiles(xmlFolderUser, Properties.Resources.XMLFileFilter, SearchOption.TopDirectoryOnly));
        }

        private static void GetherAdditionalMenu(string[] files)
        {
            if (files.Length > 0)
            { foreach (string file in files) { ReadXML(file, Properties.Resources.NodeAddMenu, XML.Local); } }
        }

        private static void GatherMenuReadXml(XmlNode nodes, ref int subTag)
        {
            int subTagCurrent = subTag;

            foreach (XmlNode node in nodes?.ChildNodes)
            {
                switch (node.Name.ToLower())
                {
                    case "item":
                        MenuItemObjects.Add(GatherMenuListObject(node, menuItemCount++, subTagCurrent, menuItemSubTag));
                        break;
                    case "submenu":
                        MenuItemObjects.Add(GatherMenuListObject(node, menuItemCount++, subTagCurrent, ++menuItemSubTag, true));

                        if (node.HasChildNodes) { GatherMenuReadXml(node, ref menuItemSubTag); }
                        break;
                    default:
                        continue;
                }
            }
        }

        public static ListObject GatherMenuListObject(XmlNode node, int count, int subCurrent, int subTag, bool submenu = false)
        {
            ListObject item = new ListObject();
            string tag = SortClass.bca.ToString();

            try
            {
                string headerEN = node.Attributes["Header"]?.InnerText ?? Empty;

                item.ID = tag + count;
                item.IsModule = CheckIfModule(ref item, headerEN);

                if (!item.IsModule)
                {
                    TranslationItems.Add(AddTranslation(item.ID + Properties.Resources.HeaderTag,
                        headerEN, node.Attributes["HeaderFI"]?.InnerText ?? headerEN, node.Attributes["HeaderSV"]?.InnerText ?? headerEN));
                    item.Header = VariableConvert(GetTranslation(item.ID + Properties.Resources.HeaderTag));

                    string tooltipEN = node.Attributes["ToolTip"]?.InnerText ?? Empty;
                    TranslationItems.Add(AddTranslation(item.ID + Properties.Resources.TooltipTag,
                        tooltipEN, node.Attributes["ToolTipFI"]?.InnerText ?? tooltipEN, node.Attributes["ToolTipSV"]?.InnerText ?? tooltipEN));
                    item.ToolTip = VariableConvert(GetTranslation(item.ID + Properties.Resources.TooltipTag));

                    string executeEN = node.Attributes["Execute"]?.InnerText ?? Empty;
                    TranslationItems.Add(AddTranslation(item.ID + Properties.Resources.TooltipTag,
                        tooltipEN, node.Attributes["ToolTipFI"]?.InnerText ?? tooltipEN, node.Attributes["ToolTipSV"]?.InnerText ?? tooltipEN));
                    item.ToolTip = VariableConvert(GetTranslation(item.ID + Properties.Resources.TooltipTag));
                }

                item.Icon = node.Attributes["Icon"]?.InnerText;
                item.SortTag = tag + node.Attributes["SortTag"]?.InnerText;
                item.SubTag = subCurrent;
                item.SubTagLink = subTag;

                item.FontBold = BoolValue(node.Attributes["FontBold"]?.InnerText);
                item.FontItalic = BoolValue(node.Attributes["FontItalic"]?.InnerText);

                item.IsEnabled = (item.IsModule ? true : false);
                item.IsSubmenu = submenu;
                if (!submenu && !item.IsModule)
                {
                    string executeEN = node.Attributes["Execute"]?.InnerText ?? Empty;
                    TranslationItems.Add(AddTranslation(item.ID + Properties.Resources.ExecuteTag,
                        executeEN, node.Attributes["ExecuteFI"]?.InnerText ?? executeEN, node.Attributes["ExecuteSV"]?.InnerText ?? executeEN));
                    item.Execute = VariableConvert(GetTranslation(item.ID + Properties.Resources.ExecuteTag));

                    string paramatersEN = node.Attributes["Parameters"]?.InnerText ?? Empty;
                    TranslationItems.Add(AddTranslation(item.ID + Properties.Resources.ParametersTag,
                        paramatersEN, node.Attributes["ParametersFI"]?.InnerText ?? paramatersEN, node.Attributes["ParametersSV"]?.InnerText ?? paramatersEN));
                    item.Parameters = VariableConvert(GetTranslation(item.ID + Properties.Resources.ParametersTag));

                    item.ForceEnable = BoolValue(node.Attributes["ForceEnable"]?.InnerText);
                }
            }
            catch
            {
                item.ID = tag + count;
                item.IsModule = false;

                TranslationItems.Add(AddTranslation(item.ID + Properties.Resources.HeaderTag,
                    GetTranslation(Properties.Resources.NameErrorText, Languages.English.GetHashCode()), 
                    GetTranslation(Properties.Resources.NameErrorText, Languages.Suomi.GetHashCode()),
                    GetTranslation(Properties.Resources.NameErrorText, Languages.Svenska.GetHashCode())));
                item.Header = VariableConvert(GetTranslation(item.ID + Properties.Resources.HeaderTag));
                TranslationItems.Add(AddTranslation(item.ID + Properties.Resources.TooltipTag, Empty, Empty, Empty));
                item.ToolTip = Empty;

                item.Icon = Empty;
                item.SortTag = tag;
                item.SubTag = subCurrent;
                item.SubTagLink = subTag;

                item.FontBold = false;
                item.FontItalic = false;

                item.Execute = Empty;
                item.Parameters = Empty;
                item.ForceEnable = false;

                item.IsEnabled = false;
                item.IsSubmenu = false;
            }

            return item;
        }

        private static bool CheckIfModule(ref ListObject item, string input)
        {
            try
            {
                MenuItem mod = ModuleList.Find(module => "[" + module.Name.ToLower() + "]" == input.ToLower());

                if (mod != null)
                {
                    item.ID = mod.Name;
                    return true;
                }
                else { return false; }
            }
            catch { return false; }
        }
        #endregion

        #region Construct
        public static void ConstructMenu()
        {
            string sortA = SortClass.abc.ToString(), sortB = SortClass.bca.ToString(), sortC = SortClass.cab.ToString();

            // Top
            var filtersA = from MenuItem in DefaultMenuItemList
                           where MenuItem.Tag.ToString().Substring(0, sortA.Length) == sortA
                           orderby MenuItem.Tag ascending
                           select MenuItem;

            foreach (var filter in filtersA) { UMmenu.Items.Add(filter); }

            // Middle
            var filtersB1 = from ListObject in MenuItemObjects
                            where ListObject.SubTag == 0
                            orderby ListObject.SortTag ascending
                            select ListObject;

            foreach (var filter in filtersB1)
            {
                if (IsNullOrWhiteSpace(GetTranslation(filter.ID + Properties.Resources.HeaderTag, Languages.English.GetHashCode()))) { continue; }

                MenuItemList.Add(ConstructMenuItem(filter, sortB));
            }

            var filtersB2 = from MenuItem in MenuItemList
                            where MenuItem.Tag.ToString().Substring(0, sortB.Length) == sortB
                            orderby MenuItem.Tag ascending
                            select MenuItem;

            if (filtersB2.Count() > 0) { UMmenu.Items.Add(new Separator { Tag = sortB }); }

            foreach (var filter in filtersB2)
            {
                ListObject lo = MenuItemObjects.First(item => item.ID == filter.Name);

                if (GetTranslation(lo.ID + Properties.Resources.HeaderTag, Languages.English.GetHashCode()) == Properties.Resources.SeparatorTag && !lo.IsSubmenu)
                { UMmenu.Items.Add(new Separator { Tag = filter.Tag }); }
                else
                { UMmenu.Items.Add(filter); }
            }

            // Bottom
            UMmenu.Items.Add(new Separator { Tag = sortC });

            var filtersC = from MenuItem in DefaultMenuItemList
                           where MenuItem.Tag.ToString().Substring(0, sortC.Length) == sortC
                           orderby MenuItem.Tag ascending
                           select MenuItem;

            foreach (var filter in filtersC) { UMmenu.Items.Add(filter); }
        }

        private static MenuItem ConstructMenuItem(ListObject item, string sort)
        {
            if (item.IsModule)
            {
                try
                {
                    MenuItem mod = ModuleList.Find(module => module.Name.ToLower() == item.ID.ToLower());
                    mod.Tag = item.SortTag;

                    return mod;
                }
                catch
                { return new MenuItem { Header = Properties.Resources.NA, Name = item.ID, Tag = item.SortTag, IsEnabled = false }; }
            }
            
            MenuItem mi = new MenuItem
            {
                Header = item.Header,
                ToolTip = (!IsNullOrWhiteSpace(item.ToolTip) ? item.ToolTip : null),
                Name = item.ID,
                Tag = item.SortTag,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            if (item.FontBold) { mi.FontWeight = FontWeights.Bold; }
            else { mi.FontWeight = FontWeights.Normal; }

            if (item.FontItalic) { mi.FontStyle = FontStyles.Italic; }
            else { mi.FontStyle = FontStyles.Normal; }

            if (!IsNullOrWhiteSpace(item.Icon)) { if (File.Exists(item.Icon)) { mi.Icon = MenuIconFromFile(item.Icon); } }

            if (item.IsSubmenu)
            {
                var filters = from ListObject in MenuItemObjects
                              where ListObject.SortTag.Substring(0, sort.Length) == sort && ListObject.SubTag == item.SubTagLink
                              orderby ListObject.SortTag ascending
                              select ListObject;

                if (filters.Count() == 0)
                { mi.IsEnabled = false; }
                else
                {
                    foreach (var filter in filters)
                    {
                        string input = GetTranslation(filter.ID + Properties.Resources.HeaderTag, Languages.English.GetHashCode());

                        if (IsNullOrWhiteSpace(input)) { continue; }

                        if (input == Properties.Resources.SeparatorTag && !filter.IsSubmenu)
                        { mi.Items.Add(new Separator { Name = filter.ID, Tag = filter.SortTag }); }
                        else
                        { mi.Items.Add(ConstructMenuItem(filter, sort)); }
                    }
                }
            }
            else
            { mi.IsEnabled = MenuItemEnableCheck(item); 
            }

            item.IsEnabled = mi.IsEnabled;
            if (!mi.HasItems) { mi.Click += MenuItem_Click; }
            return mi;
        }
        #endregion

        public static bool MenuItemEnableCheck(ListObject item)
        {
            if (item.Header.ToString() == Properties.Resources.NA || item.Header.ToString() == GetTranslation(Properties.Resources.NameErrorText)) { return false; }

            if (!IsNullOrWhiteSpace(item.Execute))
            {
                if (!item.ForceEnable)
                {
                    string input = VariableConvert(item.Execute);

                    if (!File.Exists(input) && !Directory.Exists(input) && !ExecuteCheckAllow(input)) { return false; }
                }
                return true;
            }
            else { return false; }
        }

        private static bool ExecuteCheckAllow(string tag)
        {
            if (tag.Length < 4) { return false; }

            return (tag.Substring(0, 4) == "http" || tag.Substring(0, 4) == "www." ? true : false);
        }

        public static void UpdateMenuItems()
        {
            foreach (ListObject obj in MenuItemObjects.ToList())
            {
                string id = (obj.IsModule ? obj.ID : obj.ID + Properties.Resources.HeaderTag);

                string input = GetTranslation(id, Languages.English.GetHashCode());
                if (IsNullOrWhiteSpace(input) || input == Properties.Resources.SeparatorTag) { continue; }

                string header = VariableConvert(GetTranslation(id)) ?? Properties.Resources.NA, tooltip = Empty;
                if (!obj.IsModule) { tooltip = VariableConvert(GetTranslation(obj.ID + Properties.Resources.TooltipTag)) ?? Empty; }

                obj.Execute = GetTranslation(obj.ID + Properties.Resources.ExecuteTag);
                obj.Parameters = GetTranslation(obj.ID + Properties.Resources.ParametersTag);

                bool headerChanged = false, tooltipChanged = false, isEnabledChanged = false;

                if (header != obj.Header)
                {
                    obj.Header = header;
                    headerChanged = true;
                }
                if (tooltip != obj.ToolTip && !obj.IsModule)
                {
                    obj.ToolTip = tooltip;
                    tooltipChanged = true;
                }

                if (!obj.IsSubmenu && !obj.IsModule)
                {
                    bool boolValue = MenuItemEnableCheck(obj);
                    if (boolValue != obj.IsEnabled)
                    {
                        obj.IsEnabled = boolValue;
                        isEnabledChanged = true;
                    }
                }

                if (headerChanged || tooltipChanged || isEnabledChanged)
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    { FindMenuItem(obj, headerChanged, tooltipChanged, isEnabledChanged); });
                }
            }
        }

        private static void FindMenuItem(ListObject obj, bool header, bool tooltip, bool isEnabled)
        {
            MenuItem mi;
            try { mi = MenuItemList.Find(item => item.Name == obj.ID); }
            catch { return; }

            if (mi != null)
            { UpdateMenuItem(mi, obj, header, tooltip, isEnabled); }
            else
            {
                List<MenuItem> subitems;
                bool found = false;

                try { subitems = MenuItemList.FindAll(item => item.HasItems); }
                catch { return; }

                if (subitems != null)
                {
                    foreach (var sub in subitems)
                    {
                        FindSubMenuItem(sub.Items, obj, header, tooltip, isEnabled, ref found);
                        if (found) { return; }
                    }
                }
            }
        }

        private static void FindSubMenuItem(ItemCollection collection, ListObject obj, bool header, bool tooltip, bool isEnabled, ref bool found)
        {
            foreach (var item in collection)
            {
                if (found) { return; }

                MenuItem mi;
                try { mi = (MenuItem)item; }
                catch { continue; }

                if (mi != null)
                {
                    if (mi.Name == obj.ID)
                    {
                        UpdateMenuItem(mi, obj, header, tooltip, isEnabled);
                        found = true;
                        return;
                    }
                    if (mi.HasItems) { FindSubMenuItem(mi.Items, obj, header, tooltip, isEnabled, ref found); }
                }
            }
        }

        private static void UpdateMenuItem(MenuItem mi, ListObject obj, bool header, bool tooltip, bool isEnabled)
        {
            if (header) { mi.Header = obj.Header; }
            if (tooltip) { mi.ToolTip = (!IsNullOrWhiteSpace(obj.ToolTip) ? obj.ToolTip : null); }
            if (isEnabled) { mi.IsEnabled = obj.IsEnabled; }
        }

        private void Hostname_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TBHostname.SelectAll();
        }

        private void Language_Click(object sender, RoutedEventArgs e)
        {
            MenuItem click = (MenuItem)sender;

            if (!click.IsChecked)
            {
                uiLanguage = (Languages)click.Tag;
                WriteXML();
                LanguageMenuItemCheck();
            }
        }

        private void Notification_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem mi = (MenuItem)sender;

                PopupItemObject pio = PopupItemObjects.Find(item => item.Tag == mi.Tag.ToString());
                OpenWindow(pio.Popup);
            }
            catch { return; }
        }

        private static void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi;

            try
            {
                mi = (MenuItem)sender;

                var obj = MenuItemObjects.Find(item => item.ID == mi.Name);
                Execute(VariableConvert(obj.Execute), VariableConvert(obj.Parameters));
            }
            catch { return; }
        }

        public class ListObject
        {
            public string ID { get; set; }
            public string Header { get; set; }
            public string ToolTip { get; set; }
            public string Execute { get; set; }
            public string Parameters { get; set; }
            public bool ForceEnable { get; set; }
            public bool FontBold { get; set; }
            public bool FontItalic { get; set; }
            public string Icon { get; set; }
            public string SortTag { get; set; }
            public int SubTagLink { get; set; }
            public int SubTag { get; set; }
            public bool IsSubmenu { get; set; }
            public bool IsEnabled { get; set; }
            public bool IsModule { get; set; }
        }

        public class PopupItemObject
        {
            public string Tag { get; set; }
            public Popup Popup { get; set; }
        }

        public enum SortClass
        {
            abc,
            bca,
            cab
        }

        public static List<MenuItem> DefaultMenuItemList = new List<MenuItem>();
        public static List<PopupItemObject> PopupItemObjects = new List<PopupItemObject>();
        public static List<MenuItem> MenuItemList = new List<MenuItem>();
        public static List<ListObject> MenuItemObjects = new List<ListObject>();

        private static TextBox TBHostname = new TextBox
        {
            Text = Environment.MachineName.ToUpper(),
            FontWeight = FontWeights.Bold,
            IsReadOnly = true,
            BorderThickness = new Thickness(0),
            AutoWordSelection = true
        };
    }
}