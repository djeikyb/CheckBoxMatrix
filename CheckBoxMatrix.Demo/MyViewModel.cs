using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CheckBoxMatrix.Demo;

public class MyViewModel : INotifyPropertyChanged
{
    private Matrix _matrix;
    private bool _showGridLines = true;

    public MyViewModel()
    {
        _matrix = new Matrix(xAxisLabels: ["1", "2", "3", "4"], yAxisLabels: ["L", "R", "C", "LFE"]);
        _matrix.MatrixChanged += (_, _) =>
        {
            Console.WriteLine(_matrix.Ascii());
        };

        this.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName is nameof(ShowGridLines))
            {
                Console.WriteLine($"grids? {ShowGridLines}");
            }
        };
    }

    public bool ShowGridLines
    {
        get => _showGridLines;
        set => SetField(ref _showGridLines, value);
    }

    public Matrix Matrix
    {
        get => _matrix;
        set => SetField(ref _matrix, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
