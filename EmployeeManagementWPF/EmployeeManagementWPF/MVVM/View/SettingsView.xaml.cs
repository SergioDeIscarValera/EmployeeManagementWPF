using EmployeeManagementWPF.MVVM.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace EmployeeManagementWPF.MVVM.View
{
    /// <summary>
    /// Lógica de interacción para SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService<SettingsViewModel>();
        }
    }
}
