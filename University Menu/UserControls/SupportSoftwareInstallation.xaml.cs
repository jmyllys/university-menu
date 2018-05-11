using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using static System.String;
using static System.Environment;
using University_Menu.Pages;
using System.IO;

namespace University_Menu.UserControls
{
    /// <summary>
    /// Interaction logic for SupportSoftwareInstallation.xaml
    /// </summary>
    public partial class SupportSoftwareInstallation : UserControl
    {
        private static string requestTitle;
        private static List<string> timeWindows = new List<string>();
        private static SupportRequest.CategoryItem softwareItem;

        public SupportSoftwareInstallation(string title, SupportRequest.CategoryItem item)
        {
            InitializeComponent();

            requestTitle = title;
            int hash = UI.usedLanguage.GetHashCode();

            softwareItem = item;
            bulletin.Content = MainWindow.GetTranslation(Properties.Resources.UISupportBulletin, hash);
            infoBulletin.Text = MainWindow.GetTranslation(Properties.Resources.UISupportSIBulletinInfo, hash);
            Hyperlink portal = new Hyperlink(new Run(MainWindow.GetTranslation(Properties.Resources.UISupportSIBulletinSoftwarePortal, hash)))
            {
                NavigateUri = new Uri(MainWindow.GetTranslation(Properties.Resources.UISupportSIPortalURL, hash))
            };
            portal.RequestNavigate += Portal_RequestNavigate;
            portal.MouseEnter += Hyperlink_MouseEnter;
            portal.MouseLeave += Hyperlink_MouseLeave;
            infoBulletin.Inlines.Add(portal);

            software.Content = MainWindow.GetTranslation(Properties.Resources.UISupportSISoftware, hash);
            license.Content = MainWindow.GetTranslation(Properties.Resources.UISupportSILicense, hash);
            appbar_information_circle_license.ToolTip = MainWindow.GetTranslation(Properties.Resources.UISupportSILicenseInfo, hash);
            radioLicense1.Content = MainWindow.GetTranslation(Properties.Resources.UISupportSILicense1, hash);
            radioLicense2.Content = MainWindow.GetTranslation(Properties.Resources.UISupportSILicense2, hash);
            radioLicense3.Content = MainWindow.GetTranslation(Properties.Resources.UISupportOther, hash);
            installation.Content = MainWindow.GetTranslation(Properties.Resources.UISupportSIInstallation, hash);
            appbar_information_circle_install.ToolTip = MainWindow.GetTranslation(Properties.Resources.UISupportSIInstallationInfo, hash);

            string option = MainWindow.GetTranslation(Properties.Resources.UISupportSIInstallationOption, hash), 
                time = MainWindow.GetTranslation(Properties.Resources.UISupportSIInstallationTime, hash);
            option1.Content = option + "1: ";
            option2.Content = option + "2: ";
            time1.Content = time;
            time2.Content = time;
            dateOption1.DisplayDateStart = DateTime.Now;
            dateOption2.DisplayDateStart = DateTime.Now;

            comments.Content = MainWindow.GetTranslation(Properties.Resources.UISupportSIComments, hash);
            infoInstruction.Text = MainWindow.GetTranslation(Properties.Resources.UISupportSIInstructions, hash);

            string localFolder = MainWindow.VariableConvert(MainWindow.notifyLocalFolder);
            if (Directory.Exists(localFolder))
            {
                Hyperlink hyData = new Hyperlink(new Run(localFolder)) { NavigateUri = new Uri(localFolder) };
                hyData.RequestNavigate += HyData_RequestNavigate;
                hyData.MouseEnter += Hyperlink_MouseEnter;
                hyData.MouseLeave += Hyperlink_MouseLeave;
                infoInstruction.Inlines.Add(hyData);
            }
            else { infoInstruction.Text = infoInstruction.Text.Replace(':', '.'); }

            timeWindows.Clear();
            timeWindows.Add(MainWindow.GetTranslation(Properties.Resources.UISupportSIInstallationAnyTime));
            switch (UI.usedLanguage)
            {
                case MainWindow.Languages.Suomi:
                    timeWindows.AddRange(new string[] { "Klo 9:00-10:30", "Klo 12:00-13:30", "Klo 14:00-15:30" });
                    break;
                case MainWindow.Languages.Svenska:
                    timeWindows.AddRange(new string[] { "Kl 9:00-10:30", "Kl 12:00-13:30", "Kl 14:00-15:30" });
                    break;
                default:
                    timeWindows.AddRange(new string[] { "9:00-10:30 AM", "12:00-13:30 PM", "14:00-15:30 PM" });
                    break;
            }

            comboTime1.ItemsSource = timeWindows;
            comboTime1.SelectedIndex = 0;
            comboTime2.ItemsSource = timeWindows;
            comboTime2.SelectedIndex = 0;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsNullOrWhiteSpace(textSoftware.Text))
            {
                textSoftware.Focus();
                Keyboard.Focus(textSoftware);
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

        private void Portal_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            MainWindow.Execute(e.Uri.AbsoluteUri, Empty);
        }

        private void HyData_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            MainWindow.Execute(e.Uri.AbsolutePath, Empty);
        }

        private void textSoftware_GotFocus(object sender, RoutedEventArgs e)
        {
            borderSoftware.BorderBrush = Brushes.Transparent;
        }

        private void textSoftware_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IsNullOrWhiteSpace(textSoftware.Text)) { borderSoftware.BorderBrush = Brushes.Red; }
            else { borderSoftware.BorderBrush = Brushes.Transparent; }
        }

        private void dateOption1_Loaded(object sender, RoutedEventArgs e)
        {
            Watermark(dateOption1);
        }

        private void dateOption1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Watermark(dateOption1);
        }

        private void dateOption2_Loaded(object sender, RoutedEventArgs e)
        {
            Watermark(dateOption2);
        }

        private void dateOption2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Watermark(dateOption2);
        }

        private static void Watermark(DatePicker target)
        {
            // http://stackoverflow.com/questions/1102042/wpf-toolkit-datepicker-change-default-value-show-calendar @ cp.net

            if (target.SelectedDate != null) return;

            FieldInfo fiTextBox = typeof(DatePicker).GetField("_textBox", BindingFlags.Instance | BindingFlags.NonPublic);

            if (fiTextBox != null)
            {
                DatePickerTextBox dateTextBox = (DatePickerTextBox)fiTextBox.GetValue(target);

                if (dateTextBox != null)
                {
                    PropertyInfo piWatermark = dateTextBox.GetType().GetProperty("Watermark", BindingFlags.Instance | BindingFlags.NonPublic);

                    if (piWatermark != null)
                    { piWatermark.SetValue(dateTextBox, MainWindow.GetTranslation(Properties.Resources.UISupportSIInstallationPickDate, UI.usedLanguage.GetHashCode()), null); }
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            string licenseType = "-";

            if (radioLicense1.IsChecked.Value) { licenseType = radioLicense1.Content.ToString(); }
            else if(radioLicense2.IsChecked.Value) { licenseType = radioLicense2.Content.ToString(); }
            else if(radioLicense3.IsChecked.Value) { licenseType = radioLicense3.Content.ToString(); }

            string installationTimes =
                $"- {option1.Content}" + (dateOption1.SelectedDate != null ? dateOption1.SelectedDate.Value.ToShortDateString() + $" @ {comboTime1.SelectedItem.ToString()}" : "-") + NewLine +
                $"- {option2.Content}" + (dateOption2.SelectedDate != null ? dateOption2.SelectedDate.Value.ToShortDateString() + $" @ {comboTime2.SelectedItem.ToString()}" : "-");

            string ticketInfo = Empty;

            ticketInfo = Properties.Resources.EfecteMail1Start + MainWindow.sendSettings.Email + Properties.Resources.EfecteMail1End +
                Properties.Resources.EfecteMail2Start + MainWindow.sendSettings.Email + Properties.Resources.EfecteMail2End +
                ((!IsNullOrWhiteSpace(softwareItem.RequestType)) ? Properties.Resources.EfecteReqtStart + softwareItem.RequestType + Properties.Resources.EfecteReqtEnd : Empty) +
                ((!IsNullOrWhiteSpace(softwareItem.Category1)) ? Properties.Resources.EfecteCat1Start + softwareItem.Category1 + Properties.Resources.EfecteCat1End : Empty) +
                ((!IsNullOrWhiteSpace(softwareItem.Category2)) ? Properties.Resources.EfecteCat2Start + softwareItem.Category2 + Properties.Resources.EfecteCat2End : Empty) +
                ((!IsNullOrWhiteSpace(softwareItem.Category3)) ? Properties.Resources.EfecteCat3Start + softwareItem.Category3 + Properties.Resources.EfecteCat3End : Empty) +
                ((!IsNullOrWhiteSpace(softwareItem.SupportGroup)) ? Properties.Resources.EfecteSgStart + softwareItem.SupportGroup + Properties.Resources.EfecteSgEnd : Empty);

            string message =
                ($"{software.Content}: {textSoftware.Text}" + NewLine +
                $"{license.Content}: {licenseType}" + NewLine + NewLine +
                $"{installation.Content}:" + NewLine + installationTimes + NewLine + NewLine +
                textComments.Text.Trim()).Trim();

            SupportRequest.request.Title = $"{requestTitle}: {textSoftware.Text} [{MainWindow.GetTranslation(Properties.Resources.UMIconText)}]";

            SupportRequest.request.Message = message;
            SupportRequest.request.MessageSent = ticketInfo + NewLine + NewLine + Properties.Resources.EfecteDescStart + NewLine + message;

            if (!IsNullOrWhiteSpace(textSoftware.Text)) { SupportRequest.request.Send = true; }
            else { SupportRequest.request.Send = false; }
        }
    }
}
