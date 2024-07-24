using Avalonia.Controls;

namespace CheckBoxMatrix.Demo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MyViewModel();
    }
}
