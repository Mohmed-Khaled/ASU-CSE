using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TinyScanner
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow
    {
        public ResultWindow(Scanner scanner)
        {
            InitializeComponent();

            
            // Create the Grid
            var dynamicGrid = new Grid();

            // Create Columns
            var gridCol1 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            var gridCol2 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            dynamicGrid.ColumnDefinitions.Add(gridCol1);
            dynamicGrid.ColumnDefinitions.Add(gridCol2);

            // Create header row 
            var gridRow1 = new RowDefinition { Height = GridLength.Auto};
            dynamicGrid.RowDefinitions.Add(gridRow1);

            // Add first column header
            var txtBlock1 = new TextBlock
            {
                Text = "Value",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Green),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(txtBlock1, 0);
            Grid.SetColumn(txtBlock1, 0);

            // Add second column header
            var txtBlock2 = new TextBlock
            {
                Text = "Type",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Green),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetRow(txtBlock2, 0);
            Grid.SetColumn(txtBlock2, 1);

            // Add column headers to the Grid
            dynamicGrid.Children.Add(txtBlock1);
            dynamicGrid.Children.Add(txtBlock2);

            // Create rows for tokens
            var token = scanner.GetNextToken();
            var i = 1;
            while (token != null)
            {
                var gridRow = new RowDefinition { Height = GridLength.Auto };
                dynamicGrid.RowDefinitions.Add(gridRow);
                var value = new Label
                {
                    Content = token.Value,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                var type = new Label
                {
                    Content = token.Type,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Grid.SetRow(value, i);
                Grid.SetRow(type, i);
                Grid.SetColumn(value, 0);
                Grid.SetColumn(type, 1);
                dynamicGrid.Children.Add(value);
                dynamicGrid.Children.Add(type);
                token = scanner.GetNextToken();
                i++;
            }

            // Display grid into a Window
            Viewer.Content = dynamicGrid;
        }
    }
}