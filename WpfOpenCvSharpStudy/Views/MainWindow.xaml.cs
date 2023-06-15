using System.Windows;

namespace WpfWpfOpenCvSharpStudy.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = new ViewModels.MainWindowViewModel();
        InitializeComponent();
    }
}
