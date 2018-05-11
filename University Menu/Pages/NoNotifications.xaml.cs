using System.Windows.Controls;

namespace University_Menu.Pages
{
    /// <summary>
    /// Interaction logic for NoNotifications.xaml
    /// </summary>
    public partial class NoNotifications : UserControl
    {
        public NoNotifications()
        {
            InitializeComponent();

            noNotify.Text = MainWindow.GetTranslation(Properties.Resources.UINoNotifyText, UI.usedLanguage.GetHashCode());
        }
    }
}
