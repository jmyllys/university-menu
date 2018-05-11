using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.String;

namespace University_Menu.UserControls
{
    /// <summary>
    /// Interaction logic for WTDWarranty.xaml
    /// </summary>
    public partial class WTDWarranty : UserControl
    {
        public WTDWarranty(MainWindow.Languages language)
        {
            InitializeComponent();

            wtdTitle.Text = MainWindow.GetTranslation(Properties.Resources.UIWarrantyWTDTitle, language.GetHashCode());
            wtdTitle.Foreground = MainWindow.themeFore;

            string web = MainWindow.GetTranslation(Properties.Resources.ModuleCompWarrantyUrl, language.GetHashCode());

            if (!IsNullOrWhiteSpace(web))
            {
                borderWeb.Visibility = Visibility.Visible;
                wtdWeb.Text = MainWindow.GetTranslation(Properties.Resources.UICheckupWTDWeb, language.GetHashCode());
                wtdWeb.Foreground = MainWindow.themeFore;
                pathWeb.Fill = MainWindow.themeFore;
                borderWeb.Tag = web;
            }
            else { borderWeb.Visibility = Visibility.Collapsed; }
        }

        private void borderWeb_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            borderWeb.Background = UI.enterColor;
        }

        private void borderWeb_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            borderWeb.Background = Brushes.Transparent;
        }

        private void borderWeb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsNullOrWhiteSpace(borderWeb.Tag.ToString()))
            { MainWindow.Execute(borderWeb.Tag.ToString(), Empty); }
        }
    }
}