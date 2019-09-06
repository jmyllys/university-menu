using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.String;

namespace University_Menu.UserControls
{
    /// <summary>
    /// Interaction logic for WTDOSUpgrade.xaml
    /// </summary>
    public partial class WTDOSUpgrade : UserControl
    {
        public WTDOSUpgrade(MainWindow.Languages language)
        {
            InitializeComponent();

            wtdTitle.Text = MainWindow.GetTranslation(Properties.Resources.UICheckupWTDTitle, language.GetHashCode());
            wtdTitle.Foreground = MainWindow.themeFore;
            string web = Empty;

            switch (MainWindow.OSGroup)
            {
                case MainWindow.OSUpgradeGroups.InPlace:
                    web = MainWindow.GetTranslation(Properties.Resources.UIOSUpgradeURL, language.GetHashCode());
                    wtdWeb.Text = "Read more instructions on the Helpdesk Portal";

                    pathSoftware.Data = Geometry.Parse("M3,12V6.75L9,5.43V11.91L3,12M20,3V11.75L10,11.9V5.21L20,3M3,13L9,13.09V19.9L3,18.75V13M20,13.25V22L10,20.09V13.1L20,13.25Z");
                    wtdSoftware.Foreground = MainWindow.themeFore;
                    pathSoftware.Fill = MainWindow.themeFore;
                    //borderSoftware.Tag = (@"C:\Windows\ccm\ClientUX\SCClient.exe", "softwarecenter:Page=OSD");
                    borderSoftware.Visibility = Visibility.Visible;
                    break;
                case MainWindow.OSUpgradeGroups.Poistuvat:
                    web = MainWindow.GetTranslation(Properties.Resources.UIOSUpgradeURL2, language.GetHashCode());
                    wtdWeb.Text = "Read more instructions about replacement on the Flamma";

                    wtdSoftware.Text = "Read more instructions about recyle on the Flamma";
                    pathSoftware.Data = Geometry.Parse("M21.82,15.42L19.32,19.75C18.83,20.61 17.92,21.06 17,21H15V23L12.5,18.5L15,14V16H17.82L15.6,12.15L19.93,9.65L21.73,12.77C22.25,13.54 22.32,14.57 21.82,15.42M9.21,3.06H14.21C15.19,3.06 16.04,3.63 16.45,4.45L17.45,6.19L19.18,5.19L16.54,9.6L11.39,9.69L13.12,8.69L11.71,6.24L9.5,10.09L5.16,7.59L6.96,4.47C7.37,3.64 8.22,3.06 9.21,3.06M5.05,19.76L2.55,15.43C2.06,14.58 2.13,13.56 2.64,12.79L3.64,11.06L1.91,10.06L7.05,10.14L9.7,14.56L7.97,13.56L6.56,16H11V21H7.4C6.47,21.07 5.55,20.61 5.05,19.76Z");
                    wtdSoftware.Foreground = MainWindow.themeFore;
                    pathSoftware.Fill = MainWindow.themeFore;
                    //borderSoftware.Tag = "https://flamma.helsinki.fi/en/group/it-ja-puhelin/recycling-and-removal-of-devices";
                    borderSoftware.Visibility = Visibility.Visible;
                    break;
                default:
                    borderSoftware.Visibility = Visibility.Collapsed;
                    break;
            }

            if (!IsNullOrWhiteSpace(web))
            {
                borderWeb.Visibility = Visibility.Visible;
                wtdWeb.Foreground = MainWindow.themeFore;
                pathWeb.Fill = MainWindow.themeFore;
                borderWeb.Tag = web;
            }
            else { borderWeb.Visibility = Visibility.Collapsed; }
        }

        private void borderWeb_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            borderWeb.Background = UI.enterColor;
        }

        private void borderWeb_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            borderWeb.Background = Brushes.Transparent;
        }

        private void borderWeb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsNullOrWhiteSpace(borderWeb.Tag.ToString()))
            { MainWindow.Execute(borderWeb.Tag.ToString(), Empty); }
        }

        private void BorderSoftware_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            borderSoftware.Background = UI.enterColor;
        }

        private void BorderSoftware_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            borderSoftware.Background = Brushes.Transparent;
        }

        private void BorderSoftware_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (MainWindow.OSGroup)
            {
                case MainWindow.OSUpgradeGroups.InPlace:
                    MainWindow.Execute(@"C:\Windows\ccm\ClientUX\SCClient.exe", "softwarecenter:Page=OSD");
                    break;
                case MainWindow.OSUpgradeGroups.Poistuvat:
                    MainWindow.Execute("https://flamma.helsinki.fi/en/group/it-ja-puhelin/recycling-and-removal-of-devices", Empty);
                    break;
                default:
                    return;
            }
        }
    }
}
