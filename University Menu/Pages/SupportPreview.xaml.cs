using FirstFloor.ModernUI.Windows.Navigation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.String;
using static System.Environment;
using System.Net.Mail;
using System.IO;
using FirstFloor.ModernUI.Windows.Controls;

namespace University_Menu.Pages
{
    /// <summary>
    /// Interaction logic for SupportPreview.xaml
    /// </summary>
    public partial class SupportPreview : UserControl
    {
        private SmtpClient smtp = new SmtpClient
        {
            EnableSsl = true,
            UseDefaultCredentials = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Host = MainWindow.requestHostAddress,
            Port = MainWindow.requestHostPort
        };
        private bool sendingMessage = false;
        private string signature;

        public SupportPreview()
        {
            InitializeComponent();

            int hash = UI.usedLanguage.GetHashCode();

            title.Text = MainWindow.GetTranslation(Properties.Resources.UISupportPreview, hash).ToUpper();
            message.Text = MainWindow.GetTranslation(Properties.Resources.UIPreviewMessage, hash);
            send.Content = MainWindow.GetTranslation(Properties.Resources.UISupportButtonSend, hash);
            edit.Content = MainWindow.GetTranslation(Properties.Resources.UISupportButtonEdit, hash);
            hazard.Text = MainWindow.GetTranslation(Properties.Resources.UIPreviewCanNotSend, hash);
            cancel.Content = MainWindow.GetTranslation(Properties.Resources.UIPreviewCancel, hash);

            messageTitle.Foreground = MainWindow.themeFore;
            messageText.Foreground = MainWindow.themeFore;
            messageFooter.Foreground = MainWindow.themeFore;

            smtp.SendCompleted += Smtp_SendCompleted;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (SupportRequest.request.Send && MainWindow.sendSettings.Verified)
            {
                send.IsEnabled = true;
                gridCanNotSend.Visibility = Visibility.Collapsed;
            }
            else
            {
                send.IsEnabled = false;
                gridCanNotSend.Visibility = Visibility.Visible;
            }

            string attachments = Empty;
            if (SupportRequest.request.Attachments.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (string attachment in SupportRequest.request.Attachments)
                { sb.AppendLine(MainWindow.GetTranslation(Properties.Resources.UISupportAttachment, UI.usedLanguage.GetHashCode()) + ": " + attachment); }

                attachments = sb.ToString().Trim();
            }

            signature = NewLine + NewLine +
                Properties.Resources.UISupportAutoStart + NewLine + MainWindow.sendSettings.Signature + NewLine + Properties.Resources.UISupportAutoEnd;

            messageTitle.Text = SupportRequest.request.Title;
            messageText.Text = SupportRequest.request.Message + signature;

            if (IsNullOrWhiteSpace(attachments))
            {
                messageFooter.Visibility = Visibility.Collapsed;
                messageText.Height = 275;
            }
            else
            {
                messageFooter.Visibility = Visibility.Visible;
                messageText.Height = 230;
                messageFooter.Text = attachments;
            }
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                send.IsEnabled = false;
                edit.Visibility = Visibility.Collapsed;
                edit.IsCancel = false;
                cancel.Visibility = Visibility.Visible;
                cancel.IsCancel = true;

                MailMessage message = new MailMessage
                {
                    SubjectEncoding = Encoding.UTF8,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = false,
                    //From = new MailAddress(MainWindow.sendSettings.Email, MainWindow.sendSettings.Name, Encoding.UTF8),
                    From = new MailAddress("noreply@helsinki.fi", "University Menu", Encoding.UTF8),
                    Subject = messageTitle.Text,
                    Body = SupportRequest.request.MessageSent + signature + NewLine + Properties.Resources.EfecteDescEnd,
                };

                //if (checkSendCopy.IsChecked == true)
                //{ message.CC.Add(new MailAddress(MainWindow.sendSettings.Email, MainWindow.sendSettings.Name, Encoding.UTF8)); }

                message.To.Add(SupportRequest.request.ToAddress);
                
                if (SupportRequest.request.Attachments.Count > 0)
                {
                    bool errors = false;

                    foreach (string attachment in SupportRequest.request.Attachments)
                    {
                        if (File.Exists(attachment)) { message.Attachments.Add(new Attachment(attachment)); }
                        else { errors = true; }
                    }

                    if (errors)
                    {
                        try
                        {
                            ModernDialog md = new ModernDialog
                            {
                                Title = MainWindow.GetTranslation(Properties.Resources.UISupportTitle, UI.usedLanguage.GetHashCode()),
                                Content = MainWindow.GetTranslation(Properties.Resources.UIPreviewAttachmentError, UI.usedLanguage.GetHashCode()),
                                ShowInTaskbar = true,
                                ResizeMode = ResizeMode.CanMinimize,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                                ShowActivated = true,
                                Owner = null
                            };

                            md.Buttons = new Button[] { md.YesButton, md.NoButton };
                            md.ShowDialog();

                            if (md.MessageBoxResult == MessageBoxResult.No)
                            {
                                message.Dispose();
                                return;
                            }
                        }
                        catch
                        {
                            MessageBoxResult result = MessageBox.Show(MainWindow.GetTranslation(Properties.Resources.UIPreviewAttachmentError, UI.usedLanguage.GetHashCode()),
                                MainWindow.GetTranslation(Properties.Resources.UISupportTitle, UI.usedLanguage.GetHashCode()),
                                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                            if (result == MessageBoxResult.No)
                            {
                                message.Dispose();
                                return;
                            }
                        }
                    }
                }

                sendingMessage = true;
                Mouse.OverrideCursor = Cursors.AppStarting;
                smtp.SendAsync(message, "A Support Request by University Menu");
            }
            catch
            {
                MainWindow.ExecuteError(MainWindow.GetTranslation(Properties.Resources.UISupportTitle, UI.usedLanguage.GetHashCode()),
                  MainWindow.GetTranslation(Properties.Resources.UIPreviewSendError, UI.usedLanguage.GetHashCode()) +
                  MainWindow.GetTranslation(Properties.Resources.DialogSorry, UI.usedLanguage.GetHashCode()));
            }
        }

        private void Smtp_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                try
                {
                    bool back = false;

                    if (e.Cancelled) { }
                    else if (e.Error != null)
                    {
                        MainWindow.ExecuteError(MainWindow.GetTranslation(Properties.Resources.UISupportTitle, UI.usedLanguage.GetHashCode()),
                          MainWindow.GetTranslation(Properties.Resources.UIPreviewSendError, UI.usedLanguage.GetHashCode()) +
                          MainWindow.GetTranslation(Properties.Resources.DialogSorry, UI.usedLanguage.GetHashCode()));
                    }
                    else
                    {
                        MainWindow.ExecuteError(MainWindow.GetTranslation(Properties.Resources.UISupportTitle, UI.usedLanguage.GetHashCode()),
                            MainWindow.GetTranslation(Properties.Resources.UIPreviewSendSuccess, UI.usedLanguage.GetHashCode()), MessageBoxImage.Information);
                        back = true;
                    }

                    sendingMessage = false;
                    Mouse.OverrideCursor = Cursors.Arrow;

                    cancel.Visibility = Visibility.Collapsed;
                    cancel.IsCancel = false;
                    edit.Visibility = Visibility.Visible;
                    edit.IsCancel = true;
                    send.IsEnabled = true;

                    if (back) { ReturnSupport(); }
                }
                catch
                {
                    MainWindow.ExecuteError(MainWindow.GetTranslation(Properties.Resources.UISupportTitle, UI.usedLanguage.GetHashCode()),
                      MainWindow.GetTranslation(Properties.Resources.UIPreviewSendError, UI.usedLanguage.GetHashCode()) +
                      MainWindow.GetTranslation(Properties.Resources.DialogSorry, UI.usedLanguage.GetHashCode()));
                }
            });
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            ReturnSupport();
        }

        private void ReturnSupport()
        {
            IInputElement target = NavigationHelper.FindFrame("_top", this);
            NavigationCommands.GoToPage.Execute("/Pages/SupportRequest.xaml", target);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            smtp.SendAsyncCancel();
        }
    }
}
