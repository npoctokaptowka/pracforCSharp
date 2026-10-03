using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace prac12
{
    public partial class MainWindow : Window
    {
        int actionCount = 0;
        FrameworkElement selectedElement = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateTitle()
        {
            if (selectedElement != null)
                this.Title = $"{selectedElement.Name} | Действий: {actionCount}";
        }

        private bool SameColor(Brush a, Brush b)
        {
            return a is SolidColorBrush x && b is SolidColorBrush y && x.Color == y.Color;
        }

        private void Shape_Click(object sender, MouseButtonEventArgs e)
        {
            selectedElement = (FrameworkElement)sender;
            UpdateTitle();
        }

        private void Color_Left(object sender, MouseButtonEventArgs e)
        {
            if (selectedElement == null) return;
            Brush color = ((Button)sender).Background;

            if (selectedElement is Rectangle rect)
            {
                if (SameColor(rect.Stroke, color)) return;
                rect.Fill = color;
            }
            else if (selectedElement is Ellipse ellipse)
            {
                if (SameColor(ellipse.Stroke, color)) return;
                ellipse.Fill = color;
            }
            else if (selectedElement is Border border)
            {
                if (SameColor(border.BorderBrush, color)) return;
                border.Background = color;
            }

            actionCount++;
            UpdateTitle();
        }

        private void Color_Right(object sender, MouseButtonEventArgs e)
        {
            if (selectedElement == null) return;
            Brush color = ((Button)sender).Background;

            if (selectedElement is Rectangle rect)
            {
                if (SameColor(rect.Fill, color)) return;
                rect.Stroke = color;
            }
            else if (selectedElement is Ellipse ellipse)
            {
                if (SameColor(ellipse.Fill, color)) return;
                ellipse.Stroke = color;
            }
            else if (selectedElement is Border border)
            {
                if (SameColor(border.Background, color)) return;
                border.BorderBrush = color;
            }

            actionCount++;
            UpdateTitle();
        }
    }
}