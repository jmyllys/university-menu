using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using University_Menu.UserControls;

namespace University_Menu.Pages.WarrantyExpired
{
    /// <summary>
    /// Interaction logic for WarrantyEN.xaml
    /// </summary>
    public partial class WarrantyEN : UserControl
    {
        public WarrantyEN()
        {
            InitializeComponent();

            rtbWarrantyExpired.Text = rtbWarrantyExpired.Text + " " + WarrantyTab.expiredDate;
            rtbName.Foreground = MainWindow.themeFore;
            pathImage.Fill = MainWindow.themePath;
            rtbWarrantyExpired.Foreground = MainWindow.themePath;

            ContactHelpdesk ch = new ContactHelpdesk(MainWindow.Languages.English,
                new TextRange(rtbSubTitle.ContentStart, rtbSubTitle.ContentEnd).Text);

            WTDWarranty wtd = new WTDWarranty(MainWindow.Languages.English);
            notifyWhatToDo.Children.Add(wtd);

            checkExclude.IsChecked = MainWindow.warrantyExclude;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            checkExclude.IsChecked = MainWindow.warrantyExclude;
        }

        private void checkExclude_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            MainWindow.warrantyExclude = cb.IsChecked.Value;
            MainWindow.WriteXML();
        }
    }
}