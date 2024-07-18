using FluentAssertions;
using Xunit.Abstractions;

namespace CheckBoxMatrix.Test;

public class MatrixTests(ITestOutputHelper testOutputHelper) : IDisposable
{
    private Matrix? _matrix;

    [Fact]
    public void StartEmptyAndPickOne()
    {
        _matrix = new Matrix(xAxisLabels: ["1", "2", "3"], yAxisLabels: ["L", "R", "C"]);
        PrintBefore();
        _matrix.Tap("2", "C");
        PrintAfter();
    }

    [Fact]
    public void TryToDupeWithinColumn3x3()
    {
        _matrix = new Matrix(xAxisLabels: ["1", "2", "3"], yAxisLabels: ["L", "R", "C"]);
        _matrix.Tap("2", "C");
        PrintBefore();
        _matrix.Tap("2", "L");
        PrintAfter();
    }

    [Fact]
    public void TryToDupeWithinRow3x3()
    {
        _matrix = new Matrix(xAxisLabels: ["1", "2", "3"], yAxisLabels: ["L", "R", "C"]);
        _matrix.Tap("2", "C");
        PrintBefore();
        _matrix.Tap("1", "C");
        PrintAfter();
    }

    [Fact]
    public void TryToDupeWithinColumn4x4()
    {
        _matrix = new Matrix(xAxisLabels: ["1", "2", "3", "4"], yAxisLabels: ["L", "R", "C", "LFE"]);
        _matrix.Tap("2", "C");
        PrintBefore();
        _matrix.Tap("2", "L");
        PrintAfter();
    }

    [Fact]
    public void TryToDupeWithinRow4x4()
    {
        _matrix = new Matrix(xAxisLabels: ["1", "2", "3", "4"], yAxisLabels: ["L", "R", "C", "LFE"]);
        _matrix.Tap("1","L");
        _matrix.Tap("2","R");
        _matrix.Tap("3","C");
        _matrix.Tap("4","LFE");
        PrintBefore();
        PrintAfter();
    }

    public void Dispose()
    {
        if (_matrix is null) return;
        foreach (var g in _matrix.Tiles.GroupBy(tile => tile.LabelY))
        {
            g.Where(tile => tile.IsChecked).Should().HaveCountLessOrEqualTo(1);
        }

        foreach (var g in _matrix.Tiles.GroupBy(tile => tile.LabelX))
        {
            g.Where(tile => tile.IsChecked).Should().HaveCountLessOrEqualTo(1);
        }
    }

    private void PrintBefore() => testOutputHelper.WriteLine($"Before:\n{_matrix?.Ascii()}");
    private void PrintAfter() => testOutputHelper.WriteLine($"After:\n{_matrix?.Ascii()}");
}
