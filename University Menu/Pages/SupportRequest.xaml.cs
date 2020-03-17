using FirstFloor.ModernUI.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using University_Menu.UserControls;
using static System.String;
using static System.Environment;

namespace University_Menu.Pages
{
    /// <summary>
    /// Interaction logic for SupportRequest.xaml
    /// </summary>
    public partial class SupportRequest : UserControl
    {
        public static SupportSettings request = new SupportSettings();

        public static List<string> comboItemList = new List<string>();
        public static List<CategoryItem> comboCategories = new List<CategoryItem>();

        public static readonly BackgroundWorker checkRequirements = new BackgroundWorker();

        public SupportRequest()
        {
            InitializeComponent();

            int hash = UI.usedLanguage.GetHashCode();

            messageTitle.Text = MainWindow.GetTranslation(Properties.Resources.UISupportMessageTitle, hash).ToUpper();
            message.Text = MainWindow.GetTranslation(Properties.Resources.UISupportMessage, hash);
            subject.Content = MainWindow.GetTranslation(Properties.Resources.UISupportSubject, hash);
            info.Text = MainWindow.GetTranslation(Properties.Resources.UISupportInfo, hash);
            preview.Content = MainWindow.GetTranslation(Properties.Resources.UISupportButtonPreview, hash);
            clear.Content = MainWindow.GetTranslation(Properties.Resources.UISupportButtonClear, hash);
            wait.Text = MainWindow.GetTranslation(Properties.Resources.UISupportWait, hash);
            notIdentified.Text = MainWindow.GetTranslation(Properties.Resources.UISupportNotIdentified, hash);
            buttonAgain.Content = MainWindow.GetTranslation(Properties.Resources.UISupportAgain, hash);

            Hyperlink hd = new Hyperlink(new Run(MainWindow.GetTranslation(Properties.Resources.UISupportMessageUrl, hash)))
            { NavigateUri = new Uri(MainWindow.GetTranslation(Properties.Resources.UIHelpUrl, hash)) };
            hd.RequestNavigate += Hd_RequestNavigate;
            hd.MouseEnter += Hyperlink_MouseEnter;
            hd.MouseLeave += Hyperlink_MouseLeave;
            message.Inlines.Add(hd);

            checkRequirements.DoWork += CheckRequirements_DoWork;
            checkRequirements.RunWorkerCompleted += CheckRequirements_RunWorkerCompleted;

            comboCategories.Clear();
            comboItemList.Clear();

            comboCategories.Add(new CategoryItem { Caption = MainWindow.GetTranslation(Properties.Resources.UISupportCategory, hash) });
            comboItemList.Add(MainWindow.GetTranslation(Properties.Resources.UISupportCategory, hash));
            
            foreach (MainWindow.SupportCategory category in MainWindow.requestCategories)
            {
                if (IsNullOrWhiteSpace(category.CaptionEN)) { continue; }

                CategoryItem ci = new CategoryItem();

                ci.RequestType = category.RequestType;
                ci.Category1 = category.Category1;
                ci.Category2 = category.Category2;
                ci.Category3 = category.Category3;
                ci.SupportGroup = category.SupportGroup;

                switch (category.CaptionEN.ToLower())
                {
                    case "[softwareinstallation]":
                        ci.Caption = MainWindow.GetTranslation(Properties.Resources.UISupportCategorySI, UI.usedLanguage.GetHashCode());
                        ci.Module = CategoryModule.SoftwareInstallation;
                        break;
                    case "[purchases]":
                        ci.Caption = MainWindow.GetTranslation(Properties.Resources.UISupportCategoryPurchases, UI.usedLanguage.GetHashCode());
                        ci.Module = CategoryModule.Purchases;
                        break;
                    default:
                        switch (UI.usedLanguage)
                        {
                            case MainWindow.Languages.Suomi:
                                ci.Caption = (!IsNullOrWhiteSpace(category.CaptionFI) ? category.CaptionFI : category.CaptionEN);
                                ci.Extra1 = (!IsNullOrWhiteSpace(category.Entra1FI) ? category.Entra1FI : category.Entra1EN);
                                ci.Extra2 = (!IsNullOrWhiteSpace(category.Entra2FI) ? category.Entra2FI : category.Entra2EN);
                                ci.Title = (!IsNullOrWhiteSpace(category.TitleFI) ? category.TitleFI : category.TitleEN);
                                break;
                            case MainWindow.Languages.Svenska:
                                ci.Caption = (!IsNullOrWhiteSpace(category.CaptionSV) ? category.CaptionSV : category.CaptionEN);
                                ci.Extra1 = (!IsNullOrWhiteSpace(category.Entra1SV) ? category.Entra1SV : category.Entra1EN);
                                ci.Extra2 = (!IsNullOrWhiteSpace(category.Entra2SV) ? category.Entra2SV : category.Entra2EN);
                                ci.Title = (!IsNullOrWhiteSpace(category.TitleSV) ? category.TitleSV : category.TitleEN);
                                break;
                            default:
                                ci.Caption = category.CaptionEN;
                                ci.Extra1 = category.Entra1EN;
                                ci.Extra2 = category.Entra2EN;
                                ci.Title = category.TitleEN;
                                break;
                        }
                        ci.Module = CategoryModule.Other;
                        break;
                }

                comboCategories.Add(ci);
                comboItemList.Add(ci.Caption);
            }

            comboCategories.Add(new CategoryItem
            {
                Caption = MainWindow.GetTranslation(Properties.Resources.UISupportOther, hash),
                Module = CategoryModule.Other
            });
            comboItemList.Add(MainWindow.GetTranslation(Properties.Resources.UISupportOther, hash));

            comboSubject.ItemsSource = comboItemList;
            comboSubject.SelectedIndex = 0;

            gridCheck.Visibility = Visibility.Collapsed;
        }

        private void CheckRequirements_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry("LDAP://ad.helsinki.fi:636")
                { AuthenticationType = AuthenticationTypes.Secure | AuthenticationTypes.SecureSocketsLayer | AuthenticationTypes.ServerBind })
                using (DirectorySearcher search = new DirectorySearcher(entry)
                { Filter = "(&(objectClass=user)(objectCategory=person)(samaccountname=" + UserName + "))" })
                {
                    search.PropertiesToLoad.Add("displayname");
                    search.PropertiesToLoad.Add("mail");

                    SearchResult user = search.FindOne();
                    MainWindow.sendSettings.Name = user?.Properties["displayname"][0].ToString() ?? Empty;
                    MainWindow.sendSettings.Email = user?.Properties["mail"][0].ToString() ?? Empty;
                }
            }
            catch
            {
                MainWindow.sendSettings.Name = Empty;
                MainWindow.sendSettings.Email = Empty;
            }

            if (!IsNullOrWhiteSpace(MainWindow.sendSettings.Email) && !IsNullOrWhiteSpace(MainWindow.sendSettings.Name))
            { MainWindow.sendSettings.Verified = true; }
            else
            { MainWindow.sendSettings.Verified = false; }

            Application.Current.Dispatcher.Invoke(delegate
            {
                if (MainWindow.sendSettings.Verified)
                {
                    gridError.Visibility = Visibility.Collapsed;
                    subject.IsEnabled = true;
                    comboSubject.IsEnabled = true;
                }
                else { gridError.Visibility = Visibility.Visible; }

                gridCheck.Visibility = Visibility.Collapsed;
            });
        }

        private void CheckRequirements_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });

            if (comboSubject.IsEnabled)
            { if (!IsNullOrWhiteSpace(MainWindow.sendSettings.Message)) { comboSubject.SelectedIndex = comboSubject.Items.Count - 1; } }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            NotificationCheck();
        }

        private bool NotificationCheck()
        {
            if (MainWindow.sendSettings.Verified)
            {
                gridError.Visibility = Visibility.Collapsed;
                subject.IsEnabled = true;
                comboSubject.IsEnabled = true;

                if (!IsNullOrWhiteSpace(MainWindow.sendSettings.Message)) { comboSubject.SelectedIndex = comboSubject.Items.Count - 1; }
            }
            else { gridError.Visibility = Visibility.Visible; }

            return false;
        }

        private void ComboSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            panelContent.Children.Clear();

            if (cb.SelectedIndex <= 0) { panelBotton.Visibility = Visibility.Collapsed; }
            else
            {
                panelBotton.Visibility = Visibility.Visible;

                request = new SupportSettings();

                switch (comboCategories[cb.SelectedIndex].Module)
                {
                    case CategoryModule.SoftwareInstallation:
                        panelContent.Children.Add(new SupportSoftwareInstallation(cb.SelectedItem.ToString(), comboCategories[cb.SelectedIndex]));
                        break;
                    case CategoryModule.Purchases:
                        panelContent.Children.Add(new SupportPurchases(cb.SelectedItem.ToString(), comboCategories[cb.SelectedIndex]));
                        break;
                    default:
                        panelContent.Children.Add(new SupportBasic(comboCategories[cb.SelectedIndex], MainWindow.sendSettings.Message));
                        break;
                }
            }
        }

        private void preview_Click(object sender, RoutedEventArgs e)
        {
            IInputElement target = NavigationHelper.FindFrame("_top", this);
            NavigationCommands.GoToPage.Execute("/Pages/SupportPreview.xaml", target);
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            int current = comboSubject.SelectedIndex;

            comboSubject.SelectedIndex = 0;
            comboSubject.SelectedIndex = current;
        }

        private void again_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            gridCheck.Visibility = Visibility.Visible;
            gridError.Visibility = Visibility.Collapsed;

            checkRequirements.RunWorkerAsync();
        }

        public class SupportSettings
        {
            public string Title { get; set; }
            public string Message { get; set; }
            public string MessageSent { get; set; }
            public string ToAddress = MainWindow.requestToAddress;
            public List<string> Attachments = new List<string>();
            public bool Send = false;
        }

        public class CategoryItem
        {
            public string Caption { get; set; }
            public string Extra1 { get; set; }
            public string Extra2 { get; set; }
            public string Title { get; set; }
            public string RequestType { get; set; }
            public string Category1 { get; set; }
            public string Category2 { get; set; }
            public string Category3 { get; set; }
            public string SupportGroup { get; set; }
            public CategoryModule Module = CategoryModule.Other;
        }

        public enum CategoryModule
        {
            SoftwareInstallation,
            Purchases,
            Other
        }

        private void Hd_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            MainWindow.Execute(e.Uri.AbsoluteUri, Empty);
        }

        private void Hyperlink_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Hyperlink_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
