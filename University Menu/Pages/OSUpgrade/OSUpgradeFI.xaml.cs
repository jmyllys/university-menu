using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using University_Menu.UserControls;
using static System.Environment;

namespace University_Menu.Pages.OSUpgrade
{
    /// <summary>
    /// Interaction logic for OSMigrateFI.xaml
    /// </summary>
    public partial class OSUpgradeFI : UserControl
    {
        public OSUpgradeFI()
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
                    tbOSUpgradeText.Text = "Tämän tietokoneen käyttöjärjestelmä on Windows 7. Microsoft lopettaa tuen Windows 7 -käyttöjärjestelmälle tammikuussa 2020. Tämä laite on niin vanha, ettei sitä päivitetä Windows 10:een. Jos tarvitset korvaavan laitteen, hanki se normaalin hankintaprosessin mukaisesti. Ohjeet löydät Flamman tietokoneiden hankintaohjeesta (linkki tämän tekstin alla)." + NewLine + NewLine + "Windows 7 -käyttöjärjestelmää käyttävien tietokoneiden liikennöinti estetään yliopiston verkossa 13. tammikuuta 2020 alkaen. Kierrätä vanha koneesi ohjeiden mukaisesi (linkki Flamma-ohjeeseen alla).";
                    break;
                default:
                    return;
            }

            ContactHelpdesk ch = new ContactHelpdesk(MainWindow.Languages.Suomi,
                new TextRange(rtbSubTitle.ContentStart, rtbSubTitle.ContentEnd).Text);
            notifyContact.Children.Add(ch);

            WTDOSUpgrade wtd = new WTDOSUpgrade(MainWindow.Languages.Suomi);
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
