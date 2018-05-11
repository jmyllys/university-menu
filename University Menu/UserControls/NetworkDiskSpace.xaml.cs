using System;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.String;

namespace University_Menu.UserControls
{
    /// <summary>
    /// Interaction logic for NotifyNetworkDiskSpace.xaml
    /// </summary>
    public partial class NetworkDiskSpace : UserControl
    {
        public int languageHashCode = 0;

        public NetworkDiskSpace(MainWindow.Languages language)
        {
            InitializeComponent();

            languageHashCode = language.GetHashCode();

            diskSpace.Text = MainWindow.GetTranslation(Properties.Resources.UINetworkDiskSpaceCalc, languageHashCode);
            diskSpace.Foreground = MainWindow.themePath;
            pathPie.Fill = MainWindow.themePath;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork; ;
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            using (UserPrincipal user = UserPrincipal.Current)
            {
                MainWindow.Variable var = new MainWindow.Variable();

                try
                {
                    double free = var.HomeDriveFreeSpace, total = var.HomeDriveTotalSize;
                    string drive = user.HomeDrive;

                    if (free >= 0 && total > 0 && !IsNullOrWhiteSpace(drive))
                    {
                        double percent = Math.Round(free / total * 100, 1);
                        free = free / 1024 / 1024;

                        Application.Current.Dispatcher.Invoke(delegate
                        {
                            pathPie.Fill = MainWindow.themePath;
                            diskSpace.Foreground = MainWindow.themePath;
                            diskSpace.Text = MainWindow.GetTranslation(Properties.Resources.UINetworkDiskSpaceInfo, languageHashCode)
                                .Replace("xx", free.ToString("#.##")).Replace("yy", percent.ToString()).Replace("zz", drive);
                        });
                    }
                    else { throw new Exception(); }
                }
                catch
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        diskSpace.Text = MainWindow.GetTranslation(Properties.Resources.UINetworkDiskSpaceInfoNA, languageHashCode);
                        pathPie.Fill = Brushes.Gray;
                        diskSpace.Foreground = Brushes.Gray;
                    });
                }
            }
        }
    }
}
