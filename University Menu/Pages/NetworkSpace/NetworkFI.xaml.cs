using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using University_Menu.UserControls;

namespace University_Menu.Pages.NetworkSpace
{
    /// <summary>
    /// Interaction logic for NetworkFI.xaml
    /// </summary>
    public partial class NetworkFI : UserControl
    {
        public NetworkFI()
        {
            InitializeComponent();

            rtbName.Foreground = MainWindow.themeFore;

            NetworkDiskSpace nds = new NetworkDiskSpace(MainWindow.Languages.Suomi);
            diskSpace.Children.Add(nds);

            ContactHelpdesk ch = new ContactHelpdesk(MainWindow.Languages.Suomi,
                new TextRange(rtbSubTitle.ContentStart, rtbSubTitle.ContentEnd).Text);
            notifyContact.Children.Add(ch);

            WTDNetwork wtd = new WTDNetwork(MainWindow.Languages.Suomi);
            notifyWhatToDo.Children.Add(wtd);

            checkExclude.IsChecked = MainWindow.networkExclude;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            checkExclude.IsChecked = MainWindow.networkExclude;
        }

        private void checkExclude_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            MainWindow.networkExclude = cb.IsChecked.Value;
            MainWindow.WriteXML();
        }
    }
}
