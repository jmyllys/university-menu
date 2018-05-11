using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace University_Menu.UserControls
{
    /// <summary>
    /// Interaction logic for RebootNow.xaml
    /// </summary>
    public partial class WTDReboot : UserControl
    {
        public WTDReboot(MainWindow.Languages language)
        {
            InitializeComponent();

            wtdTitle.Text = MainWindow.GetTranslation(Properties.Resources.UICheckupWTDTitle, language.GetHashCode());
            textConfirm.Text = MainWindow.GetTranslation(Properties.Resources.UIRestartConfirm, language.GetHashCode());
            buttonNow.Content = MainWindow.GetTranslation(Properties.Resources.UIRestartButton, language.GetHashCode());

            wtdTitle.Foreground = MainWindow.themeFore;
        }

        private void checkConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (checkConfirm.IsChecked.Value) { buttonNow.IsEnabled = true; }
            else { buttonNow.IsEnabled = false; }
        }

        private void buttonNow_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo("shutdown.exe", "/r /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }
    }
}
