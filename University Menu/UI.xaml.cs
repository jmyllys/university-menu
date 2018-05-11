using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Windows;
using System.Windows.Media;
using University_Menu.Pages;
using static System.String;

namespace University_Menu
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI : ModernWindow
    {
        public static MainWindow.Languages usedLanguage = MainWindow.uiLanguage;
        public static Brush enterColor = Brushes.AliceBlue;
        public static bool saveSettings = false;

        public UI(MainWindow.Popup source)
        {
            InitializeComponent();

            usedLanguage = MainWindow.uiLanguage;
            enterColor = new SolidColorBrush(MainWindow.themeColor);

            int count = 0;
            foreach (int status in MainWindow.notifyStatus) { if (status > 0) { count++; } }

            LinkGroupCollection links = new LinkGroupCollection();

            LinkGroup notify = new LinkGroup { DisplayName = MainWindow.GetTranslation(Properties.Resources.UINotifyTitle) + $" ({count})" };

            if ((MainWindow.checkupIconState.GetHashCode() > 0 && MainWindow.allowCheckup) || source == MainWindow.Popup.Checkup)
            {
                notify.Links.Add(new Link
                {
                    DisplayName = MainWindow.GetTranslation(Properties.Resources.DefaultCheckup),
                    Source = new Uri("/Pages/CheckupTab.xaml", UriKind.Relative)
                });
            }
            if ((MainWindow.networkIconState.GetHashCode() > 0 && MainWindow.allowNetwork) || source == MainWindow.Popup.NetworkSpace)
            {
                notify.Links.Add(new Link
                {
                    DisplayName = MainWindow.GetTranslation(Properties.Resources.DefaultNetwork),
                    Source = new Uri("/Pages/NetworkTab.xaml", UriKind.Relative)
                });
            }
            if ((MainWindow.rebootIconState.GetHashCode() > 0 && MainWindow.allowReboot) || source == MainWindow.Popup.RebootPending)
            {
                notify.Links.Add(new Link
                {
                    DisplayName = MainWindow.GetTranslation(Properties.Resources.DefaultReboot),
                    Source = new Uri("/Pages/RebootTab.xaml", UriKind.Relative)
                });
            }
            if ((MainWindow.roamingIconState.GetHashCode() > 0 && MainWindow.allowRoaming) || source == MainWindow.Popup.RoamingProfile)
            {
                notify.Links.Add(new Link
                {
                    DisplayName = MainWindow.GetTranslation(Properties.Resources.DefaultRoaming),
                    Source = new Uri("/Pages/RoamingTab.xaml", UriKind.Relative)
                });
            }
            if ((MainWindow.warrantyIconState.GetHashCode() > 0 && MainWindow.allowWarranty) || source == MainWindow.Popup.WarrantyExpired)
            {
                notify.Links.Add(new Link
                {
                    DisplayName = MainWindow.GetTranslation(Properties.Resources.DefaultWarranty),
                    Source = new Uri("/Pages/WarrantyTab.xaml", UriKind.Relative)
                });
            }
            if (count == 0 || notify.Links.Count == 0)
            {
                notify.Links.Add(new Link
                {
                    DisplayName = MainWindow.GetTranslation(Properties.Resources.UINoNotify),
                    Source = new Uri("/Pages/NoNotifications.xaml", UriKind.Relative)
                });
            }

            links.Add(notify);

            if (MainWindow.allowSupport)
            {
                LinkGroup support = new LinkGroup { DisplayName = MainWindow.GetTranslation(Properties.Resources.UISupportTitle) };
                support.Links.Add(new Link
                {
                    DisplayName = MainWindow.GetTranslation(Properties.Resources.UISupportForm),
                    Source = new Uri("/Pages/SupportRequest.xaml", UriKind.Relative)
                });
                support.Links.Add(new Link
                {
                    DisplayName = MainWindow.GetTranslation(Properties.Resources.UISupportPreview),
                    Source = new Uri("/Pages/SupportPreview.xaml", UriKind.Relative)
                });
                links.Add(support);
            }
            
            MenuLinkGroups.Clear();
            foreach (var link in links) { MenuLinkGroups.Add(link); }

            switch (source)
            {
                case MainWindow.Popup.Checkup:
                    ContentSource = new Uri("/Pages/CheckupTab.xaml", UriKind.Relative);
                    break;
                case MainWindow.Popup.NetworkSpace:
                    ContentSource = new Uri("/Pages/NetworkTab.xaml", UriKind.Relative);
                    break;
                case MainWindow.Popup.RebootPending:
                    ContentSource = new Uri("/Pages/RebootTab.xaml", UriKind.Relative);
                    break;
                case MainWindow.Popup.RoamingProfile:
                    ContentSource = new Uri("/Pages/RoamingTab.xaml", UriKind.Relative);
                    break;
                case MainWindow.Popup.WarrantyExpired:
                    ContentSource = new Uri("/Pages/WarrantyTab.xaml", UriKind.Relative);
                    break;
                case MainWindow.Popup.SupportRequest:
                    ContentSource = new Uri("/Pages/SupportRequest.xaml", UriKind.Relative);
                    break;
                case MainWindow.Popup.Settings:
                    ContentSource = new Uri("/Pages/SettingsPage.xaml", UriKind.Relative);
                    break;
                default:
                    break;
            }

            LinkCollection titles = new LinkCollection();
            titles.Add(new Link { DisplayName = MainWindow.GetTranslation(Properties.Resources.UISettings),
                Source = new Uri("/Pages/SettingsPage.xaml", UriKind.Relative) });

            titles.Add(new Link { DisplayName = MainWindow.GetTranslation(Properties.Resources.UIHelp),
                Source = new Uri(MainWindow.GetTranslation(Properties.Resources.UIHelpUrl), UriKind.Absolute) });

            TitleLinks.Clear();
            foreach (var title in titles) { TitleLinks.Add(title); }
        }

        private void ModernWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            MainWindow.sendSettings.Message = Empty;
            if (saveSettings) { MainWindow.WriteXML(); }
        }
    }
}
