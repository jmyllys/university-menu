using FirstFloor.ModernUI.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Net.NetworkInformation;
using static System.String;
using FirstFloor.ModernUI.Presentation;
using System.Net;

namespace University_Menu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                OnlyOneProcess();
                SetIconSize();
                SetFileWatchEvents();

                ReadXML(xmlFileUser, Properties.Resources.NodeUserSettings, XML.User);
                AppearanceSettings();

                BuildMenu(false);

                Worker.DoWork += Worker_DoWork;
                SetBackgroundTimer();
                NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(AddressChangedCallback);

                FinishIcon();
                FinalizeSettings();
            }
            catch
            { Application.Current.Shutdown(); }
        }

        private void OnlyOneProcess()
        {
            // Only one process for each logged on user is allowed

            Process[] prosessList = Process.GetProcesses();

            int i = 0;
            foreach (Process process in prosessList)
            {
                if (process.SessionId == Process.GetCurrentProcess().SessionId &&
                    process.ProcessName == Process.GetCurrentProcess().ProcessName)
                { i++; }
            }

            if (i > 1) { Environment.Exit(1); }
        }

        private void SetIconSize()
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiX = (int)dpiXProperty.GetValue(null, null);

            if (dpiX > 144) { iconFixedSize = 48; }
            else if (dpiX > 120) { iconFixedSize = 32; }
            else if (dpiX > 96) { iconFixedSize = 24; }
            else { iconFixedSize = 16; }
        }

        private void SetFileWatchEvents()
        {
            Watcher.Changed += Watcher_Changed;
            Watcher.Created += Watcher_Changed;
            Watcher.Deleted += Watcher_Changed;
            Watcher.Renamed += Watcher_Changed;

            Directory.CreateDirectory(xmlFolderUser);
            WatcherUser.Path = (xmlFolderUser);

            WatcherUser.Changed += Watcher_Changed;
            WatcherUser.Created += Watcher_Changed;
            WatcherUser.Deleted += Watcher_Changed;
            WatcherUser.Renamed += Watcher_Changed;

            Watcher.EnableRaisingEvents = true;
            WatcherUser.EnableRaisingEvents = true;
        }

        private void SetBackgroundTimer()
        {
            if (startTimer > 0)
            {
                BackgroundTimer.Tick += BackgroundTimer_Tick;
                BackgroundTimer.Interval = TimeSpan.FromMinutes(startTimer);
                BackgroundTimer.Start();

                ClipboardIcon.Tick += ClipboardIcon_Tick;
            }
        }

        private void BackgroundTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (timerFirstLoop)
                {
                    timerFirstLoop = false;

                    if (allowWelcome) { WelcomeMessage(); }
                }

                BuildModules(true);
                CheckNotifications(true);
            }
            catch { }

            int idleTime;
            Random random = new Random();

            try { idleTime = random.Next(idleTimerMin, idleTimerMax); }
            catch { idleTime = random.Next(defaultIdleTimeMin, defaultIdleTimeMax); }

            if (idleTime > 0) { BackgroundTimer.Interval = TimeSpan.FromMinutes(idleTime); }
            else { BackgroundTimer.Stop(); }
        }

        public void FinishIcon()
        {
            // Set the final settings for the NotifyIcon
            UMicon.ContextMenu = UMmenu;
            UMicon.ToolTipText = GetTranslation(Properties.Resources.UMIconText);
            UMicon.Visibility = Visibility.Visible;
            UMicon.TrayBalloonTipClicked += UMicon_TrayBalloonTipClicked;
            UMicon.TrayLeftMouseDown += UMicon_TrayMouseDown;
            UMicon.TrayRightMouseDown += UMicon_TrayMouseDown;

            IconType startIcon = IconType.Normal;
            ChangeNotificationIcon(IconType.Normal, Empty, ref startIcon, true, true);
        }

        private void AppearanceSettings()
        {
            AppearanceManager.Current.AccentColor = themeColor;
            AppearanceManager.Current.FontSize = themeFont;
            AppearanceManager.Current.ThemeSource = theme;

            if (AppearanceManager.Current.ThemeSource == AppearanceManager.DarkThemeSource)
            {
                themeFore = System.Windows.Media.Brushes.LightGray;
                themeForeOpposite = System.Windows.Media.Brushes.Black;
                themePath = System.Windows.Media.Brushes.Red;
                darkTheme = true;
            }
            else
            {
                themeFore = System.Windows.Media.Brushes.Black;
                themeForeOpposite = System.Windows.Media.Brushes.White;
                themePath = System.Windows.Media.Brushes.DarkRed;
                darkTheme = false;
            }
        }

        private void FinalizeSettings()
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg.ToLower() == Properties.Resources.ArgumentSettings) { OpenWindow(Popup.Settings); }
                if (arg.ToLower() == Properties.Resources.ArgumentDebug) { UMDebug.Visibility = Visibility.Visible; }
            }
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                Watcher.EnableRaisingEvents = false;
                WatcherUser.EnableRaisingEvents = false;

                Application.Current.Dispatcher.Invoke(delegate
                {
                    int check = forceLanguage;

                    BuildMenu(skipBuildModules:false);
                    CheckNotifications(false);

                    if (check != forceLanguage) { LanguageMenuItemCheck(); }
                });
            }
            catch { }
            finally
            {
                Watcher.EnableRaisingEvents = true;
                WatcherUser.EnableRaisingEvents = true;
            }
        }

        private static void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool updateLanguage = false;

            try { updateLanguage = (bool)e.Argument; }
            catch { updateLanguage = false; }

            try
            {
                if (updateLanguage)
                {
                    UpdateTranslations();
                    UpdateDefaultMenuItems();
                }

                UpdateMenuItems();
                CheckNotifications(false);

                Application.Current.Dispatcher.Invoke(delegate
                {
                    if (notifyStatus.Max() == 0) { UMicon.ToolTipText = GetTranslation(Properties.Resources.UMIconText); }
                    else { UMicon.ToolTipText = GetTranslation(Properties.Resources.UMIconTextNotify); }
                });

                BuildModules(true);
            }
            catch { return; }
        }

        public static void ExecuteWorker(bool languageUpdate)
        {
            if (Worker.IsBusy && !languageUpdate) { return; }
            else if (Worker.IsBusy && languageUpdate)
            {
                BackgroundWorker langWorker = new BackgroundWorker();
                langWorker.DoWork += new DoWorkEventHandler(Worker_DoWork);
                langWorker.RunWorkerAsync(languageUpdate);
            }
            else
            { Worker.RunWorkerAsync(languageUpdate); }
        }

        static void AddressChangedCallback(object sender, EventArgs e)
        {
            if (!allowConnection || !timerConnectionReady) { return; }
            if ((DateTime.Now - timerConnectionLastRun).TotalHours < 4) { return; }

            timerConnectionReady = false;

            if (connectionTimer > 0)
            { System.Threading.Thread.Sleep(connectionTimer * 1000 * 60); }

            Variable var = new Variable();
            string input = var.WinUpdateTime;
            DateTime output;
            double comparison;

            if (!DateTime.TryParse(input, out output)) { return; }
            comparison = (DateTime.Now - output).TotalDays;

            if (comparison >= checkupMinDays)
            {
                try
                {
                    WebRequest request = WebRequest.Create("https://wsus.it.helsinki.fi");
                    request.Timeout = 5000;
                    WebResponse response = request.GetResponse();
                    response.Close();

                    Execute("wuauclt.exe", "/detectnow /updatenow");
                    timerConnectionLastRun = DateTime.Now;
                }
                catch { }
            }

            timerConnectionReady = true;
        }

        public static TaskbarIcon UMicon = new TaskbarIcon
        {
            Visibility = Visibility.Collapsed,
            MenuActivation = PopupActivationMode.LeftOrRightClick,
            NoLeftClickDelay = true,
            OverridesDefaultStyle = true
        };

        private void UMicon_TrayMouseDown(object sender, RoutedEventArgs e)
        {
            ExecuteWorker(false);
        }

        private void UMicon_TrayBalloonTipClicked(object sender, RoutedEventArgs e)
        {
            UMmenu.IsOpen = true;
        }

        static ContextMenu UMmenu = new ContextMenu();

        FileSystemWatcher Watcher = new FileSystemWatcher()
        {
            Path = xmlFolder,
            IncludeSubdirectories = false,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size,
            Filter = "um*.xml"
        };

        FileSystemWatcher WatcherUser = new FileSystemWatcher()
        {
            IncludeSubdirectories = false,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size,
            Filter = Properties.Resources.XMLFileFilter
        };

        private static readonly BackgroundWorker Worker = new BackgroundWorker { WorkerSupportsCancellation = true };
        private static DispatcherTimer BackgroundTimer = new DispatcherTimer();

        private void ModernWindow_Closing(object sender, CancelEventArgs e)
        {
            try { if (uiWindow != null) { uiWindow.Close(); } }
            catch { }
        }
    }
}
