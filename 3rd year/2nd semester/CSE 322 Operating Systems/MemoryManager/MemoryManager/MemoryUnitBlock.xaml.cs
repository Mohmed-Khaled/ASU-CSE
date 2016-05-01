using System;
using System.Windows.Media;

namespace MemoryManager
{
    /// <summary>
    /// Interaction logic for MemoryUnitBlock.xaml
    /// </summary>
    public partial class MemoryUnitBlock
    {
        public MemoryUnitBlock(MemoryUnit unit)
        {
            InitializeComponent();
            UnitStart.Content = unit.StartingAddress;
            UnitName.Content = unit.Name;
            UnitSize.Content = unit.Size;
            UnitEnd.Content = unit.EndAddress;
            switch (unit.Type)
            {
                case UnitType.Hole:
                    Panel.Background = Brushes.LightGreen;
                    break;
                case UnitType.Process:
                    Panel.Background = Brushes.Orange;
                    break;
                case UnitType.Reserved:
                    Panel.Background = Brushes.Red;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
