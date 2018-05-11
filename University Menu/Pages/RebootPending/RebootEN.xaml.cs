using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using University_Menu.UserControls;

namespace University_Menu.Pages.RebootPending
{
    /// <summary>
    /// Interaction logic for RebootEN.xaml
    /// </summary>
    public partial class RebootEN : UserControl
    {
        public RebootEN()
        {
            InitializeComponent();

            rtbName.Foreground = MainWindow.themeFore;

            WTDReboot rn = new WTDReboot(MainWindow.Languages.English);
            restartNow.Children.Add(rn);

            ContactHelpdesk ch = new ContactHelpdesk(MainWindow.Languages.English,
                new TextRange(rtbSubTitle.ContentStart, rtbSubTitle.ContentEnd).Text);
            notifyContact.Children.Add(ch);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            checkExclude.IsChecked = MainWindow.rebootExclude;
        }

        private void checkExclude_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            MainWindow.rebootExclude = cb.IsChecked.Value;
            MainWindow.WriteXML();
        }
    }
}
