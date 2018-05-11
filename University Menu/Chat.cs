using System;
using System.Drawing;
using System.Windows.Forms;
using static System.String;

namespace University_Menu
{
    public partial class Chat : Form
    {
        public Chat()
        {
            InitializeComponent();

            bool sizeChanged = false;

            if (MainWindow.chatWindowHeight >= 0)
            {
                Height = MainWindow.chatWindowHeight;
                sizeChanged = true;
            }
            if (MainWindow.chatWindowWidth >= 0)
            {
                Width = MainWindow.chatWindowWidth;
                sizeChanged = true;
            }
            if (sizeChanged)
            { Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2); }

            try
            { browserChat.Url = new Uri(MainWindow.GetTranslation(Properties.Resources.ChatUrl)); }
            catch
            {
                MainWindow.ExecuteError(MainWindow.GetTranslation(Properties.Resources.ErrorText), 
                    MainWindow.GetTranslation(Properties.Resources.DialogError) + MainWindow.GetTranslation(Properties.Resources.DialogSorry));
            }
        }

        private void Chat_Load(object sender, EventArgs e)
        {
            if (MainWindow.chatExternalBrowsing)
            {
                MainWindow.Execute(MainWindow.GetTranslation(Properties.Resources.ChatUrl), Empty);
                Close();
            }
        }
    }
}
