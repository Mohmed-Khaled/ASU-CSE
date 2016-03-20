using System.Windows;
using System.Windows.Controls;

namespace OSScheduler
{
    /// <summary>
    /// Interaction logic for DataEntry1.xaml
    /// </summary>
    public partial class DataEntry
    {
        private readonly int _numberOfProcesses;
        private readonly string _alg;

        public DataEntry(int numberOfProcesses,string alg)
        {
            _numberOfProcesses = numberOfProcesses;
            _alg = alg;
            InitializeComponent();
            switch (_alg)
            {
                case "RR":
                    RenderGrid3();
                    break;
                case "PNP":
                case "PP":
                    RenderGrid2();
                    break;
                default:
                    RenderGrid1();
                    break;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();

        }

        private static void Simulate_Click(object sender, RoutedEventArgs e)
        {
            var result = new Result();
            result.Show();
        }

        private void RenderGrid1()
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
                Content = "Processes",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var label2 = new Label
            {
                Content = "Burst Time",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var label3 = new Label
            {
                Content = "Arrival Time",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(label1,0);
            Grid.SetRow(label2,0);
            Grid.SetRow(label3,0);
            Grid.SetColumn(label1,0);
            Grid.SetColumn(label2,1);
            Grid.SetColumn(label3,2);
            Grid.Children.Add(label1);
            Grid.Children.Add(label2);
            Grid.Children.Add(label3);
            int i;
            for (i = 1; i <= _numberOfProcesses; i++)
            {
                var gridRow = new RowDefinition { Height = GridLength.Auto };
                Grid.RowDefinitions.Add(gridRow);
                var label = new Label
                {
                    Content = "P[" + (i) + "]",
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                var txtBox1 = new TextBox
                {
                    Name = "Bt" + (i),
                    Margin = new Thickness(10)
                };
                var txtBox2 = new TextBox
                {
                    Name = "At" + (i),
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
                Content = "Simulate",
                Margin = new Thickness(10)
            };
            button1.Click += Back_Click;
            button2.Click += Simulate_Click;
            Grid.SetRow(button1, i);
            Grid.SetRow(button2, i);
            Grid.SetColumn(button1, 1);
            Grid.SetColumn(button2, 2);
            Grid.Children.Add(button1);
            Grid.Children.Add(button2);
        }

        private void RenderGrid2()
        {
            var gridCol1 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            var gridCol2 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            var gridCol3 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            var gridCol4 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            Grid.ColumnDefinitions.Add(gridCol1);
            Grid.ColumnDefinitions.Add(gridCol2);
            Grid.ColumnDefinitions.Add(gridCol3);
            Grid.ColumnDefinitions.Add(gridCol4);
            var gridRow1 = new RowDefinition { Height = GridLength.Auto };
            Grid.RowDefinitions.Add(gridRow1);
            var label1 = new Label
            {
                Content = "Processes",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var label2 = new Label
            {
                Content = "Burst Time",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var label3 = new Label
            {
                Content = "Arrival Time",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var label4 = new Label
            {
                Content = "Priority",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(label1, 0);
            Grid.SetRow(label2, 0);
            Grid.SetRow(label3, 0);
            Grid.SetRow(label4, 0);
            Grid.SetColumn(label1, 0);
            Grid.SetColumn(label2, 1);
            Grid.SetColumn(label3, 2);
            Grid.SetColumn(label4, 3);
            Grid.Children.Add(label1);
            Grid.Children.Add(label2);
            Grid.Children.Add(label3);
            Grid.Children.Add(label4);
            int i;
            for (i = 1; i <= _numberOfProcesses; i++)
            {
                var gridRow = new RowDefinition { Height = GridLength.Auto };
                Grid.RowDefinitions.Add(gridRow);
                var label = new Label
                {
                    Content = "P[" + (i) + "]",
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                var txtBox1 = new TextBox
                {
                    Name = "Bt" + (i),
                    Margin = new Thickness(10)
                };
                var txtBox2 = new TextBox
                {
                    Name = "At" + (i),
                    Margin = new Thickness(10)
                };
                var txtBox3 = new TextBox
                {
                    Name = "Pt" + (i),
                    Margin = new Thickness(10)
                };
                Grid.SetRow(label, i);
                Grid.SetRow(txtBox1, i);
                Grid.SetRow(txtBox2, i);
                Grid.SetRow(txtBox3, i);
                Grid.SetColumn(label, 0);
                Grid.SetColumn(txtBox1, 1);
                Grid.SetColumn(txtBox2, 2);
                Grid.SetColumn(txtBox3, 3);
                Grid.Children.Add(label);
                Grid.Children.Add(txtBox1);
                Grid.Children.Add(txtBox2);
                Grid.Children.Add(txtBox3);
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
                Content = "Simulate",
                Margin = new Thickness(10)
            };
            button1.Click += Back_Click;
            button2.Click += Simulate_Click;
            Grid.SetRow(button1, i);
            Grid.SetRow(button2, i);
            Grid.SetColumn(button1, 2);
            Grid.SetColumn(button2, 3);
            Grid.Children.Add(button1);
            Grid.Children.Add(button2);
        }

        private void RenderGrid3()
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
                Content = "Enter quantum value: ",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var txtBox = new TextBox
            {
                Name = "quantum",
                Margin = new Thickness(10)
            };
            Grid.SetRow(label1, 0);
            Grid.SetRow(txtBox, 0);
            Grid.SetColumn(label1, 0);
            Grid.SetColumn(txtBox, 1);
            Grid.Children.Add(label1);
            Grid.Children.Add(txtBox);
            var gridRow2 = new RowDefinition { Height = GridLength.Auto };
            Grid.RowDefinitions.Add(gridRow2);
            var label2 = new Label
            {
                Content = "Processes",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var label3 = new Label
            {
                Content = "Burst Time",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var label4 = new Label
            {
                Content = "Arrival Time",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(label2, 1);
            Grid.SetRow(label3, 1);
            Grid.SetRow(label4, 1);
            Grid.SetColumn(label2, 0);
            Grid.SetColumn(label3, 1);
            Grid.SetColumn(label4, 2);
            Grid.Children.Add(label2);
            Grid.Children.Add(label3);
            Grid.Children.Add(label4);
            int i;
            for (i = 1; i <= _numberOfProcesses; i++)
            {
                var gridRow = new RowDefinition { Height = GridLength.Auto };
                Grid.RowDefinitions.Add(gridRow);
                var label = new Label
                {
                    Content = "P[" + (i) + "]",
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                var txtBox1 = new TextBox
                {
                    Name = "Bt" + (i),
                    Margin = new Thickness(10)
                };
                var txtBox2 = new TextBox
                {
                    Name = "At" + (i),
                    Margin = new Thickness(10)
                };
                Grid.SetRow(label, i+1);
                Grid.SetRow(txtBox1, i+1);
                Grid.SetRow(txtBox2, i+1);
                Grid.SetColumn(label, 0);
                Grid.SetColumn(txtBox1, 1);
                Grid.SetColumn(txtBox2, 2);
                Grid.Children.Add(label);
                Grid.Children.Add(txtBox1);
                Grid.Children.Add(txtBox2);
            }
            var gridRow3 = new RowDefinition { Height = GridLength.Auto };
            Grid.RowDefinitions.Add(gridRow3);
            var button1 = new Button
            {
                Content = "Back",
                Margin = new Thickness(10)
            };
            var button2 = new Button
            {
                Content = "Simulate",
                Margin = new Thickness(10)
            };
            button1.Click += Back_Click;
            button2.Click += Simulate_Click;
            Grid.SetRow(button1, i+1);
            Grid.SetRow(button2, i+1);
            Grid.SetColumn(button1, 1);
            Grid.SetColumn(button2, 2);
            Grid.Children.Add(button1);
            Grid.Children.Add(button2);
        }
    }
}
