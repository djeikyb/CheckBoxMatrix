using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Matrix = CheckBoxMatrix.Model.Matrix;

namespace CheckBoxMatrix.Control;

public class CheckBoxMatrix : UserControl
{
    public CheckBoxMatrix()
    {
        _xAxisLabels = [];
        _yAxisLabels = [];

        XAxisLabelsProperty.Changed.AddClassHandler<CheckBoxMatrix, IEnumerable<string>>((_, _) =>
        {
            Matrix = new Matrix(_xAxisLabels.ToList(), _yAxisLabels.ToList());
        });

        YAxisLabelsProperty.Changed.AddClassHandler<CheckBoxMatrix, IEnumerable<string>>((_, _) =>
        {
            Matrix = new Matrix(_xAxisLabels.ToList(), _yAxisLabels.ToList());
        });

        MatrixProperty.Changed.AddClassHandler<CheckBoxMatrix, Matrix?>((sender, args) =>
        {
            var m = args.NewValue.Value;
            if (m is null) return;

            m.MatrixChanged += (m_sender, _) =>
            {
                var newmatrix = (Matrix?)m_sender;
                var set = new HashSet<(string x, string y)>();
                if (newmatrix is null)
                {
                    Mappings = set;
                    return;
                }

                var xlabels = XAxisLabels.ToArray();
                var ylabels = YAxisLabels.ToArray();
                foreach (var tile in newmatrix.Tiles)
                {
                    if (tile.IsChecked)
                    {
                        var x = xlabels[tile.X];
                        var y = ylabels[tile.Y];
                        set.Add((x: x, y: y));
                    }
                }

                Mappings = set;
            };

            MappingsProperty.Changed.AddClassHandler<CheckBoxMatrix, HashSet<(string, string)>>((_, args) =>
            {
                var sb = new StringBuilder();
                foreach (string s in args.NewValue.Value.Select(x => $"[{x.Item1},{x.Item2}]")) sb.Append(s);
                Console.WriteLine($"⚡️MatrixNoXaml.MappingsProperty: {sb}");
            });

            ShowGridLines = false; // why doesn't the grid redraw when this prop changes?
            var grid = new Grid();
            grid.Bind(Grid.ShowGridLinesProperty, new Binding { Source = this, Path = nameof(ShowGridLinesProperty) });
            grid.ShowGridLines = ShowGridLines; // why do i need this to get gridlines drawn?

            // make room for labels
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // make room for x axis boxes
            for (int i = 0; i < m.XAxisLabels.Count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // make room for y axis boxes
            for (int i = 0; i < m.YAxisLabels.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // add x axis labels
            for (var i = 0; i < m.XAxisLabels.Count; i++)
            {
                var label = m.XAxisLabels[i];
                var text = new TextBlock();
                text.Text = label;
                text.Classes.Add("XAxisLabel");
                Grid.SetRow(text, 0);
                Grid.SetColumn(text, i + 1); // +1 to skip top left corner
                grid.Children.Add(text);
            }

            // add y axis labels
            for (var i = 0; i < m.YAxisLabels.Count; i++)
            {
                var label = m.YAxisLabels[i];
                var text = new TextBlock();
                text.Text = label;
                text.Classes.Add("YAxisLabel");
                Grid.SetColumn(text, 0);
                Grid.SetRow(text, i + 1); // +1 to skip top left corner
                grid.Children.Add(text);
            }

            // add boxes
            foreach (var tile in m.Tiles)
            {
                var box = new CheckBox();
                box.Bind(ToggleButton.IsCheckedProperty, new Binding { Source = tile, Path = nameof(tile.IsChecked) });
                box.Bind(Button.CommandProperty, new Binding { Source = tile, Path = nameof(tile.Tap) });
                Grid.SetColumn(box, tile.X + 1); // +1 to leave room for label
                Grid.SetRow(box, tile.Y + 1); // +1 to leave room for label
                grid.Children.Add(box);
            }

            sender.Content = grid;
        });
    }


    public static readonly StyledProperty<bool> ShowGridLinesProperty
        = AvaloniaProperty.Register<CheckBoxMatrix, bool>(nameof(ShowGridLines));

    public bool ShowGridLines
    {
        get => GetValue(ShowGridLinesProperty);
        set => SetValue(ShowGridLinesProperty, value);
    }


    public static readonly StyledProperty<Matrix?> MatrixProperty
        = AvaloniaProperty.Register<CheckBoxMatrix, Matrix?>(nameof(Matrix));

    public Matrix? Matrix
    {
        get => GetValue(MatrixProperty);
        set => SetValue(MatrixProperty, value);
    }


    private IEnumerable<string> _xAxisLabels;

    public static readonly DirectProperty<CheckBoxMatrix, IEnumerable<string>> XAxisLabelsProperty =
        AvaloniaProperty.RegisterDirect<CheckBoxMatrix, IEnumerable<string>>(
            unsetValue: ["a,b,c"],
            name: "XAxisLabels", getter: o => o.XAxisLabels, setter: (o, v) => o.XAxisLabels = v);

    public IEnumerable<string> XAxisLabels
    {
        get => _xAxisLabels;
        set => SetAndRaise(XAxisLabelsProperty, ref _xAxisLabels, value);
    }


    private IEnumerable<string> _yAxisLabels;

    public static readonly DirectProperty<CheckBoxMatrix, IEnumerable<string>> YAxisLabelsProperty =
        AvaloniaProperty.RegisterDirect<CheckBoxMatrix, IEnumerable<string>>(
            unsetValue: ["1", "2", "3"],
            name: "YAxisLabels", getter: o => o.YAxisLabels, setter: (o, v) => o.YAxisLabels = v);

    public IEnumerable<string> YAxisLabels
    {
        get => _yAxisLabels;
        set => SetAndRaise(YAxisLabelsProperty, ref _yAxisLabels, value);
    }


    public static readonly StyledProperty<HashSet<(string x, string y)>> MappingsProperty =
        AvaloniaProperty.Register<CheckBoxMatrix, HashSet<(string x, string y)>>(
            nameof(Mappings), defaultValue: [], defaultBindingMode: BindingMode.OneWayToSource);

    public HashSet<(string x, string y)> Mappings
    {
        get => GetValue(MappingsProperty);
        set => SetValue(MappingsProperty, value);
    }
}
