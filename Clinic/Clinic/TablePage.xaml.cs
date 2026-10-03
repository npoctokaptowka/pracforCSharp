using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clinic
{
    public partial class TablePage : Page
    {
        private readonly TableInfo table;
        private DataTable data;

        public TablePage(TableInfo table)
        {
            InitializeComponent();
            this.table = table;
            Title = table.Title;
            TitleText.Text = table.Title;
            Loaded += TablePage_Loaded;
        }

        private void TablePage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                data = Db.Query(table.SelectSql);
                DataTableGrid.ItemsSource = data.DefaultView;
                ApplySearch();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить данные: " + ex.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplySearch()
        {
            if (data == null)
                return;

            string text = SearchBox.Text.Trim();
            if (text == "")
            {
                data.DefaultView.RowFilter = "";
                return;
            }

            StringBuilder safe = new StringBuilder();
            foreach (char c in text)
            {
                if (c == '[' || c == ']' || c == '%' || c == '*')
                    safe.Append("[" + c + "]");
                else if (c == '\'')
                    safe.Append("''");
                else
                    safe.Append(c);
            }

            List<string> parts = new List<string>();
            foreach (DataColumn column in data.Columns)
            {
                parts.Add("Convert([" + column.ColumnName + "], 'System.String') LIKE '%" + safe + "%'");
            }
            data.DefaultView.RowFilter = string.Join(" OR ", parts);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplySearch();
        }

        private long? GetSelectedId()
        {
            DataRowView row = DataTableGrid.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Сначала выберите запись в таблице.",
                    "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            return Convert.ToInt64(row["№"]);
        }

        private void OpenEditWindow(long? id)
        {
            EditWindow window = new EditWindow(table, id);
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            OpenEditWindow(null);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            long? id = GetSelectedId();
            if (id != null)
                OpenEditWindow(id);
        }

        private void DataTableGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataTableGrid.SelectedItem != null)
                OpenEditWindow(GetSelectedId());
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            long? id = GetSelectedId();
            if (id == null)
                return;

            MessageBoxResult answer = MessageBox.Show(
                "Удалить выбранную запись?\nСвязанные записи в других таблицах тоже будут удалены.",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer != MessageBoxResult.Yes)
                return;

            try
            {
                Db.Execute("DELETE FROM " + table.Name + " WHERE Id = @Id",
                    new Dictionary<string, object> { { "Id", id } });
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось удалить запись: " + ex.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}
