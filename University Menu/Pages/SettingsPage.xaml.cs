using FirstFloor.ModernUI.Presentation;
using System;
using System.Windows.Controls;

namespace University_Menu.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            InitializeComponent();

            LinkCollection list = new LinkCollection();
            list.Add(new Link { DisplayName = MainWindow.GetTranslation(Properties.Resources.UISettingsAppearance),
                Source = new Uri("/Pages/Settings/Appearance.xaml", UriKind.Relative) });
            list.Add(new Link { DisplayName = MainWindow.GetTranslation(Properties.Resources.UISettingsAbout),
                Source = new Uri("/Pages/Settings/About.xaml", UriKind.Relative) });

            settingsTab.Links.Clear();
            foreach (var item in list) { settingsTab.Links.Add(item); }
        }
    }
}
