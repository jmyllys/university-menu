using System;
using System.Windows;
using System.Windows.Controls;

namespace University_Menu.Pages
{
    /// <summary>
    /// Interaction logic for Debugging_Tools.xaml
    /// </summary>
    public partial class Debugging_Tools : UserControl
    {
        public Debugging_Tools()
        {
            InitializeComponent();
        }

        private void DTBalloonCheckup_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ShowNotification(false, MainWindow.Popup.Checkup, true);
        }

        private void DTBalloonNetwork_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ShowNotification(false, MainWindow.Popup.NetworkSpace, true);
        }

        private void DTBalloonReboot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ShowNotification(false, MainWindow.Popup.RebootPending, true);
        }

        private void DTBalloonWelcome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ShowNotification(false, MainWindow.Popup.Welcome, true);
        }

        private void DTPopupCheckup_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CheckCheckup(false, Convert.ToInt32(DTCheckupDays.Text));
            MainWindow.ShowNotification(true, MainWindow.Popup.Checkup, true);
        }

        private void DTPopupNetwork_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CheckNetworkSpace(false, Convert.ToInt32(DTNetworkMB.Text));
            MainWindow.ShowNotification(true, MainWindow.Popup.NetworkSpace, true);
        }

        private void DTPopupReboot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CheckRebootPending(false, true);
            MainWindow.ShowNotification(true, MainWindow.Popup.RebootPending, true);
        }

        private void DTPopupRoaming_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CheckRoamingProfile(Convert.ToUInt32(DTRoamingStatus.Text));
        }

        private void DTPopupWarranty_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(DTPopupStatus.Text)) { MainWindow.CheckWarrantyExpired(Convert.ToDateTime(DTPopupStatus.Text)); }
            else { MainWindow.CheckWarrantyExpired(MainWindow.defaultDate); }
        }
    }
}
