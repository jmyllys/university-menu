using Microsoft.Win32;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using University_Menu.Pages;
using static System.String;

namespace University_Menu.UserControls
{
    /// <summary>
    /// Interaction logic for SupportAttachments.xaml
    /// </summary>
    public partial class SupportAttachments : UserControl
    {
        public SupportAttachments()
        {
            InitializeComponent();

            attachmentCount.Text = MainWindow.GetTranslation(Properties.Resources.UIAttachmentCount0, UI.usedLanguage.GetHashCode());
            attachmentCount.Foreground = MainWindow.themeFore;
            buttonAdd.Content = MainWindow.GetTranslation(Properties.Resources.UIAttachmentAdd, UI.usedLanguage.GetHashCode());
            buttonClear.Content = MainWindow.GetTranslation(Properties.Resources.UIAttachmentClear, UI.usedLanguage.GetHashCode());

            expanderAttachments.AllowDrop = true;
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog attachments = new OpenFileDialog
            {
                Multiselect = true,
                Title = buttonAdd.Content.ToString()
            };

            if (attachments.ShowDialog() == true) { CollectFiles(attachments.FileNames); }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            SupportRequest.request.Attachments.Clear();
            attachmentCount.Text = MainWindow.GetTranslation(Properties.Resources.UIAttachmentCount0, UI.usedLanguage.GetHashCode());
            attachmentCount.ToolTip = null;
            buttonClear.IsEnabled = false;
        }

        private void expanderAttachments_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) { CollectFiles((string[])e.Data.GetData(DataFormats.FileDrop)); }
        }

        private void CollectFiles(string[] attachments)
        {
            foreach (var attachment in attachments) { SupportRequest.request.Attachments.Add(attachment); }

            attachmentCount.Text = (SupportRequest.request.Attachments.Count > 1 ?
                    SupportRequest.request.Attachments.Count + MainWindow.GetTranslation(Properties.Resources.UIAttachmentCount2, UI.usedLanguage.GetHashCode()) :
                    SupportRequest.request.Attachments.Count + MainWindow.GetTranslation(Properties.Resources.UIAttachmentCount1, UI.usedLanguage.GetHashCode()));

            StringBuilder sbAttachments = new StringBuilder();

            foreach (string target in SupportRequest.request.Attachments)
            {
                if (!IsNullOrWhiteSpace(sbAttachments.ToString())) { sbAttachments.Append(Environment.NewLine); }
                sbAttachments.Append(target);
            }

            attachmentCount.ToolTip = sbAttachments.ToString();
            buttonClear.IsEnabled = true;
        }
    }
}
