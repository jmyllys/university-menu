using System;
using System.Windows.Controls;
using static System.String;

namespace University_Menu.Pages
{
    /// <summary>
    /// Interaction logic for WarrantyTab.xaml
    /// </summary>
    public partial class WarrantyTab : UserControl
    {
        public static string expiredDate = Empty;

        public WarrantyTab()
        {
            InitializeComponent();

            MainWindow.Variable var = new MainWindow.Variable();
            expiredDate = var.Warranty;

            Uri page;
            switch (UI.usedLanguage)
            {
                case MainWindow.Languages.Suomi:
                    page = new Uri("/Pages/WarrantyExpired/WarrantyFI.xaml", UriKind.Relative);
                    break;
                case MainWindow.Languages.Svenska:
                    page = new Uri("/Pages/WarrantyExpired/WarrantySV.xaml", UriKind.Relative);
                    break;
                default:
                    page = new Uri("/Pages/WarrantyExpired/WarrantyEN.xaml", UriKind.Relative);
                    break;
            }
            startPage.SelectedSource = page;
        }
    }
}
