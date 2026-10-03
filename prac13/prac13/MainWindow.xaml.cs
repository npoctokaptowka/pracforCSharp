using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace prac13
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetColor(Brush color)
        {
            Box.Background = color;
        }

        private bool DoubleConfirm(string action)
        {
            MessageBoxResult first = MessageBox.Show("Вы действительно хотите " + action + "?",
                "Подтверждение 1 из 2", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (first != MessageBoxResult.Yes)
            {
                ShowCancelled();
                return false;
            }

            MessageBoxResult second = MessageBox.Show("Это действие нельзя отменить. Точно " + action + "?",
                "Подтверждение 2 из 2", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (second != MessageBoxResult.Yes)
            {
                ShowCancelled();
                return false;
            }
            return true;
        }

        private void ShowCancelled()
        {
            MessageBox.Show("Действие отменено", "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ResetBox()
        {
            if (((SolidColorBrush)Box.Background).Color == Colors.LightGray)
            {
                MessageBox.Show("Объект уже в исходном состоянии", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!DoubleConfirm("сбросить оформление"))
            {
                return;
            }
            SetColor(Brushes.LightGray);
            MessageBox.Show("Оформление сброшено", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Red_Click(object sender, RoutedEventArgs e)
        {
            SetColor(Brushes.Red);
        }

        private void Green_Click(object sender, RoutedEventArgs e)
        {
            SetColor(Brushes.Green);
        }

        private void Blue_Click(object sender, RoutedEventArgs e)
        {
            SetColor(Brushes.Blue);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetBox();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.D1: SetColor(Brushes.Red); break;
                case Key.D2: SetColor(Brushes.Green); break;
                case Key.D3: SetColor(Brushes.Blue); break;
                case Key.R: ResetBox(); break;
                case Key.Q: Close(); break;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!DoubleConfirm("выйти из программы"))
            {
                e.Cancel = true;
            }
        }
    }
}