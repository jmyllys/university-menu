using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace University_Menu.UserControls
{
    /// <summary>
    /// Interaction logic for FancyBalloon.xaml
    /// </summary>
    public partial class FancyBalloon : UserControl
    {
        private bool isClosing = false;
        private MainWindow.Popup popup;
        private DispatcherTimer closingTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(MainWindow.balloonShowTime) };

        public FancyBalloon(string header, string text, MainWindow.Popup source)
        {
            InitializeComponent();
            TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);
            closingTimer.Tick += ClosingTimer_Tick;

            FBHeader.Text = header;
            FBText.Text = text;
            popup = source;
        }

        private void CloseBalloon()
        {
            if (isClosing) { return; }

            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.CloseBalloon();
        }

        private void OnBalloonClosing(object sender, RoutedEventArgs e)
        {
            isClosing = true;
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isClosing) return;
            if (closingTimer.IsEnabled) { closingTimer.Stop(); }

            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.ResetBalloonCloseTimer();
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isClosing) return;

            closingTimer.Start();
        }

        private void ClosingTimer_Tick(object sender, EventArgs e)
        {
            closingTimer.Stop();
            CloseBalloon();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isClosing)
            {
                CloseBalloon();
                MainWindow.ShowNotification(true, popup, true);
            }
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseBalloon();
        }

        private void Close_MouseEnter(object sender, MouseEventArgs e)
        {
            Close.Fill = Brushes.Black;
        }

        private void Close_MouseLeave(object sender, MouseEventArgs e)
        {
            Close.Fill = Brushes.Gray;
        }
    }
}
