using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace University_Menu.Pages.Settings
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();

            string[] version = Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');
            Assembly asm = Assembly.GetExecutingAssembly();

            textVersion.Text = textVersion.Text.Replace("XX", version[0]).Replace("YY", version[1]).Replace("ZZ", version[2]).Replace("KKKK", version[3]);
            textCopyright.Text = ((AssemblyCopyrightAttribute)asm.GetCustomAttribute(typeof(AssemblyCopyrightAttribute))).Copyright;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            { Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)); }
            catch { }
        }
    }
}
