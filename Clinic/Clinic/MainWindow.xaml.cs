using System;
using System.Windows;
using System.Windows.Controls;

namespace Clinic
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Db.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось подключиться к базе данных: " + ex.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Page home = new Page();
            home.Title = "Главная";
            home.Content = new TextBlock
            {
                Text = "Выберите таблицу в меню слева",
                FontSize = 18,
                Margin = new Thickness(20)
            };
            MainFrame.Navigate(home);
        }

        private void Patients_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TablePage(Tables.Patients));
        }

        private void Doctors_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TablePage(Tables.Doctors));
        }

        private void Cabinets_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TablePage(Tables.Cabinets));
        }

        private void Appointments_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TablePage(Tables.Appointments));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
                MainFrame.GoBack();
        }
    }
}
