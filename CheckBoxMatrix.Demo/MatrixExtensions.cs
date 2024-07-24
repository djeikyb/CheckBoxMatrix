using System.Collections.Generic;
using System.Linq;
using CheckBoxMatrix.Model;
using ConsoleTables;

namespace CheckBoxMatrix.Demo;

public static class MatrixExtensions
{
    public static string Ascii(this Matrix matrix)
    {
        var ct = new ConsoleTable(
            new ConsoleTableOptions { Columns = ["x", ..matrix.XAxisLabels], EnableCount = true, });
        for (var yLabelIndex = 0; yLabelIndex < matrix.YAxisLabels.Count; yLabelIndex++)
        {
            var labelY = matrix.YAxisLabels[yLabelIndex];
            var l = new List<string> { labelY };
            l.AddRange(matrix.Tiles.Where(t => yLabelIndex.Equals(t.Y)).Select(tile => tile.IsChecked ? "X" : "-"));
            ct.AddRow([..l]);
        }

        return ct.ToMinimalString();
    }
}
