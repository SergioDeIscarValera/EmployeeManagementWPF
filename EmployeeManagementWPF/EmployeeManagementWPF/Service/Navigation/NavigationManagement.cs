using System;

namespace EmployeeManagementWPF.Service.Navigation;

internal class NavigationManagement
{
    public static NavigationManagement Instance { get; } = new NavigationManagement();

    private NavigationManagement() { }

    public event Action<object> ChangeViewRequested;

    public string? DepartmentId { get; set; }

    public void OnChangeViewRequested(object viewModel)
    {
        ChangeViewRequested?.Invoke(viewModel);
    }
}
