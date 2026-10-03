using System;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Printing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace prac14
{
    public partial class MainWindow : Window
    {
        double a = 0;
        string op = "";
        bool newInput = true;
        string system = "DEC";
        int precision = 2;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InputBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "[0-9\\.-]");
        }

        private void InputBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Equal_Click(sender, e);
        }

        double GetVal()
        {
            double v;
            double.TryParse(InputBox.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out v);
            return v;
        }

        void SetVal(double v)
        {
            InputBox.Text = v.ToString("F" + precision, CultureInfo.InvariantCulture);
        }

        void Add(string t)
        {
            if (newInput || InputBox.Text == "0")
            {
                InputBox.Text = t;
                newInput = false;
            }
            else
                InputBox.Text += t;
        }

        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            Add(((Button)sender).Content.ToString());
        }

        private void Dot_Click(object sender, RoutedEventArgs e)
        {
            if (!InputBox.Text.Contains("."))
                Add(".");
        }

        private void Op_Click(object sender, RoutedEventArgs e)
        {
            a = GetVal();
            op = ((Button)sender).Content.ToString();
            newInput = true;
        }

        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            double b = GetVal();
            double r = 0;
            try
            {
                switch (op)
                {
                    case "+": r = a + b; break;
                    case "-": r = a - b; break;
                    case "*": r = a * b; break;
                    case "/":
                        if (b == 0) throw new Exception("Деление на 0");
                        r = a / b;
                        break;
                }
                SetVal(r);
                ConvertSystem(r);
                newInput = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Text = "0";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (InputBox.Text.Length > 1)
                InputBox.Text = InputBox.Text.Substring(0, InputBox.Text.Length - 1);
            else
                InputBox.Text = "0";
        }

        private void Sign_Click(object sender, RoutedEventArgs e)
        {
            SetVal(-GetVal());
        }

        private void Sqrt_Click(object sender, RoutedEventArgs e)
        {
            double v = GetVal();
            if (v < 0)
            {
                MessageBox.Show("Ошибка");
                return;
            }
            SetVal(Math.Sqrt(v));
        }

        private void System_Checked(object sender, RoutedEventArgs e)
        {
            system = ((RadioButton)sender).Content.ToString();
            ConvertSystem(GetVal());
        }

        void ConvertSystem(double v)
        {
            int n = (int)Math.Round(v);
            if (system == "BIN")
                InputBox.Text = Convert.ToString(n, 2);
            else if (system == "HEX")
                InputBox.Text = Convert.ToString(n, 16).ToUpper();
            else
                SetVal(v);
        }

        private void Precision_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (InputBox == null) return;
            precision = int.Parse(((ComboBoxItem)PrecisionBox.SelectedItem).Content.ToString());
            if (system == "DEC")
                SetVal(GetVal());
        }

        private void Color_Checked(object sender, RoutedEventArgs e)
        {
            string c = ((RadioButton)sender).Content.ToString();
            if (c == "Белый") InputBox.Background = Brushes.White;
            if (c == "Серый") InputBox.Background = Brushes.LightGray;
            if (c == "Жёлтый") InputBox.Background = Brushes.LightYellow;
            if (c == "Зелёный") InputBox.Background = Brushes.LightGreen;
        }

        private void Font_Checked(object sender, RoutedEventArgs e)
        {
            int size = int.Parse(((RadioButton)sender).Tag.ToString());
            InputBox.FontSize = size;
        }

        private void Style_Changed(object sender, RoutedEventArgs e)
        {
            bool bold = false;
            bool italic = false;
            var panel = ((CheckBox)sender).Parent as StackPanel;
            foreach (var item in panel.Children)
            {
                if (item is CheckBox cb)
                {
                    if (cb.Content.ToString() == "Жирный" && cb.IsChecked == true)
                        bold = true;
                    if (cb.Content.ToString() == "Курсив" && cb.IsChecked == true)
                        italic = true;
                }
            }
            InputBox.FontWeight = bold ? FontWeights.Bold : FontWeights.Normal;
            InputBox.FontStyle = italic ? FontStyles.Italic : FontStyles.Normal;
        }

        private void Theme_Toggled(object sender, RoutedEventArgs e)
        {
            if (DarkMode.IsChecked == true)
            {
                Background = Brushes.Black;
                InputBox.Foreground = Brushes.White;
            }
            else
            {
                Background = Brushes.White;
                InputBox.Foreground = Brushes.Black;
            }
        }

        private void Align_Changed(object sender, SelectionChangedEventArgs e)
        {
            string val = ((ComboBoxItem)AlignBox.SelectedItem).Content.ToString();
            if (val == "Лево") InputBox.TextAlignment = TextAlignment.Left;
            if (val == "Центр") InputBox.TextAlignment = TextAlignment.Center;
            if (val == "Право") InputBox.TextAlignment = TextAlignment.Right;
        }
    }
}