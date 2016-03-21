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
    
        public ChartBar(string name, double time) : this()
        {
            ProcessName.Content = name;
            ProcessTime.Content = time;
            if (time < 1)
            {
                Width = 25;
            }
            else
            {
                Width = 30 * time;
            }
        }
    }
}
