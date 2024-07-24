using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CheckBoxMatrix.Model;

namespace CheckBoxMatrix.Demo;

public sealed class MyViewModel : INotifyPropertyChanged
{
    private Matrix? _matrix;
    private bool _showGridLines = true;
    private HashSet<(string x, string y)> _mappings = [];

    public MyViewModel()
    {
        XAxisLabels = ["1", "2", "3", "4"];
        YAxisLabels = ["L", "R", "C", "LFE"];

        this.PropertyChanged += (_, args) =>
        {
            Console.WriteLine($"⚡️ {nameof(MyViewModel)}.{args.PropertyName}");
            switch (args.PropertyName)
            {
                case nameof(ShowGridLines):
                    Console.WriteLine($"grids? {ShowGridLines}");
                    break;
                case nameof(Mappings):
                {
                    var items = Mappings.ToList();
                    if (items.Count == 0)
                    {
                        Console.WriteLine($"{Matrix?.Ascii()}❎ []");
                        return;
                    }

                    var sb = new StringBuilder();
                    sb.Append(Matrix?.Ascii());

                    sb.Append($"❎ [{items[0].x},{items[0].y}]");

                    for (int i = 1; i < items.Count; i++)
                    {
                        var tuple = items[i];
                        sb.Append($",[{tuple.x},{tuple.y}]");
                    }

                    Console.WriteLine(sb.ToString());
                    break;
                }
                default:
                    Console.WriteLine("🐙");
                    break;
            }
        };
    }

    public List<string> XAxisLabels { get; set; }
    public List<string> YAxisLabels { get; set; }

    public HashSet<(string x, string y)> Mappings
    {
        get => _mappings;
        set => SetField(ref _mappings, value);
    }

    public bool ShowGridLines
    {
        get => _showGridLines;
        set => SetField(ref _showGridLines, value);
    }

    public Matrix? Matrix
    {
        get => _matrix;
        set => SetField(ref _matrix, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
