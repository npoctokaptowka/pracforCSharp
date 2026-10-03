using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace prac15
{
    public partial class MainWindow : Window
    {
        private string currentFilePath = "";
        private bool isTextChanged = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeFormattingToolbar();
            isTextChanged = false;
        }

        private void InitializeFormattingToolbar()
        {
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                FontFamilyComboBox.Items.Add(fontFamily.Source);
            }
            FontFamilyComboBox.SelectedItem = "Consolas";

            string[] sizes = { "8", "10", "12", "14", "16", "18", "20", "24", "28", "32", "36", "48", "72" };
            foreach (string size in sizes)
            {
                FontSizeComboBox.Items.Add(size);
            }
            FontSizeComboBox.SelectedItem = "16";

            FontColorComboBox.SelectedIndex = 0;
        }

        private string GetEditorText()
        {
            TextRange textRange = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
            string text = textRange.Text;
            if (text.EndsWith("\r\n"))
            {
                text = text.Substring(0, text.Length - 2);
            }
            return text;
        }

        private void SetEditorText(string text)
        {
            Editor.Document.Blocks.Clear();
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(text);
            Editor.Document.Blocks.Add(paragraph);
        }

        private void ClearEditor()
        {
            Editor.Document.Blocks.Clear();
            Editor.Document.Blocks.Add(new Paragraph());
        }

        private TextPointer GetPointer(int charIndex)
        {
            TextPointer pointer = Editor.Document.ContentStart;
            int count = 0;
            while (pointer != null)
            {
                TextPointerContext context = pointer.GetPointerContext(LogicalDirection.Forward);
                if (context == TextPointerContext.Text)
                {
                    string run = pointer.GetTextInRun(LogicalDirection.Forward);
                    if (count + run.Length >= charIndex)
                    {
                        return pointer.GetPositionAtOffset(charIndex - count);
                    }
                    count += run.Length;
                    pointer = pointer.GetPositionAtOffset(run.Length);
                }
                else
                {
                    if (context == TextPointerContext.ElementEnd
                        && pointer.GetAdjacentElement(LogicalDirection.Forward) is Paragraph)
                    {
                        count += 2;
                    }
                    else if (context == TextPointerContext.ElementStart
                        && pointer.GetAdjacentElement(LogicalDirection.Forward) is LineBreak)
                    {
                        count += 2;
                    }
                    pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
                }
            }
            return Editor.Document.ContentEnd;
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSaveBeforeAction())
                return;
            ClearEditor();
            currentFilePath = "";
            isTextChanged = false;
            Title = "Новый файл — Блокнот";
            UpdateStatus();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckSaveBeforeAction())
                return;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Открыть файл";
            dialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                SetEditorText(File.ReadAllText(dialog.FileName, Encoding.UTF8));
                currentFilePath = dialog.FileName;
                isTextChanged = false;
                Title = currentFilePath + " — Блокнот";
                UpdateStatus();
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileAs();
        }

        private bool SaveFile()
        {
            if (currentFilePath == "")
            {
                return SaveFileAs();
            }
            File.WriteAllText(currentFilePath, GetEditorText(), Encoding.UTF8);
            isTextChanged = false;
            Title = currentFilePath + " — Блокнот";
            UpdateStatus();
            return true;
        }

        private bool SaveFileAs()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Сохранить файл как";
            dialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            dialog.FileName = "Новый документ.txt";
            if (dialog.ShowDialog() == true)
            {
                currentFilePath = dialog.FileName;
                return SaveFile();
            }
            return false;
        }

        private void PrintFile_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(Editor, "Печать документа");
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool CheckSaveBeforeAction()
        {
            if (!isTextChanged)
                return true;
            MessageBoxResult result = MessageBox.Show(
                "Файл был изменён. Сохранить изменения?",
                "Сохранение",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                return SaveFile();
            if (result == MessageBoxResult.No)
                return true;
            return false;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!CheckSaveBeforeAction())
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (Editor.CanUndo)
                Editor.Undo();
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (Editor.CanRedo)
                Editor.Redo();
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            Editor.Cut();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Editor.Copy();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            Editor.Paste();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (!Editor.Selection.IsEmpty)
            {
                Editor.Selection.Text = "";
            }
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            Editor.SelectAll();
        }

        private void WordWrap_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                if (item.IsChecked)
                    Editor.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                else
                    Editor.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
        }

        private void IncreaseFont_Click(object sender, RoutedEventArgs e)
        {
            object value = Editor.Selection.GetPropertyValue(TextElement.FontSizeProperty);
            double currentSize = 16;
            if (value != DependencyProperty.UnsetValue)
                currentSize = (double)value;
            double newSize = currentSize + 2;
            Editor.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, newSize);
            FontSizeComboBox.Text = newSize.ToString();
            Editor.Focus();
        }

        private void DecreaseFont_Click(object sender, RoutedEventArgs e)
        {
            object value = Editor.Selection.GetPropertyValue(TextElement.FontSizeProperty);
            double currentSize = 16;
            if (value != DependencyProperty.UnsetValue)
                currentSize = (double)value;
            if (currentSize > 8)
            {
                double newSize = currentSize - 2;
                Editor.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, newSize);
                FontSizeComboBox.Text = newSize.ToString();
            }
            Editor.Focus();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "WPF-Блокнот\n\nПрограмма позволяет создавать, открывать, редактировать, сохранять и печатать текстовые файлы.",
                "О программе",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            isTextChanged = true;
            if (currentFilePath == "")
                Title = "Новый файл * — Блокнот";
            else
                Title = currentFilePath + " * — Блокнот";
            UpdateStatus();
        }

        private void Editor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (LineColumnText == null)
                return;

            string text = GetEditorText();
            TextRange rangeToCaret = new TextRange(Editor.Document.ContentStart, Editor.CaretPosition);
            string textBeforeCaret = rangeToCaret.Text;
            if (textBeforeCaret.EndsWith("\r\n"))
            {
                textBeforeCaret = textBeforeCaret.Substring(0, textBeforeCaret.Length - 2);
            }

            int line = 1;
            int column = 1;
            for (int i = 0; i < textBeforeCaret.Length; i++)
            {
                if (textBeforeCaret[i] == '\n')
                {
                    line++;
                    column = 1;
                }
                else if (textBeforeCaret[i] != '\r')
                {
                    column++;
                }
            }

            LineColumnText.Text = "Строка: " + line + ", Столбец: " + column;
            CharCountText.Text = "Символов: " + text.Length;
            FilePathText.Text = currentFilePath == "" ? "Новый файл" : currentFilePath;
        }

        private void ShowFindPanel_Click(object sender, RoutedEventArgs e)
        {
            FindPanel.Visibility = Visibility.Visible;
            ReplaceTextBlock.Visibility = Visibility.Collapsed;
            ReplaceTextBox.Visibility = Visibility.Collapsed;
            ReplaceButton.Visibility = Visibility.Collapsed;
            ReplaceAllButton.Visibility = Visibility.Collapsed;
            FindTextBox.Focus();
        }

        private void ShowReplacePanel_Click(object sender, RoutedEventArgs e)
        {
            FindPanel.Visibility = Visibility.Visible;
            ReplaceTextBlock.Visibility = Visibility.Visible;
            ReplaceTextBox.Visibility = Visibility.Visible;
            ReplaceButton.Visibility = Visibility.Visible;
            ReplaceAllButton.Visibility = Visibility.Visible;
            FindTextBox.Focus();
        }

        private void CloseFindPanel_Click(object sender, RoutedEventArgs e)
        {
            FindPanel.Visibility = Visibility.Collapsed;
            Editor.Focus();
        }

        private StringComparison GetComparison()
        {
            if (CaseSensitiveCheckBox.IsChecked == true)
                return StringComparison.CurrentCulture;
            return StringComparison.CurrentCultureIgnoreCase;
        }

        private bool CheckFindText(string title)
        {
            if (FindTextBox.Text == "")
            {
                MessageBox.Show("Введите текст для поиска.", title, MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            return true;
        }

        private void FindNext_Click(object sender, RoutedEventArgs e)
        {
            FindNext();
        }

        private void FindNext()
        {
            if (!CheckFindText("Поиск"))
                return;

            string findText = FindTextBox.Text;
            string text = GetEditorText();
            int startIndex = new TextRange(Editor.Document.ContentStart, Editor.Selection.End).Text.Length;
            if (startIndex > text.Length)
                startIndex = text.Length;

            int index = text.IndexOf(findText, startIndex, GetComparison());
            if (index == -1 && startIndex > 0)
                index = text.IndexOf(findText, 0, GetComparison());

            if (index >= 0)
            {
                Editor.Focus();
                Editor.Selection.Select(GetPointer(index), GetPointer(index + findText.Length));
            }
            else
            {
                MessageBox.Show("Текст не найден.", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFindText("Замена"))
                return;
            if (Editor.Selection.Text.Equals(FindTextBox.Text, GetComparison()))
            {
                Editor.Selection.Text = ReplaceTextBox.Text;
            }
            FindNext();
        }

        private void ReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFindText("Замена"))
                return;

            string findText = FindTextBox.Text;
            string replaceText = ReplaceTextBox.Text;
            int count = 0;
            int position = 0;

            while (true)
            {
                string text = GetEditorText();
                if (position > text.Length)
                    break;
                int index = text.IndexOf(findText, position, GetComparison());
                if (index == -1)
                    break;
                TextRange range = new TextRange(GetPointer(index), GetPointer(index + findText.Length));
                range.Text = replaceText;
                position = index + replaceText.Length;
                count++;
            }

            MessageBox.Show("Выполнено замен: " + count, "Замена", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            Editor.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            Editor.Focus();
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            Editor.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            Editor.Focus();
        }

        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            Editor.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            Editor.Focus();
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyComboBox.SelectedItem == null)
                return;
            Editor.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty,
                new FontFamily(FontFamilyComboBox.SelectedItem.ToString()));
            Editor.Focus();
        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeComboBox.SelectedItem == null)
                return;
            double selectedSize;
            if (double.TryParse(FontSizeComboBox.SelectedItem.ToString(), out selectedSize))
            {
                Editor.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, selectedSize);
            }
            Editor.Focus();
        }

        private void FontColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = FontColorComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem == null)
                return;
            BrushConverter converter = new BrushConverter();
            Brush brush = converter.ConvertFromString(selectedItem.Tag.ToString()) as Brush;
            if (brush != null)
            {
                Editor.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
            }
            Editor.Focus();
        }

        private void AlignLeft_Click(object sender, RoutedEventArgs e)
        {
            Editor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
            Editor.Focus();
        }

        private void AlignCenter_Click(object sender, RoutedEventArgs e)
        {
            Editor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
            Editor.Focus();
        }

        private void AlignRight_Click(object sender, RoutedEventArgs e)
        {
            Editor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
            Editor.Focus();
        }

        private void AlignJustify_Click(object sender, RoutedEventArgs e)
        {
            Editor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Justify);
            Editor.Focus();
        }

        private void DocStyleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = DocStyleComboBox.SelectedItem as ComboBoxItem;
            if (item == null)
                return;
            string mode = item.Content.ToString();

            string font;
            double size;
            Brush color;
            Brush background;
            double spacing;

            switch (mode)
            {
                case "Учебный конспект":
                    font = "Calibri";
                    size = 16;
                    color = Brushes.Black;
                    background = new SolidColorBrush(Color.FromRgb(255, 253, 231));
                    spacing = 1.5;
                    break;
                case "Официальная записка":
                    font = "Times New Roman";
                    size = 14;
                    color = Brushes.Black;
                    background = Brushes.White;
                    spacing = 1.15;
                    break;
                case "Черновик":
                    font = "Courier New";
                    size = 14;
                    color = Brushes.Gray;
                    background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                    spacing = 1.0;
                    break;
                case "Код программы":
                    font = "Consolas";
                    size = 14;
                    color = Brushes.LightGreen;
                    background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
                    spacing = 1.15;
                    break;
                default:
                    font = "Arial";
                    size = 22;
                    color = Brushes.DarkBlue;
                    background = new SolidColorBrush(Color.FromRgb(227, 242, 253));
                    spacing = 2.0;
                    break;
            }

            TextRange range;
            if (WholeDocumentCheckBox.IsChecked == true)
            {
                range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
                Editor.Document.FontFamily = new FontFamily(font);
                Editor.Document.FontSize = size;
                Editor.Document.Foreground = color;
                Editor.Document.LineHeight = size * spacing;
            }
            else
            {
                if (Editor.Selection.IsEmpty)
                {
                    MessageBox.Show("Выделите текст или включите галочку «Применить ко всему документу».",
                        "Оформление", MessageBoxButton.OK, MessageBoxImage.Information);
                    DocStyleComboBox.SelectedIndex = -1;
                    return;
                }
                range = new TextRange(Editor.Selection.Start, Editor.Selection.End);
            }

            range.ApplyPropertyValue(TextElement.FontFamilyProperty, new FontFamily(font));
            range.ApplyPropertyValue(TextElement.FontSizeProperty, size);
            range.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            SetLineHeight(range, size * spacing);
            Editor.Background = background;

            DocStyleText.Text = "Оформление: " + mode;
            Editor.Focus();
        }

        private void SetLineHeight(TextRange range, double lineHeight)
        {
            foreach (Block block in Editor.Document.Blocks)
            {
                if (block is Paragraph paragraph
                    && paragraph.ContentEnd.CompareTo(range.Start) >= 0
                    && paragraph.ContentStart.CompareTo(range.End) <= 0)
                {
                    paragraph.LineHeight = lineHeight;
                }
            }
        }
    }
}
