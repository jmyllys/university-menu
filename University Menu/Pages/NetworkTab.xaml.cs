using System;
using System.Windows.Controls;

namespace University_Menu.Pages
{
    /// <summary>
    /// Interaction logic for NetworkTab.xaml
    /// </summary>
    public partial class NetworkTab : UserControl
    {
        public NetworkTab()
        {
            InitializeComponent();

            Uri page;
            switch (UI.usedLanguage)
            {
                case MainWindow.Languages.Suomi:
                    page = new Uri("/Pages/NetworkSpace/NetworkFI.xaml", UriKind.Relative);
                    break;
                case MainWindow.Languages.Svenska:
                    page = new Uri("/Pages/NetworkSpace/NetworkSV.xaml", UriKind.Relative);
                    break;
                default:
                    page = new Uri("/Pages/NetworkSpace/NetworkEN.xaml", UriKind.Relative);
                    break;
            }
            startPage.SelectedSource = page;
        }
    }
}
