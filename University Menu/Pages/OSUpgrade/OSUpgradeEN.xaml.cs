using System;
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
                    tbOSUpgradeText.Text = "This Computer is running Windows 7 operating system. Microsoft is discontinuing support of Windows 7 in January 2020. Computer you are using is so old that upgrading it to Windows 10 is not wise. If you need a replacement for this computer, follow instructions from link provided." + NewLine + NewLine + "Windows 7 Computers will be blocked from the network January 13th 2020. Recycle your you computer by following instructions.";
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
