using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables;

namespace CheckBoxMatrix;

public delegate void MatrixChangedEventHandler(object? sender, MatrixChangedEventArgs e);

public class MatrixChangedEventArgs;

public class Matrix
{
    private readonly List<Tile> _tiles = new();

    public Matrix(List<string> xAxisLabels, List<string> yAxisLabels)
    {
        XAxisLabels = xAxisLabels;
        YAxisLabels = yAxisLabels;

        for (var x = 0; x < xAxisLabels.Count; x++)
        {
            for (var y = 0; y < yAxisLabels.Count; y++)
            {
                var tile = new Tile(this)
                {
                    X = x,
                    Y = y,
                    LabelX = xAxisLabels[x],
                    LabelY = yAxisLabels[y],
                };
                _tiles.Add(tile);
            }
        }
    }

    public event MatrixChangedEventHandler? MatrixChanged;

    protected virtual void OnMatrixChanged(MatrixChangedEventArgs e)
    {
        MatrixChanged?.Invoke(this, e);
    }

    public IReadOnlyList<Tile> Tiles => _tiles;

    public List<string> XAxisLabels { get; }
    public List<string> YAxisLabels { get; }

    public string Ascii()
    {
        var ct = new ConsoleTable(new ConsoleTableOptions { Columns = ["x", ..XAxisLabels], EnableCount = true, });
        foreach (var labelY in YAxisLabels)
        {
            var l = new List<string>();
            l.Add(labelY);
            foreach (var tile in _tiles.Where(t => labelY.Equals(t.LabelY)))
            {
                l.Add(tile.IsChecked ? "X" : "-");
            }

            ct.AddRow([..l]);
        }

        return ct.ToMinimalString();
    }

    public void Tap(Tile tile)
    {
        var col = Tiles.Where(t => t.X == tile.X);
        foreach (var t in col)
        {
            if (t == tile) continue ;
            t.IsChecked = false;
        }
        var row = Tiles.Where(t => t.Y == tile.Y);
        foreach (var t in row)
        {
            if (t == tile) continue;
            t.IsChecked = false;
        }

        OnMatrixChanged(new MatrixChangedEventArgs());
    }

    public void Tap(string x, string y)
    {
        var found = Tiles.FirstOrDefault(tile => tile.LabelX.Equals(x) && tile.LabelY.Equals(y));
        if (found is null) throw new Exception($"Either label x '{x}' or label y '{y}' is not in matrix.");
        found.IsChecked = !found.IsChecked;
        Tap(found);
    }
}
