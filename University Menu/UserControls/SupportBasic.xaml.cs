using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using University_Menu.Pages;
using static System.String;
using static System.Environment;
using System.Text.RegularExpressions;

namespace University_Menu.UserControls
{
    /// <summary>
    /// Interaction logic for SupportBasic.xaml
    /// </summary>
    public partial class SupportBasic : UserControl
    {
        private string requestTitle = Empty;
        private bool optinalTitle = false;
        private static SupportRequest.CategoryItem basicItem;

        public SupportBasic(SupportRequest.CategoryItem item, string defaultMessage = "")
        {
            InitializeComponent();

            basicItem = item;
            message.Content = MainWindow.GetTranslation(Properties.Resources.UISupportBasicMessage, UI.usedLanguage.GetHashCode());
            attachments.Content = MainWindow.GetTranslation(Properties.Resources.UISupportAttachments, UI.usedLanguage.GetHashCode());

            SupportAttachments sa = new SupportAttachments();
            srAttachments.Children.Clear();
            srAttachments.Children.Add(sa);

            if (IsNullOrWhiteSpace(item.Extra1))
            {
                extra1.Visibility = Visibility.Collapsed;
                textExtra1.Visibility = Visibility.Collapsed;
            }
            else { extra1.Content = item.Extra1; }

            if (IsNullOrWhiteSpace(item.Extra2))
            {
                extra2.Visibility = Visibility.Collapsed;
                textExtra2.Visibility = Visibility.Collapsed;
            }
            else { extra2.Content = item.Extra2; }

            if (!IsNullOrWhiteSpace(item.Title))
            {
                if (item.Title.IndexOf("%") >= 0)
                { item.Title = Regex.Replace(item.Title, "%category%", item.Caption, RegexOptions.IgnoreCase); }
                requestTitle = item.Title.Trim();
                optinalTitle = true;
            }
            else { requestTitle = MainWindow.GetTranslation(Properties.Resources.UISupportTitle, UI.usedLanguage.GetHashCode()) + ": " + item.Caption; }

            if (!IsNullOrWhiteSpace(defaultMessage))
            {
                textMessage.Text = defaultMessage;
                if (!SupportRequest.checkRequirements.IsBusy) { MainWindow.sendSettings.Message = Empty; }
            }

            if (!IsNullOrWhiteSpace(textMessage.Text)) { borderMessage.BorderBrush = Brushes.Transparent; }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            textMessage.Focus();
            Keyboard.Focus(textMessage);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            borderMessage.BorderBrush = Brushes.Transparent;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IsNullOrWhiteSpace(textMessage.Text)) { borderMessage.BorderBrush = Brushes.Red; }
            else { borderMessage.BorderBrush = Brushes.Transparent; }
        }

        private void extra1_Loaded(object sender, RoutedEventArgs e)
        {
            if (extra1.ActualWidth >= columnTitle.MaxWidth) { extra1.ToolTip = extra1.Content; }
        }

        private void extra2_Loaded(object sender, RoutedEventArgs e)
        {
            if (extra2.ActualWidth >= columnTitle.MaxWidth) { extra2.ToolTip = extra2.Content; }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            string message = textMessage.Text.Trim(), lines = NewLine + NewLine;

            if (!IsNullOrWhiteSpace(message)) { SupportRequest.request.Send = true; }
            else { SupportRequest.request.Send = false; }

            if (extra1.Visibility == Visibility.Visible)
            {
                message = message + lines + extra1.Content.ToString().Trim() + ": " + textExtra1.Text;
                lines = NewLine;
            }
            if (extra2.Visibility == Visibility.Visible)
            { message = message + lines + extra2.Content.ToString().Trim() + ": " + textExtra2.Text; }

            string ticketInfo = Empty;

            ticketInfo = Properties.Resources.EfecteMail1Start + MainWindow.sendSettings.Email + Properties.Resources.EfecteMail1End +
                Properties.Resources.EfecteMail2Start + MainWindow.sendSettings.Email + Properties.Resources.EfecteMail2End +
                ((!IsNullOrWhiteSpace(basicItem.RequestType)) ? Properties.Resources.EfecteReqtStart + basicItem.RequestType + Properties.Resources.EfecteReqtEnd : Empty) +
                ((!IsNullOrWhiteSpace(basicItem.Category1)) ? Properties.Resources.EfecteCat1Start + basicItem.Category1 + Properties.Resources.EfecteCat1End : Empty) +
                ((!IsNullOrWhiteSpace(basicItem.Category2)) ? Properties.Resources.EfecteCat2Start + basicItem.Category2 + Properties.Resources.EfecteCat2End : Empty) +
                ((!IsNullOrWhiteSpace(basicItem.Category3)) ? Properties.Resources.EfecteCat3Start + basicItem.Category3 + Properties.Resources.EfecteCat3End : Empty) +
                ((!IsNullOrWhiteSpace(basicItem.SupportGroup)) ? Properties.Resources.EfecteSgStart + basicItem.SupportGroup + Properties.Resources.EfecteSgEnd : Empty);

            SupportRequest.request.Message = message;
            SupportRequest.request.MessageSent = ticketInfo + NewLine + NewLine + Properties.Resources.EfecteDescStart + NewLine + message;

            string input = requestTitle;
            if (input.IndexOf("%") >= 0 && optinalTitle)
            {
                input = Regex.Replace(input, "%extrafield1%",
                    (!IsNullOrWhiteSpace(textExtra1.Text) ? textExtra1.Text.Trim() : Empty), RegexOptions.IgnoreCase);
                input = Regex.Replace(input, "%extrafield2%",
                    (!IsNullOrWhiteSpace(textExtra2.Text) ? textExtra2.Text.Trim() : Empty), RegexOptions.IgnoreCase);
            }
            SupportRequest.request.Title = input + $" [{MainWindow.GetTranslation(Properties.Resources.UMIconText)}]";
        }
    }
}
