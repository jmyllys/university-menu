using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using University_Menu.Pages;
using static System.String;
using static System.Environment;

namespace University_Menu.UserControls
{
    /// <summary>
    /// Interaction logic for SupportPurchases.xaml
    /// </summary>
    public partial class SupportPurchases : UserControl
    {
        private static string requestTitle = Empty;
        private static SupportRequest.CategoryItem purchaseItem;

        public SupportPurchases(string title, SupportRequest.CategoryItem item)
        {
            InitializeComponent();
            
            requestTitle = title;
            int hash = UI.usedLanguage.GetHashCode();

            purchaseItem = item;
            bulletin.Content = MainWindow.GetTranslation(Properties.Resources.UISupportBulletin, hash);
            infoBulletin.Text = MainWindow.GetTranslation(Properties.Resources.UISupportPuBulletinInfo, hash);

            Hyperlink flamma = new Hyperlink(new Run(MainWindow.GetTranslation(Properties.Resources.UISupportPuFlammaURL, hash)))
            { NavigateUri = new Uri(MainWindow.GetTranslation(Properties.Resources.UISupportPuFlammaURL, hash)) };
            flamma.RequestNavigate += Flamma_RequestNavigate;
            flamma.MouseEnter += Hyperlink_MouseEnter;
            flamma.MouseLeave += Hyperlink_MouseLeave;
            infoBulletin.Inlines.Add(flamma);

            orders.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuOrders, hash);
            attachments.Content = MainWindow.GetTranslation(Properties.Resources.UISupportAttachments, hash);
            acceptor.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuAcceptor, hash);
            radioAcceptor1.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuMe, hash);
            radioAcceptor2.Content = MainWindow.GetTranslation(Properties.Resources.UISupportOther, hash);
            endUser.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuEndUser, hash);
            radioEndUser1.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuMe, hash);
            radioEndUser2.Content = MainWindow.GetTranslation(Properties.Resources.UISupportOther, hash);
            wbs.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuWBS, hash);
            purchaseType.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuType, hash);
            radioPurchase1.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuTypeNew, hash);
            radioPurchase2.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuTypeReplace, hash);
            device.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuDevice, hash);
            radioDevice1.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuDevice1, hash);
            radioDevice2.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuDevice2, hash);
            radioDevice3.Content = MainWindow.GetTranslation(Properties.Resources.UISupportPuDevice3, hash);
            radioDevice4.Content = MainWindow.GetTranslation(Properties.Resources.UISupportOther, hash);

            switch (UI.usedLanguage)
            {
                case MainWindow.Languages.Suomi:
                    textAcceptorFI.Visibility = Visibility.Visible;
                    textEndUserFI.Visibility = Visibility.Visible;
                    textWBSFI.Visibility = Visibility.Visible;
                    textPurchaseTypeFI.Visibility = Visibility.Visible;
                    break;
                case MainWindow.Languages.Svenska:
                    textAcceptorSV.Visibility = Visibility.Visible;
                    textEndUserSV.Visibility = Visibility.Visible;
                    textWBSSV.Visibility = Visibility.Visible;
                    textPurchaseTypeSV.Visibility = Visibility.Visible;
                    break;
                default:
                    textAcceptorEN.Visibility = Visibility.Visible;
                    textEndUserEN.Visibility = Visibility.Visible;
                    textWBSEN.Visibility = Visibility.Visible;
                    textPurchaseTypeEN.Visibility = Visibility.Visible;
                    break;
            }

            SupportAttachments sa = new SupportAttachments();
            srAttachments.Children.Clear();
            srAttachments.Children.Add(sa);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsNullOrWhiteSpace(textOrders.Text))
            {
                textOrders.Focus();
                Keyboard.Focus(textOrders);
            }
        }

        private void Hyperlink_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Hyperlink_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Flamma_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            MainWindow.Execute(e.Uri.AbsoluteUri, Empty);
        }

        private void textOrders_GotFocus(object sender, RoutedEventArgs e)
        {
            borderOrders.BorderBrush = Brushes.Transparent;
        }

        private void textOrders_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IsNullOrWhiteSpace(textOrders.Text)) { borderOrders.BorderBrush = Brushes.Red; }
            else { borderOrders.BorderBrush = Brushes.Transparent; }
        }

        private void radioAcceptor1_Checked(object sender, RoutedEventArgs e)
        {
            if (borderAcceptor.BorderBrush != Brushes.Transparent) { borderAcceptor.BorderBrush = Brushes.Transparent; }
        }

        private void textAcceptor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!radioAcceptor1.IsChecked.Value && !radioAcceptor2.IsChecked.Value) { radioAcceptor2.IsChecked = true; }
        }

        private void radioEndUser1_Checked(object sender, RoutedEventArgs e)
        {
            if (borderEndUser.BorderBrush != Brushes.Transparent) { borderEndUser.BorderBrush = Brushes.Transparent; }
        }

        private void textEndUser_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!radioEndUser1.IsChecked.Value && !radioEndUser2.IsChecked.Value) { radioEndUser2.IsChecked = true; }
        }

        private void textWBS_GotFocus(object sender, RoutedEventArgs e)
        {
            borderWBS.BorderBrush = Brushes.Transparent;
        }

        private void textWBS_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox obj;

            switch (UI.usedLanguage)
            {
                case MainWindow.Languages.Suomi:
                    obj = textWBSFI;
                    break;
                case MainWindow.Languages.Svenska:
                    obj = textWBSSV;
                    break;
                default:
                    obj = textWBSEN;
                    break;
            }

            if (IsNullOrWhiteSpace(obj.Text)) { borderWBS.BorderBrush = Brushes.Red; }
            else { borderWBS.BorderBrush = Brushes.Transparent; }
        }

        private void radioDevice1_Checked(object sender, RoutedEventArgs e)
        {
            if (borderDevice.BorderBrush != Brushes.Transparent) { borderDevice.BorderBrush = Brushes.Transparent; }
        }

        private void radioDevice2_Checked(object sender, RoutedEventArgs e)
        {
            if (borderDevice.BorderBrush != Brushes.Transparent) { borderDevice.BorderBrush = Brushes.Transparent; }
        }

        private void radioDevice3_Checked(object sender, RoutedEventArgs e)
        {
            if (borderDevice.BorderBrush != Brushes.Transparent) { borderDevice.BorderBrush = Brushes.Transparent; }
        }

        private void radioDevice4_Checked(object sender, RoutedEventArgs e)
        {
            if (borderDevice.BorderBrush != Brushes.Transparent) { borderDevice.BorderBrush = Brushes.Transparent; }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            string acceptedBy = "-";
            TextBox tbAcceptor, tbEndUser, tbWBS, tbPType;

            switch (UI.usedLanguage)
            {
                case MainWindow.Languages.Suomi:
                    tbAcceptor = textAcceptorFI;
                    tbEndUser = textEndUserFI;
                    tbWBS = textWBSFI;
                    tbPType = textPurchaseTypeFI;
                    break;
                case MainWindow.Languages.Svenska:
                    tbAcceptor = textAcceptorSV;
                    tbEndUser = textEndUserSV;
                    tbWBS = textWBSSV;
                    tbPType = textPurchaseTypeSV;
                    break;
                default:
                    tbAcceptor = textAcceptorEN;
                    tbEndUser = textEndUserEN;
                    tbWBS = textWBSEN;
                    tbPType = textPurchaseTypeEN;
                    break;
            }

            if (radioAcceptor1.IsChecked.Value) { acceptedBy = radioAcceptor1.Content.ToString(); }
            else if (radioAcceptor2.IsChecked.Value)
            { acceptedBy = $"{radioAcceptor2.Content.ToString()}: {(!IsNullOrWhiteSpace(tbAcceptor.Text) ? tbAcceptor.Text : "-")}"; }

            string user = "-";

            if (radioEndUser1.IsChecked.Value) { user = radioEndUser1.Content.ToString(); }
            else if (radioEndUser2.IsChecked.Value)
            { user= $"{radioEndUser2.Content.ToString()}: {(!IsNullOrWhiteSpace(tbEndUser.Text) ? tbEndUser.Text : "-")}"; }

            string newOrOld = Empty;

            if (radioPurchase1.IsChecked.Value) { newOrOld = radioPurchase1.Content.ToString(); }
            else if (radioPurchase2.IsChecked.Value)
            { newOrOld = $"{radioPurchase2.Content.ToString()}: {(!IsNullOrWhiteSpace(tbPType.Text) ? tbPType.Text : "-")}"; }

            string deviceSelection = Empty;

            if (radioDevice1.IsChecked.Value) { deviceSelection = radioDevice1.Content.ToString(); }
            else if (radioDevice2.IsChecked.Value) { deviceSelection = radioDevice2.Content.ToString(); }
            else if (radioDevice3.IsChecked.Value) { deviceSelection = radioDevice3.Content.ToString(); }
            else if (radioDevice4.IsChecked.Value) { deviceSelection = radioDevice4.Content.ToString(); }

            string ticketInfo = Empty;

            ticketInfo = Properties.Resources.EfecteMail1Start + MainWindow.sendSettings.Email + Properties.Resources.EfecteMail1End +
                Properties.Resources.EfecteMail2Start + MainWindow.sendSettings.Email + Properties.Resources.EfecteMail2End +
                ((!IsNullOrWhiteSpace(purchaseItem.RequestType)) ? Properties.Resources.EfecteReqtStart + purchaseItem.RequestType + Properties.Resources.EfecteReqtEnd : Empty) +
                ((!IsNullOrWhiteSpace(purchaseItem.Category1)) ? Properties.Resources.EfecteCat1Start + purchaseItem.Category1 + Properties.Resources.EfecteCat1End : Empty) +
                ((!IsNullOrWhiteSpace(purchaseItem.Category2)) ? Properties.Resources.EfecteCat2Start + purchaseItem.Category2 + Properties.Resources.EfecteCat2End : Empty) +
                ((!IsNullOrWhiteSpace(purchaseItem.Category3)) ? Properties.Resources.EfecteCat3Start + purchaseItem.Category3 + Properties.Resources.EfecteCat3End : Empty) +
                ((!IsNullOrWhiteSpace(purchaseItem.SupportGroup)) ? Properties.Resources.EfecteSgStart + purchaseItem.SupportGroup + Properties.Resources.EfecteSgEnd : Empty);

            string message =
                (textOrders.Text.Trim() + NewLine + NewLine +
                $"{device.Content}: {deviceSelection}" + NewLine +
                $"{acceptor.Content}: {acceptedBy}" + NewLine +
                $"{endUser.Content}: {user}" + NewLine + NewLine +
                $"{wbs.Content}: {tbWBS.Text}" + NewLine +
                $"{purchaseType.Content}: {newOrOld}").Trim();

            SupportRequest.request.Title = $"{requestTitle}: {deviceSelection}/{tbWBS.Text} [{MainWindow.GetTranslation(Properties.Resources.UMIconText)}]";

            SupportRequest.request.Message = message;
            SupportRequest.request.MessageSent = ticketInfo + NewLine + NewLine + Properties.Resources.EfecteDescStart + NewLine + message;

            if (!IsNullOrWhiteSpace(textOrders.Text) && (radioAcceptor1.IsChecked.Value || radioAcceptor2.IsChecked.Value)
                && (radioEndUser1.IsChecked.Value || radioEndUser2.IsChecked.Value) && !IsNullOrWhiteSpace(tbWBS.Text)
                && (radioDevice1.IsChecked.Value || radioDevice2.IsChecked.Value || radioDevice3.IsChecked.Value || radioDevice4.IsChecked.Value))
            { SupportRequest.request.Send = true; }
            else { SupportRequest.request.Send = false; }
        }
    }
}
