using FirstFloor.ModernUI.Windows.Navigation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.String;
using static System.Environment;

namespace University_Menu.UserControls
{
    /// <summary>
    /// Interaction logic for NotifyContactHelpdesk.xaml
    /// </summary>
    public partial class ContactHelpdesk : UserControl
    {
        private static string messageText = Empty;

        public ContactHelpdesk(MainWindow.Languages language, string message)
        {
            InitializeComponent();

            messageText = message;

            helpdeskTitle.Text = MainWindow.GetTranslation(Properties.Resources.UIHelpdeskTitle, language.GetHashCode());
            helpdeskTitle.Foreground = MainWindow.themeFore;
            helpdeskPhone.Text = MainWindow.notifyHelpdeskPhone;
            helpdeskPhone.Foreground = MainWindow.themeFore;
            pathPhone.Fill = MainWindow.themeFore;
            helpdeskEmail.Text = MainWindow.notifyHelpdeskEmail;
            helpdeskEmail.Foreground = MainWindow.themeFore;
            pathEmail.Fill = MainWindow.themeFore;

            if (MainWindow.allowSupport)
            {
                borderSupport.Visibility = Visibility.Visible;
                helpdeskSupport.Text = MainWindow.GetTranslation(Properties.Resources.UIHelpdeskSupport, language.GetHashCode());
                helpdeskSupport.Foreground = MainWindow.themeFore;
                pathSupport.Fill = MainWindow.themeFore;
            }
            else
            { borderSupport.Visibility = Visibility.Collapsed; }
            
            if (MainWindow.allowChat)
            {
                borderChat.Visibility = Visibility.Visible;
                helpdeskChat.Foreground = MainWindow.themeFore;
                pathChat.Fill = MainWindow.themeFore;
            }
            else { borderChat.Visibility = Visibility.Collapsed; }
        }

        private void borderPhone_MouseEnter(object sender, MouseEventArgs e)
        {
            borderPhone.Background = UI.enterColor;
        }

        private void borderPhone_MouseLeave(object sender, MouseEventArgs e)
        {
            borderPhone.Background = Brushes.Transparent;
        }

        private void borderEmail_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            borderEmail.Background = UI.enterColor;
        }

        private void borderEmail_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            borderEmail.Background = Brushes.Transparent;
        }

        private void borderEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.Execute("mailto:" + MainWindow.notifyHelpdeskEmail, Empty);
        }

        private void borderChat_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            borderChat.Background = UI.enterColor;
        }

        private void borderChat_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            borderChat.Background = Brushes.Transparent;
        }

        private void borderChat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.OpenWindow(MainWindow.Popup.Chat);
        }

        private void borderSupport_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            borderSupport.Background = UI.enterColor;
        }

        private void borderSupport_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            borderSupport.Background = Brushes.Transparent;
        }

        private void borderSupport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sendSettings.Message = NewLine + NewLine +
                Properties.Resources.UISupportAutoStart + NewLine +
                messageText + NewLine +
                Properties.Resources.UISupportAutoEnd;

            IInputElement target = NavigationHelper.FindFrame("_top", this);
            NavigationCommands.GoToPage.Execute("/Pages/SupportRequest.xaml", target);
        }
    }
}
