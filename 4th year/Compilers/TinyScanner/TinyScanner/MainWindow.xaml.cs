using System.Windows;

namespace TinyScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            var scanner = new Scanner(Editor.Text);
            var result =  new ResultWindow(scanner);
            result.Show();
        }
    }
}
