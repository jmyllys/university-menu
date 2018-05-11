namespace University_Menu
{
    partial class Chat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chat));
            this.browserChat = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // browserChat
            // 
            this.browserChat.AllowWebBrowserDrop = false;
            this.browserChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserChat.Location = new System.Drawing.Point(0, 0);
            this.browserChat.MinimumSize = new System.Drawing.Size(20, 20);
            this.browserChat.Name = "browserChat";
            this.browserChat.ScriptErrorsSuppressed = true;
            this.browserChat.ScrollBarsEnabled = false;
            this.browserChat.Size = new System.Drawing.Size(422, 567);
            this.browserChat.TabIndex = 0;
            this.browserChat.Url = new System.Uri("http://www.helsinki.fi/helpdesk-extra/univmenu_chat/univmenu_chat_index.html", System.UriKind.Absolute);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 567);
            this.Controls.Add(this.browserChat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "Chat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat";
            this.Load += new System.EventHandler(this.Chat_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser browserChat;
    }
}