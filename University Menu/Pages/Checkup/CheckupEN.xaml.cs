using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using University_Menu.UserControls;

namespace University_Menu.Pages
{
    /// <summary>
    /// Interaction logic for CheckupEN.xaml
    /// </summary>
    public partial class CheckupEN : UserControl
    {
        public CheckupEN()
        {
            InitializeComponent();

            rtbLastUpdates.Text = rtbLastUpdates.Text + " " + CheckupTab.lastUpdates;
            rtbName.Foreground = MainWindow.themeFore;
            pathImage.Fill = MainWindow.themePath;
            rtbLastUpdates.Foreground = MainWindow.themePath;

            ContactHelpdesk ch = new ContactHelpdesk(MainWindow.Languages.English, 
                new TextRange(rtbSubTitle.ContentStart, rtbSubTitle.ContentEnd).Text);
            notifyContact.Children.Add(ch);

            WTDCheckup wtd = new WTDCheckup(MainWindow.Languages.English);
            notifyWhatToDo.Children.Add(wtd);

            checkExclude.IsChecked = MainWindow.checkupExclude;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            checkExclude.IsChecked = MainWindow.checkupExclude;
        }

        private void checkExclude_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            MainWindow.checkupExclude = cb.IsChecked.Value;
            MainWindow.WriteXML();
        }
    }
}
