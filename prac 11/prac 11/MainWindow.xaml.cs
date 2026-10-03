using System;
using System.Windows;
using System.Windows.Threading;

namespace prac_11
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            ClockText.Text = DateTime.Now.ToString("HH:mm:ss");
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ClockText.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void LeftClick(object sender, RoutedEventArgs e)
        {
            Rect area = SystemParameters.WorkArea;
            Left = area.Left;
            Top = area.Top + (area.Height - ActualHeight) / 2;
        }

        private void RightClick(object sender, RoutedEventArgs e)
        {
            Rect area = SystemParameters.WorkArea;
            Left = area.Right - ActualWidth;
            Top = area.Top + (area.Height - ActualHeight) / 2;
        }

        private void TopClick(object sender, RoutedEventArgs e)
        {
            Rect area = SystemParameters.WorkArea;
            Left = area.Left + (area.Width - ActualWidth) / 2;
            Top = area.Top;
        }

        private void BottomClick(object sender, RoutedEventArgs e)
        {
            Rect area = SystemParameters.WorkArea;
            Left = area.Left + (area.Width - ActualWidth) / 2;
            Top = area.Bottom - ActualHeight;
        }
    }
}