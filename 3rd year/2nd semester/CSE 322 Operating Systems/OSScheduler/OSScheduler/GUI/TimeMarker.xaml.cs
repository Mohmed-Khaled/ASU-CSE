using System.Windows.Controls;

namespace OSScheduler.GUI
{
    /// <summary>
    /// Interaction logic for TimeMarker.xaml
    /// </summary>
    public partial class TimeMarker
    {
        public TimeMarker()
        {
            InitializeComponent();
            
        }

        public TimeMarker(int time) : this()
        {
            Time.Content = time;
        }
    }
}
