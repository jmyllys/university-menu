using System;
using System.Windows.Controls;

namespace University_Menu.Pages
{
    /// <summary>
    /// Interaction logic for RebootTab.xaml
    /// </summary>
    public partial class RebootTab : UserControl
    {
        public RebootTab()
        {
            InitializeComponent();

            Uri page;
            switch (UI.usedLanguage)
            {
                case MainWindow.Languages.Suomi:
                    page = new Uri("/Pages/RebootPending/RebootFI.xaml", UriKind.Relative);
                    break;
                case MainWindow.Languages.Svenska:
                    page = new Uri("/Pages/RebootPending/RebootSV.xaml", UriKind.Relative);
                    break;
                default:
                    page = new Uri("/Pages/RebootPending/RebootEN.xaml", UriKind.Relative);
                    break;
            }
            startPage.SelectedSource = page;
        }
    }
}
