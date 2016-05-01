using System.Windows;

namespace MemoryManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            System.Threading.Thread.Sleep(1500);
            InitializeComponent();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            uint numberOfHoles,memorySize;
            if (uint.TryParse(NumberOfHoles.Text, out numberOfHoles) 
                && uint.TryParse(MemorySize.Text, out memorySize))
            {
                var holesInfo = new HolesInfo(numberOfHoles,memorySize);
                holesInfo.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Please enter valid inputs.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
