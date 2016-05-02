using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MemoryManager
{
    /// <summary>
    /// Interaction logic for HolesInfo.xaml
    /// </summary>
    public partial class HolesInfo
    {
        private readonly uint _numberOfHoles;
        private readonly uint _memorySize;

        public HolesInfo(uint numberOfHoles,uint memorySize)
        {
            InitializeComponent();
            _numberOfHoles = numberOfHoles;
            _memorySize = memorySize;
            RenderGrid();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var units = new List<MemoryUnit>();
            for (var i = 1; i <= _numberOfHoles; i++)
            {
                var name = "Hole " + i;
                uint startAddress = 0, size = 0;
                const UnitType type = UnitType.Hole;
                foreach (var element in Grid.Children.OfType<TextBox>())
                {
                    if (element.Name == "SA" + (i))
                    {
                        if (uint.TryParse(element.Text, out startAddress)) continue;
                        MessageBox.Show("Please enter valid inputs.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (element.Name != "S" + (i)) continue;
                    if (!uint.TryParse(element.Text, out size))
                    {
                        MessageBox.Show("Please enter valid inputs.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    break;
                }
                var tmp = new MemoryUnit(name, startAddress, size, type);
                if (tmp.StartingAddress + tmp.Size > _memorySize
                       || tmp.StartingAddress + tmp.Size == 0)
                {
                    MessageBox.Show("Please enter valid inputs.", "Error",
                           MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                units.Add(tmp);
            }
            var memoryState = new MemoryState(_memorySize,units);
            memoryState.Show();
            Close();
        }

        private void RenderGrid()
        {
            var gridCol1 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            var gridCol2 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            var gridCol3 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            Grid.ColumnDefinitions.Add(gridCol1);
            Grid.ColumnDefinitions.Add(gridCol2);
            Grid.ColumnDefinitions.Add(gridCol3);
            var gridRow1 = new RowDefinition { Height = GridLength.Auto };
            Grid.RowDefinitions.Add(gridRow1);
            var label1 = new Label
            {
                Content = "Hole Number",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var label2 = new Label
            {
                Content = "Starting Address (in Bytes)",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var label3 = new Label
            {
                Content = "Size (in Bytes)",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(label1, 0);
            Grid.SetRow(label2, 0);
            Grid.SetRow(label3, 0);
            Grid.SetColumn(label1, 0);
            Grid.SetColumn(label2, 1);
            Grid.SetColumn(label3, 2);
            Grid.Children.Add(label1);
            Grid.Children.Add(label2);
            Grid.Children.Add(label3);
            int i;
            for (i = 1; i <= _numberOfHoles; i++)
            {
                var gridRow = new RowDefinition { Height = GridLength.Auto };
                Grid.RowDefinitions.Add(gridRow);
                var label = new Label
                {
                    Content = i,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                var txtBox1 = new TextBox
                {
                    Name = "SA" + (i),
                    Margin = new Thickness(10)
                };
                var txtBox2 = new TextBox
                {
                    Name = "S" + (i),
                    Margin = new Thickness(10)
                };
                Grid.SetRow(label, i);
                Grid.SetRow(txtBox1, i);
                Grid.SetRow(txtBox2, i);
                Grid.SetColumn(label, 0);
                Grid.SetColumn(txtBox1, 1);
                Grid.SetColumn(txtBox2, 2);
                Grid.Children.Add(label);
                Grid.Children.Add(txtBox1);
                Grid.Children.Add(txtBox2);
            }
            var gridRow2 = new RowDefinition { Height = GridLength.Auto };
            Grid.RowDefinitions.Add(gridRow2);
            var button1 = new Button
            {
                Content = "Back",
                Margin = new Thickness(10)
            };
            var button2 = new Button
            {
                Content = "Start",
                Margin = new Thickness(10)
            };
            button1.Click += Back_Click;
            button2.Click += Start_Click;
            Grid.SetRow(button1, i);
            Grid.SetRow(button2, i);
            Grid.SetColumn(button1, 1);
            Grid.SetColumn(button2, 2);
            Grid.Children.Add(button1);
            Grid.Children.Add(button2);
        }
    }
}
