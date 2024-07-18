using Avalonia.Controls;
using Avalonia.Layout;

namespace CheckBoxMatrix;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MyViewModel();

        // var grid = new Grid();
        // grid.ShowGridLines = true;
        // grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        // grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //
        // var text1 = SomeLabel();
        // var text2 = SomeLabel();
        //
        // Grid.SetColumn(text1, 0);
        // Grid.SetColumn(text2, 1);
        //
        // grid.Children.Add(text1);
        // grid.Children.Add(text2);
        //
        //
        // this.Content = grid;
    }

    private static int _i = 0;

    private static Label SomeLabel()
    {
        var text = new Label();
        text.Content = $"halooooo {_i++}";
        text.HorizontalAlignment = HorizontalAlignment.Center;
        text.VerticalAlignment = VerticalAlignment.Center;
        text.FontSize = 72;
        return text;
    }
}
