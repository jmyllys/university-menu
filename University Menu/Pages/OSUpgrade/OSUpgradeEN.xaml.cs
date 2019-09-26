using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using University_Menu.UserControls;
using static System.Environment;

namespace University_Menu.Pages.OSUpgrade
{
    /// <summary>
    /// Interaction logic for OSMigrateEN.xaml
    /// </summary>
    public partial class OSUpgradeEN : UserControl
    {
        public OSUpgradeEN()
        {
            InitializeComponent();

            rtbOS.Text = rtbOS.Text + " " + OSUpgradeTab.currentOS;
            rtbName.Foreground = MainWindow.themeFore;
            pathImage.Fill = MainWindow.themePath;
            rtbOS.Foreground = MainWindow.themePath;

            switch (MainWindow.OSGroup)
            {
                case MainWindow.OSUpgradeGroups.InPlace:
                    tbOSUpgradeText.Text = "This Computer is running Windows 7 operating system. Please read and follow the instructions from the link provided to upgrade the operating system to Windows 10.";
                    break;
                case MainWindow.OSUpgradeGroups.Poistuvat:
                    tbOSUpgradeText.Text = "This computer is running the Windows 7 operating system. Microsoft will discontinue support of Windows 7 in January 2020. The computer you are using is so old that it will not be upgraded to Windows 10. If you need a replacement for this computer, follow the Flamma instructions for acquiring a computer (link below)." + NewLine + NewLine + "Windows 7 computers will be blocked from the network on January 13th 2020. Recycle your computer by following instructions (link below).";
                    break;
                default:
                    return;
            }

            ContactHelpdesk ch = new ContactHelpdesk(MainWindow.Languages.English,
                new TextRange(rtbSubTitle.ContentStart, rtbSubTitle.ContentEnd).Text);
            notifyContact.Children.Add(ch);

            WTDOSUpgrade wtd = new WTDOSUpgrade(MainWindow.Languages.English);
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
