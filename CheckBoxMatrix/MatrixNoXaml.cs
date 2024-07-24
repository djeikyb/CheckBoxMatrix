using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Reactive;
using R3;

namespace CheckBoxMatrix;

public class MatrixNoXaml : UserControl
{
    private IDisposable _disposable = Disposable.Empty;
    public MatrixNoXaml()
    {
        MatrixProperty.Changed.AddClassHandler<MatrixNoXaml, Matrix?>((sender, args) =>
        {
            _disposable.Dispose();
            var m = args.NewValue.Value;
            if (m is null) return;

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

                _disposable = Disposable.Combine(_disposable, box.GetObservable(ToggleButton.IsCheckedProperty).Subscribe(new AnonymousObserver<bool?>(b =>
                {
                    tile.IsChecked = b ?? false;
                })));

                var obv = tile.ObservePropertyChanged(t => t.IsChecked).Select(b => (bool?)b).AsSystemObservable();
                _disposable = Disposable.Combine(_disposable, box.Bind(ToggleButton.IsCheckedProperty, obv));

                box.Bind(Button.CommandProperty, new Binding { Source = tile, Path = nameof(tile.Tap) });
                Grid.SetColumn(box, tile.X + 1); // +1 to leave room for label
                Grid.SetRow(box, tile.Y + 1); // +1 to leave room for label
                grid.Children.Add(box);
            }

            sender.Content = grid;
        });
    }


    public static readonly StyledProperty<bool> ShowGridLinesProperty
        = AvaloniaProperty.Register<MatrixNoXaml, bool>(nameof(ShowGridLines));

    public bool ShowGridLines
    {
        get => GetValue(ShowGridLinesProperty);
        set => SetValue(ShowGridLinesProperty, value);
    }


    public static readonly StyledProperty<Matrix?> MatrixProperty
        = AvaloniaProperty.Register<MatrixNoXaml, Matrix?>(nameof(Matrix));

    public Matrix? Matrix
    {
        get => GetValue(MatrixProperty);
        set => SetValue(MatrixProperty, value);
    }
}
