using EmployeeManagementWPF.MVVM.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace EmployeeManagementWPF.MVVM.View
{
    /// <summary>
    /// Lógica de interacción para DepartmentView.xaml
    /// </summary>
    public partial class DepartmentView : UserControl
    {
        public DepartmentView()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService<DepartmentViewModel>();
        }
    }
}
