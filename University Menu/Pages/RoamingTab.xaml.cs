using System;
using System.Windows.Controls;

namespace University_Menu.Pages
{
    /// <summary>
    /// Interaction logic for RoamingTab.xaml
    /// </summary>
    public partial class RoamingTab : UserControl
    {
        public RoamingTab()
        {
            InitializeComponent();

            Uri page;
            switch (UI.usedLanguage)
            {
                case MainWindow.Languages.Suomi:
                    page = new Uri("/Pages/RoamingProfile/RoamingFI.xaml", UriKind.Relative);
                    break;
                case MainWindow.Languages.Svenska:
                    page = new Uri("/Pages/RoamingProfile/RoamingSV.xaml", UriKind.Relative);
                    break;
                default:
                    page = new Uri("/Pages/RoamingProfile/RoamingEN.xaml", UriKind.Relative);
                    break;
            }
            startPage.SelectedSource = page;
        }
    }
}
