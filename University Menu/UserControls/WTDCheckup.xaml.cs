using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.String;

namespace University_Menu.UserControls
{
    /// <summary>
    /// Interaction logic for NotifyWhatToDo.xaml
    /// </summary>
    public partial class WTDCheckup : UserControl
    {
        public WTDCheckup(MainWindow.Languages language)
        {
            InitializeComponent();

            wtdTitle.Text = MainWindow.GetTranslation(Properties.Resources.UICheckupWTDTitle, language.GetHashCode());
            wtdTitle.Foreground = MainWindow.themeFore;
            wtdConnect.Text = MainWindow.GetTranslation(Properties.Resources.UICheckupWTDConnect, language.GetHashCode());
            wtdConnect.Foreground = MainWindow.themeFore;
            pathConnect.Fill = MainWindow.themeFore;

            if (File.Exists(MainWindow.notifyVPNExecute))
            {
                borderVPN.Visibility = Visibility.Visible;
                wtdVPN.Text = MainWindow.GetTranslation(Properties.Resources.UICheckupWTDVPN, language.GetHashCode());
                wtdVPN.Foreground = MainWindow.themeFore;
                pathVPN.Fill = MainWindow.themeFore;
            }
            else { borderVPN.Visibility = Visibility.Collapsed; }

            string web = MainWindow.GetTranslation(Properties.Resources.UICheckupURL, language.GetHashCode());

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

        private void borderConnect_MouseEnter(object sender, MouseEventArgs e)
        {
            borderConnect.Background = UI.enterColor;
        }

        private void borderConnect_MouseLeave(object sender, MouseEventArgs e)
        {
            borderConnect.Background = Brushes.Transparent;
        }

        private void borderVPN_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            borderVPN.Background = UI.enterColor;
        }

        private void borderVPN_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            borderVPN.Background = Brushes.Transparent;
        }

        private void borderVPN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsNullOrWhiteSpace(MainWindow.notifyVPNExecute))
            { MainWindow.Execute(MainWindow.notifyVPNExecute, MainWindow.notifyVPNParameters); }
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
