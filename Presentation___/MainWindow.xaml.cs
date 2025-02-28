using Presentation.ViewModels;
using System.Windows;
using System.Windows.Input;
namespace Presentation;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    //private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
    //{
    //    if (e.LeftButton == MouseButtonState.Pressed)
    //    {
    //        DragMove();
    //    }
    //}

    //private void CloseButton_Click(object sender, RoutedEventArgs e)
    //{
    //    Environment.Exit(0);
    //}

    //private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    //{
    //    WindowState = WindowState.Minimized;
    //}

    //private void FullScreenToggleButton_Click(object sender, RoutedEventArgs e)
    //{
    //    if (WindowState == WindowState.Maximized)
    //    {
    //        WindowState = WindowState.Normal;
    //    }
    //    else
    //    {
    //        WindowState = WindowState.Maximized;
    //    }
    //}
}