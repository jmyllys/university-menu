using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using University_Menu.UserControls;

namespace University_Menu.Pages.OSUpgrade
{
    /// <summary>
    /// Interaction logic for OSMigrateSV.xaml
    /// </summary>
    public partial class OSUpgradeSV : UserControl
    {
        public OSUpgradeSV()
        {
            InitializeComponent();

            rtbOS.Text = rtbOS.Text + " " + OSUpgradeTab.currentOS;
            rtbName.Foreground = MainWindow.themeFore;
            pathImage.Fill = MainWindow.themePath;
            rtbOS.Foreground = MainWindow.themePath;

            ContactHelpdesk ch = new ContactHelpdesk(MainWindow.Languages.Svenska,
                new TextRange(rtbSubTitle.ContentStart, rtbSubTitle.ContentEnd).Text);
            notifyContact.Children.Add(ch);

            WTDOSUpgrade wtd = new WTDOSUpgrade(MainWindow.Languages.Svenska);
            notifyWhatToDo.Children.Add(wtd);

            //checkExclude.IsChecked = MainWindow.osupgradeExclude;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //checkExclude.IsChecked = MainWindow.osupgradeExclude;
        }

        private void checkExclude_Click(object sender, RoutedEventArgs e)
        {
            //CheckBox cb = (CheckBox)sender;

            //MainWindow.osupgradeExclude = cb.IsChecked.Value;
            //MainWindow.WriteXML();
        }
    }
}
