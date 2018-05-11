using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using University_Menu.UserControls;

namespace University_Menu.Pages.RoamingProfile
{
    /// <summary>
    /// Interaction logic for RoamingFI.xaml
    /// </summary>
    public partial class RoamingFI : UserControl
    {
        public RoamingFI()
        {
            InitializeComponent();

            rtbName.Foreground = MainWindow.themeFore;

            ContactHelpdesk ch = new ContactHelpdesk(MainWindow.Languages.Suomi,
                new TextRange(rtbSubTitle.ContentStart, rtbSubTitle.ContentEnd).Text);
            notifyContact.Children.Add(ch);

            checkExclude.IsChecked = MainWindow.roamingExclude;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            checkExclude.IsChecked = MainWindow.roamingExclude;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            MainWindow.Execute(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void checkExclude_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            MainWindow.roamingExclude = cb.IsChecked.Value;
            MainWindow.WriteXML();
        }
    }
}
