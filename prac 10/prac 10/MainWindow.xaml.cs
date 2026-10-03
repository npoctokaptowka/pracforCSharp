using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prac_10
{
    public partial class MainWindow : Window
    {
        string baseTitle = "Старт";

        public MainWindow()
        {
            InitializeComponent();
            this.Title = baseTitle;
        }

        private void TitleButtonClick(object sender, RoutedEventArgs e)
        {
            this.Title += "!";
            if (this.Title.Length > 20)
            {
                this.Title = baseTitle;
            }
        }
    }
}