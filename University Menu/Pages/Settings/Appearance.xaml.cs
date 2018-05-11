using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace University_Menu.Pages.Settings
{
    /// <summary>
    /// Interaction logic for Appearance.xaml
    /// </summary>
    public partial class Appearance : UserControl
    {
        BitmapImage en = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuEN.ico", UriKind.Absolute));
        BitmapImage fi = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuFI.ico", UriKind.Absolute));
        BitmapImage sv = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuSV.ico", UriKind.Absolute));
        bool restartOnly = false;
        bool loading = false;

        public Appearance()
        {
            InitializeComponent();

            int hash = UI.usedLanguage.GetHashCode();

            appearance.Text = MainWindow.GetTranslation(Properties.Resources.UISettingsAppearance, hash).ToUpper();
            theme.Text = MainWindow.GetTranslation(Properties.Resources.UISettingsTheme, hash);
            fontSize.Text = MainWindow.GetTranslation(Properties.Resources.UISettingsFontSize, hash);
            language.Text = MainWindow.GetTranslation(Properties.Resources.UISettingsLanguage, hash);

            comboLanguage.ItemsSource = new string[] { "english", "suomi", "svenska" };

            if (MainWindow.forceLanguage < 0) { rowLanguage.Height = GridLength.Auto; }
            else { rowLanguage.Height = new GridLength(0); }

            // create and assign the appearance view model
            this.DataContext = new AppearanceViewModel();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loading = true;
            comboLanguage.SelectedIndex = MainWindow.uiLanguage.GetHashCode();
            loading = false;
        }

        private void comboLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            buttonLanguage.Content = MainWindow.GetTranslation(Properties.Resources.UISettingsApply, cb.SelectedIndex);

            if (cb.SelectedIndex == MainWindow.uiLanguage.GetHashCode() && MainWindow.uiLanguage != UI.usedLanguage)
            {
                toLanguage.Visibility = Visibility.Collapsed;
                imageNew.Visibility = Visibility.Collapsed;
                buttonLanguage.Visibility = Visibility.Visible;

                restartOnly = true;
            }
            else if (cb.SelectedIndex == UI.usedLanguage.GetHashCode() && MainWindow.uiLanguage == UI.usedLanguage)
            {
                toLanguage.Visibility = Visibility.Collapsed;
                imageNew.Visibility = Visibility.Collapsed;
                buttonLanguage.Visibility = Visibility.Hidden;

                restartOnly = false;
            }
            else
            {
                toLanguage.Visibility = Visibility.Visible;
                imageNew.Visibility = Visibility.Visible;
                buttonLanguage.Visibility = Visibility.Visible;

                restartOnly = false;
            }

            switch (MainWindow.uiLanguage)
            {
                case MainWindow.Languages.Suomi:
                    imageCurrent.Source = fi;
                    break;
                case MainWindow.Languages.Svenska:
                    imageCurrent.Source = sv;
                    break;
                default:
                    imageCurrent.Source = en;
                    break;
            }
            switch (cb.SelectedIndex)
            {
                case 1:
                    imageNew.Source = fi;
                    break;
                case 2:
                    imageNew.Source = sv;
                    break;
                default:
                    imageNew.Source = en;
                    break;
            }

            if (!loading) { ChangeUILanguage(); }
        }

        private void buttonLanguage_Click(object sender, RoutedEventArgs e)
        {
            ChangeUILanguage();
        }

        private void ChangeUILanguage()
        {
            if (MainWindow.forceLanguage < 0)
            {
                switch (comboLanguage.SelectedIndex)
                {
                    case 1:
                        MainWindow.uiLanguage = MainWindow.Languages.Suomi;
                        break;
                    case 2:
                        MainWindow.uiLanguage = MainWindow.Languages.Svenska;
                        break;
                    default:
                        MainWindow.uiLanguage = MainWindow.Languages.English;
                        break;
                }

                if (!restartOnly) { MainWindow.LanguageMenuItemCheck(); }
            }

            MainWindow.OpenWindow(MainWindow.Popup.Settings, true);
        }
    }
}
