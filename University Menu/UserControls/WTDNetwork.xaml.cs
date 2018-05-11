using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.String;

namespace University_Menu.UserControls
{
    /// <summary>
    /// Interaction logic for NotifyWhatToDoNetwork.xaml
    /// </summary>
    public partial class WTDNetwork : UserControl
    {
        public static string folder = MainWindow.VariableConvert(MainWindow.notifyLocalFolder);

        public WTDNetwork(MainWindow.Languages language)
        {
            InitializeComponent();

            wtdTitle.Text = MainWindow.GetTranslation(Properties.Resources.UICheckupWTDTitle, language.GetHashCode());
            wtdTitle.Foreground = MainWindow.themeFore;
            wtdHome.Text = MainWindow.GetTranslation(Properties.Resources.UINetworkWTDHome, language.GetHashCode());
            wtdHome.Foreground = MainWindow.themeFore;
            pathHome.Fill = MainWindow.themeFore;

            if (Directory.Exists(folder))
            {
                borderHYData.Visibility = Visibility.Visible;
                wtdHYData.Text = MainWindow.GetTranslation(Properties.Resources.UINetworkWTDHYData, language.GetHashCode());
                wtdHYData.Foreground = MainWindow.themeFore;
                pathHYData.Fill = MainWindow.themeFore;
            }
            else { borderHYData.Visibility = Visibility.Collapsed; }

            string web = MainWindow.GetTranslation(Properties.Resources.UINetworkURL, language.GetHashCode());

            if (!IsNullOrWhiteSpace(web))
            {
                borderWeb.Visibility = Visibility.Visible;
                wtdWeb.Text = MainWindow.GetTranslation(Properties.Resources.UICheckupWTDWeb, language.GetHashCode());
                wtdWeb.Foreground = MainWindow.themeFore;
                pathWeb.Fill = MainWindow.themeFore;
                borderWeb.Tag = web;
            }
            else { borderWeb.Visibility = Visibility.Collapsed; }
        }

        private void borderHome_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            borderHome.Background = UI.enterColor;
        }

        private void borderHome_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            borderHome.Background = Brushes.Transparent;
        }

        private void borderHome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.AppStarting;

            using (UserPrincipal user = UserPrincipal.Current)
            { MainWindow.Execute(user.HomeDrive, Empty); }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void borderHYData_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            borderHYData.Background = UI.enterColor;
        }

        private void borderHYData_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            borderHYData.Background = Brushes.Transparent;
        }

        private void borderHYData_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.Execute(folder, Empty);
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
    }
}
