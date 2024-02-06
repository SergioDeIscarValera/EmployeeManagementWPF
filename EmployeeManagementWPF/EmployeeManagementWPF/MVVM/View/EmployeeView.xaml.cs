using EmployeeManagementWPF.MVVM.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace EmployeeManagementWPF.MVVM.View
{
    /// <summary>
    /// Lógica de interacción para EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : UserControl
    {
        public EmployeeView()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService<EmployeeViewModel>();
        }
    }
}
