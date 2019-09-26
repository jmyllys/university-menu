using System;
using System.Windows.Controls;
using static System.String;

namespace University_Menu.Pages
{
    /// <summary>
    /// Interaction logic for OSUpgradeTab.xaml
    /// </summary>
    public partial class OSUpgradeTab : UserControl
    {
        public static string currentOS = Empty;

        public OSUpgradeTab()
        {
            InitializeComponent();

            MainWindow.Variable var = new MainWindow.Variable();
            currentOS = var.OperatingSystem;

            Uri page;
            switch (UI.usedLanguage)
            {
                case MainWindow.Languages.Suomi:
                    page = new Uri("/Pages/OSUpgrade/OSUpgradeFI.xaml", UriKind.Relative);
                    break;
                //case MainWindow.Languages.Svenska:
                //    page = new Uri("/Pages/OSUpgrade/OSUpgradeSV.xaml", UriKind.Relative);
                //    break;
                default:
                    page = new Uri("/Pages/OSUpgrade/OSUpgradeEN.xaml", UriKind.Relative);
                    break;
            }
            startPage.SelectedSource = page;
        }
    }
}
