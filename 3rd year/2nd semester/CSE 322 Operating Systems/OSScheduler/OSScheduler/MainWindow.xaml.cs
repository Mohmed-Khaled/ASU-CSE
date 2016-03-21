using System.Windows;
using System.Windows.Controls;

namespace OSScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private RadioButton _alg;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            int numberOfProcesses;
            if (int.TryParse(NumberOfProcesses.Text, out numberOfProcesses) && _alg != null)
            {
                if (numberOfProcesses > 0)
                {
                    var dataEntry = new DataEntry(numberOfProcesses, _alg.Tag.ToString());
                    dataEntry.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Number of processes must be positive integer", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Please enter valid inputs.","Error",
                    MessageBoxButton.OK,MessageBoxImage.Error);

            }
        }


        private void AlgSelected(object sender, RoutedEventArgs e)
        {
            _alg = sender as RadioButton;
        }
    }
}
