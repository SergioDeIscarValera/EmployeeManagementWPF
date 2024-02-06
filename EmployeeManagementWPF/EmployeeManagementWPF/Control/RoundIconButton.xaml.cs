using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeeManagementWPF.Control;

/// <summary>
/// Lógica de interacción para RoundIconButton.xaml
/// </summary>
public partial class RoundIconButton : UserControl
{
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(RoundIconButton));

    public static readonly DependencyProperty DisplayContentProperty =
        DependencyProperty.Register("DisplayContent", typeof(object), typeof(RoundIconButton));

    public static readonly DependencyProperty GroupNameProperty =
        DependencyProperty.Register("GroupName", typeof(string), typeof(RoundIconButton));

    public static readonly DependencyProperty IsCheckedProperty =
        DependencyProperty.Register("IsChecked", typeof(bool), typeof(RoundIconButton));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(string), typeof(RoundIconButton));

    public static readonly DependencyProperty ColorProperty = 
        DependencyProperty.Register("Color", typeof(Brush), typeof(RoundIconButton));

    public static readonly DependencyProperty PressColorProperty = 
        DependencyProperty.Register("PressColor", typeof(Brush), typeof(RoundIconButton));

    public object DisplayContent
    {
        get { return GetValue(DisplayContentProperty); }
        set { SetValue(DisplayContentProperty, value); }
    }

    public string GroupName
    {
        get { return (string)GetValue(GroupNameProperty); }
        set { SetValue(GroupNameProperty, value); }
    }

    public bool IsChecked
    {
        get { return (bool)GetValue(IsCheckedProperty); }
        set { SetValue(IsCheckedProperty, value); }
    }

    public string Icon
    {
        get { return (string)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    public Brush Color
    {
        get { return (Brush)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }

    public Brush PressColor
    {
        get { return (Brush)GetValue(PressColorProperty); }
        set { SetValue(PressColorProperty, value); }
    }

    public RoundIconButton()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        radioButton.IsChecked = true;
        if (Command != null && Command.CanExecute(null))
        {
            Command.Execute(null);
        }
    }

    private void RoundedIconButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            Command?.Execute(null);
            IsChecked = true;
        }
    }
}
