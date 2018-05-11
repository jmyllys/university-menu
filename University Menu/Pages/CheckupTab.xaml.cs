using System;
using System.Windows.Controls;
using static System.String;

namespace University_Menu.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class CheckupTab : UserControl
    {
        public static string lastUpdates = Empty;

        public CheckupTab()
        {
            InitializeComponent();

            MainWindow.Variable var = new MainWindow.Variable();
            lastUpdates = var.WinUpdateTime;

            Uri page;
            switch (UI.usedLanguage)
            {
                case MainWindow.Languages.Suomi:
                    page = new Uri("/Pages/Checkup/CheckupFI.xaml", UriKind.Relative);
                    break;
                case MainWindow.Languages.Svenska:
                    page = new Uri("/Pages/Checkup/CheckupSV.xaml", UriKind.Relative);
                    break;
                default:
                    page = new Uri("/Pages/Checkup/CheckupEN.xaml", UriKind.Relative);
                    break;
            }
            startPage.SelectedSource = page;
        }
    }
}
