using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Clinic
{
    public partial class EditWindow : Window
    {
        private readonly TableInfo table;
        private readonly long? id;
        private readonly Dictionary<string, Control> inputs = new Dictionary<string, Control>();

        public EditWindow(TableInfo table, long? id)
        {
            InitializeComponent();
            this.table = table;
            this.id = id;

            Title = (id == null ? "Добавление: " : "Изменение: ") + table.Title;
            HeaderText.Text = Title;

            BuildFields();
            if (id != null)
                LoadValues();
        }

        private void BuildFields()
        {
            for (int i = 0; i < table.Fields.Count; i++)
            {
                FieldInfo field = table.Fields[i];
                FieldsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                TextBlock label = new TextBlock
                {
                    Text = field.Caption + ":",
                    Margin = new Thickness(0, 5, 10, 5),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(label, i);
                FieldsGrid.Children.Add(label);

                Control input;
                if (field.Kind == FieldKind.Lookup)
                {
                    ComboBox combo = new ComboBox
                    {
                        DisplayMemberPath = "Name",
                        SelectedValuePath = "Id"
                    };
                    try
                    {
                        combo.ItemsSource = Db.Query(field.LookupSql).DefaultView;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не удалось загрузить список «" + field.Caption + "»: " + ex.Message,
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    input = combo;
                }
                else
                {
                    input = new TextBox();
                }

                input.Margin = new Thickness(0, 5, 0, 5);
                Grid.SetRow(input, i);
                Grid.SetColumn(input, 1);
                FieldsGrid.Children.Add(input);
                inputs[field.Name] = input;
            }
        }

        private void LoadValues()
        {
            try
            {
                DataTable result = Db.Query("SELECT * FROM " + table.Name + " WHERE Id = @Id",
                    new Dictionary<string, object> { { "Id", id } });
                if (result.Rows.Count == 0)
                {
                    MessageBox.Show("Запись не найдена. Возможно, она уже удалена.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DataRow row = result.Rows[0];
                foreach (FieldInfo field in table.Fields)
                {
                    if (inputs[field.Name] is ComboBox combo)
                        combo.SelectedValue = row[field.Name];
                    else
                        ((TextBox)inputs[field.Name]).Text = row[field.Name].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить запись: " + ex.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string Validate(FieldInfo field, out object value)
        {
            value = null;

            if (field.Kind == FieldKind.Lookup)
            {
                ComboBox combo = (ComboBox)inputs[field.Name];
                if (combo.Items.Count == 0)
                    return "Список «" + field.Caption + "» пуст. Сначала добавьте записи в связанную таблицу.";
                if (combo.SelectedValue == null)
                    return "Выберите значение в поле «" + field.Caption + "».";
                value = combo.SelectedValue;
                return null;
            }

            string text = ((TextBox)inputs[field.Name]).Text.Trim();
            if (text == "")
                return "Заполните поле «" + field.Caption + "».";
            if (text.Length > field.MaxLength)
                return "Поле «" + field.Caption + "» не должно быть длиннее " + field.MaxLength + " символов.";

            switch (field.Kind)
            {
                case FieldKind.Name:
                    if (!Regex.IsMatch(text, @"^[\p{L}\s\-]+$"))
                        return "Поле «" + field.Caption + "» может содержать только буквы, пробелы и дефис.";
                    break;

                case FieldKind.Int:
                    int number;
                    if (!int.TryParse(text, out number) || number < field.Min || number > field.Max)
                        return "В поле «" + field.Caption + "» должно быть целое число от " + field.Min + " до " + field.Max + ".";
                    value = number;
                    return null;

                case FieldKind.Date:
                    DateTime date;
                    if (!DateTime.TryParseExact(text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                        || date > DateTime.Today || date.Year < 1900)
                        return "В поле «" + field.Caption + "» введите дату в формате ДД.ММ.ГГГГ, не позже сегодняшнего дня.";
                    break;

                case FieldKind.DateTime:
                    DateTime dateTime;
                    if (!DateTime.TryParseExact(text, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                        return "В поле «" + field.Caption + "» введите дату и время в формате ДД.ММ.ГГГГ ЧЧ:ММ, например 05.10.2026 09:30.";
                    break;

                case FieldKind.Phone:
                    if (!Regex.IsMatch(text, @"^\+?[0-9\s\-\(\)]{6,20}$"))
                        return "В поле «" + field.Caption + "» введите телефон: цифры, пробелы, скобки, дефис, можно + в начале.";
                    break;
            }

            value = text;
            return null;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            foreach (FieldInfo field in table.Fields)
            {
                object value;
                string error = Validate(field, out value);
                if (error != null)
                {
                    MessageBox.Show(error, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                    inputs[field.Name].Focus();
                    return;
                }
                values[field.Name] = value;
            }

            try
            {
                if (id == null)
                {
                    string columns = string.Join(", ", values.Keys);
                    string parameters = string.Join(", ", values.Keys.Select(k => "@" + k));
                    Db.Execute("INSERT INTO " + table.Name + " (" + columns + ") VALUES (" + parameters + ")", values);
                }
                else
                {
                    string sets = string.Join(", ", values.Keys.Select(k => k + " = @" + k));
                    values["Id"] = id;
                    Db.Execute("UPDATE " + table.Name + " SET " + sets + " WHERE Id = @Id", values);
                }
                DialogResult = true;
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                MessageBox.Show("Такая запись уже существует или связанная запись была удалена.",
                    "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось сохранить запись: " + ex.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
