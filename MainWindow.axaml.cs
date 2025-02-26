using Avalonia.Controls;

namespace HW1;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var image = FileManager.ReadB2img();
        FileManager.WriteB2img(image);
    }
}