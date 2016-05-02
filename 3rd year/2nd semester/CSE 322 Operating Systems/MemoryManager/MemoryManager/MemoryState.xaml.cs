using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MemoryManager
{
    /// <summary>
    /// Interaction logic for MemoryState.xaml
    /// </summary>
    public partial class MemoryState
    {
        private readonly uint _memorySize;
        private List<MemoryUnit> _units;

        public MemoryState(uint memorySize,List<MemoryUnit> units)
        {
            InitializeComponent();
            _memorySize = memorySize;
            _units = units;
            InitialMemory();
        }

        private void InitialMemory()
        {
            MergeHoles();
            RenameHoles();
            AddReserved();
            RenderMemory();
        }

        private void RenderMemory()
        {
            StackPanel.Children.Clear();
            foreach (var unit in _units)
            {
                StackPanel.Children.Add(new MemoryUnitBlock(unit));
            }
        }

        private void AddReserved()
        {
            UnitsSort();
            var reservedCount = 1;
            if (_units.First().StartingAddress > 0)
            {
                _units.Add(
                    new MemoryUnit("Reserved " + reservedCount++,
                    0, _units.First().StartingAddress, UnitType.Reserved));
                UnitsSort();
            }
            for (var i = 0; i < _units.Count - 1; i++)
            {
                if ((int) _units[i + 1].StartingAddress - (int) _units[i].EndAddress <= 1) continue;
                _units.Add(
                    new MemoryUnit("Reserved " + reservedCount++,
                        _units[i].EndAddress + 1,
                        _units[i + 1].StartingAddress - _units[i].EndAddress - 1, UnitType.Reserved));
                UnitsSort();
            }
            var lastAddress = _units.Last().StartingAddress + _units.Last().Size;
            if (lastAddress >= _memorySize) return;
            _units.Add(new MemoryUnit("Reserved " + reservedCount,
                    lastAddress, _memorySize - lastAddress, UnitType.Reserved));
            UnitsSort();
        }

        private void MergeHoles()
        {
            UnitsSort();
            for (var i = 0; i < _units.Count - 1; i++)
            {
                if (_units[i + 1].Type != UnitType.Hole || _units[i].Type != UnitType.Hole) continue;
                if ((int) _units[i + 1].StartingAddress - (int) _units[i].EndAddress > 1) continue;
                _units[i].EndAddress = _units[i + 1].EndAddress;
                _units[i].Size = _units[i].EndAddress - _units[i].StartingAddress + 1;
                _units.RemoveAt(i + 1);
                i = -1;
            }
        }

        private void SplitHole(MemoryUnit hole,MemoryUnit process)
        {
            hole.StartingAddress = process.EndAddress + 1;
            hole.Size -= process.Size;
            _units.Add(process);
            UnitsSort();
            RenderMemory();
        }

        private void RenameHoles()
        {
            UnitsSort();
            var count = 1;
            foreach (var unit in _units.Where(unit => unit.Type == UnitType.Hole))
            {
                unit.Name = "Hole " + count++;
            }
        }

        public void AllocateFirstFit(string name, uint size)
        {
            var existingUnit = GetProcessByName(name);
            var allocated = false;
            if (existingUnit != null)
            {
                MessageBox.Show("Process name already exists.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                foreach (var unit in _units.Where(unit => unit.Type == UnitType.Hole))
                {
                    if (unit.Size == size)
                    {
                        unit.Name = name;
                        unit.Type = UnitType.Process;
                        allocated = true;
                        RenderMemory();
                        break;
                    }
                    else if (unit.Size > size)
                    {
                        SplitHole(unit,new MemoryUnit(name,unit.StartingAddress,size,UnitType.Process));
                        allocated = true;
                        break;
                    }
                }
            }
            if (!allocated)
            {
                MessageBox.Show("Could not allocate process.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AllocateBestFit(string name, uint size)
        {
            var existingUnit = GetProcessByName(name);
            var allocated = false;
            if (existingUnit != null)
            {
                MessageBox.Show("Process name already exists.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var orderedEnumerable = _units.OrderBy(x => x.Size);
                var tmpUnits = new List<MemoryUnit>(orderedEnumerable);
                foreach (var unit in tmpUnits.Where(unit => unit.Type == UnitType.Hole))
                {
                    if (unit.Size == size)
                    {
                        unit.Name = name;
                        unit.Type = UnitType.Process;
                        allocated = true;
                        RenderMemory();
                        break;
                    }
                    else if (unit.Size > size)
                    {
                        SplitHole(unit, new MemoryUnit(name, unit.StartingAddress, size, UnitType.Process));
                        allocated = true;
                        break;
                    }
                }
            }
            if (!allocated)
            {
                MessageBox.Show("Could not allocate process.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AllocateWorstFit(string name, uint size)
        {
            var existingUnit = GetProcessByName(name);
            var allocated = false;
            if (existingUnit != null)
            {
                MessageBox.Show("Process name already exists.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var orderedEnumerable = _units.OrderByDescending(x => x.Size);
                var tmpUnits = new List<MemoryUnit>(orderedEnumerable);
                foreach (var unit in tmpUnits.Where(unit => unit.Type == UnitType.Hole))
                {
                    if (unit.Size == size)
                    {
                        unit.Name = name;
                        unit.Type = UnitType.Process;
                        allocated = true;
                        RenderMemory();
                        break;
                    }
                    else if (unit.Size > size)
                    {
                        SplitHole(unit, new MemoryUnit(name, unit.StartingAddress, size, UnitType.Process));
                        allocated = true;
                        break;
                    }
                }
            }
            if (!allocated)
            {
                MessageBox.Show("Could not allocate process.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Deallocate_Click(object sender, RoutedEventArgs e)
        {
            if (!(ProcessName2.Text.Length > 0))
            {
                MessageBox.Show("Please enter valid inputs.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var name = ProcessName2.Text;
            var unit = GetProcessByName(name);
            if (unit == null)
            {
                MessageBox.Show("Wrong process name.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                unit.Name = "Hole";
                unit.Type = UnitType.Hole;
                MergeHoles();
                RenameHoles();
                RenderMemory();
            }
        }

        private void Allocate_Click(object sender, RoutedEventArgs e)
        {
            uint size;
            if (!(ProcessName1.Text.Length > 0) || !uint.TryParse(ProcessSize.Text, out size))
            {
                MessageBox.Show("Please enter valid inputs.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var name = ProcessName1.Text;
            var method = Method.SelectedItem as ListBoxItem;
            if (method != null && method.Tag.ToString() == "FF")
            {
                AllocateFirstFit(name,size);
            }
            else if (method != null && method.Tag.ToString() == "BF")
            {
                AllocateBestFit(name, size);
            }
            else if (method != null && method.Tag.ToString() == "WF")
            {
                AllocateWorstFit(name, size);
            }
            else
            {
                MessageBox.Show("Please choose method.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UnitsSort()
        {
            var orderedEnumerable = _units.OrderBy(x => x.StartingAddress);
            _units = new List<MemoryUnit>(orderedEnumerable);
        }

        private MemoryUnit GetProcessByName(string name)
        {
            return _units.FirstOrDefault(unit => unit.Name == name && unit.Type == UnitType.Process);
        }
    }
}
