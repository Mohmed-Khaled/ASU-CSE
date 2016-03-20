using System.Windows.Controls;

namespace OSScheduler.GUI
{
    /// <summary>
    /// Interaction logic for ChartBar.xaml
    /// </summary>
    public partial class ChartBar
    {
        public ChartBar()
        {
            InitializeComponent();
            Height = 60;
        }
    
        public ChartBar(string name, int time) : this()
        {
            ProcessName.Content = name;
            ProcessTime.Content = time;
            Width = 30 * time;
        }
    }
}
