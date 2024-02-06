
using CommunityToolkit.Mvvm.ComponentModel;
using EmployeeManagementWPF.Core;
using EmployeeManagementWPF.MVVM.Model;
using EmployeeManagementWPF.Service.Navigation;
using EmployeeManagementWPF.Service.Repository.EmployeeRepo;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EmployeeManagementWPF.MVVM.ViewModel;

internal partial class EmployeeViewModel : ObservableObject
{
    private readonly IEmployeeRepository _employeetRepository;
    private string _departmentId;

    [ObservableProperty]
    private ObservableCollection<Employee> _employees;

    [ObservableProperty]
    private ObservableCollection<string> _positions;

    public RelayCommand DepartmentsNavigateCommand { get; set; }
    public RelayCommand DeleteEmployeeCommand { get; set; }
    public RelayCommand SaveEmployeeCommand { get; set; }
    public RelayCommand CancelEmployeeCommand { get; set; }

    private string _employeeUrl;
    private string _employeeName;
    private string _employeeDni;
    private string _employeePosition;
    public string EmployeeUrl
    {
        get { return _employeeUrl; }
        set
        {
            if (_employeeUrl != value)
            {
                _employeeUrl = value;
                OnPropertyChanged(nameof(EmployeeUrl));
            }
        }
    }
    public string EmployeeName
    {
        get { return _employeeName; }
        set
        {
            if (_employeeName != value)
            {
                _employeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
            }
        }
    }
    public string EmployeeDni
    {
        get { return _employeeDni; }
        set
        {
            if (_employeeDni != value)
            {
                _employeeDni = value;
                OnPropertyChanged(nameof(EmployeeDni));
            }
        }
    }
    public string EmployeePosition
    {
        get { return _employeePosition; }
        set
        {
            if (_employeePosition != value)
            {
                _employeePosition = value;
                OnPropertyChanged(nameof(EmployeePosition));
            }
        }
    }

    private Employee? _selectedEmployee;
    public Employee? SelectedEmployee
    {
        get { return _selectedEmployee; }
        set
        {
            if (_selectedEmployee != value)
            {
                _selectedEmployee = value;
                if (_selectedEmployee != null)
                {
                    InitForm(_selectedEmployee);
                }
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }
    }

    private void InitForm(Employee selectedEmployee)
    {
        EmployeeDni = selectedEmployee.Dni;
        EmployeeName = selectedEmployee.Name;
        EmployeePosition = selectedEmployee.Position;
        EmployeeUrl = selectedEmployee.UrlImage;
    }

    public EmployeeViewModel()
    {
        _departmentId = NavigationManagement.Instance.DepartmentId ?? "";
        Employees = new ObservableCollection<Employee>();
        Positions = new ObservableCollection<string>();
        _employeetRepository = App.Current.Services.GetService<IEmployeeRepository>();

        DepartmentsNavigateCommand = new RelayCommand(BackToDepartments);
        CancelEmployeeCommand = new RelayCommand(ClearForm, CanCancelEmployeeCommand);
        SaveEmployeeCommand = new RelayCommand(SaveEmployeeHandler, CanSaveEmployee);
        DeleteEmployeeCommand = new RelayCommand(DeleteEmployeeHandler, CanDeleteEmployee);

        SetUpList();
        ClearForm();
    }

    private bool CanCancelEmployeeCommand(object? arg) => _selectedEmployee != null;

    private bool CanDeleteEmployee(object? arg) => _selectedEmployee != null;

    private void DeleteEmployeeHandler(object? obj)
    {
        if (_selectedEmployee == null) return;
        _employeetRepository.DeleteById(_selectedEmployee.Id, "idc");
        ClearForm();
    }

    private bool CanSaveEmployee(object? arg)
        => _employeeDni.Trim().Length > 0 && _employeeName.Trim().Length > 0 && _employeeUrl.Trim().Length > 0;

    private void SaveEmployeeHandler(object? obj)
    {
        Employee employee;
        if (_selectedEmployee != null)
        {
            employee = new Employee(
                id: _selectedEmployee.Id,
                dni: _employeeDni,
                name: _employeeName,
                urlImage: _employeeUrl,
                position: _employeePosition,
                idDepartment: _departmentId
            );
        }
        else
        {
            employee = new Employee(
                dni: _employeeDni,
                name: _employeeName,
                urlImage: _employeeUrl,
                position: _employeePosition,
                idDepartment: _departmentId
            );
        }
        _employeetRepository.Save(employee, "idc", employee.Id);
        ClearForm();
    }

    private void ClearForm(object? obj = null)
    {
        SelectedEmployee = null;

        EmployeeUrl = "https://static.vecteezy.com/system/resources/thumbnails/007/126/739/small/question-mark-icon-free-vector.jpg";
        EmployeeName = "";
        EmployeeDni = "";
        EmployeePosition = Positions[0];
    }

    private async void SetUpList()
    {
        // Positions
        var values = Enum.GetValues(typeof(EmployeePosition));
        foreach (var item in values)
        {
            Positions.Add(item.ToString());
        }
        // Employees
        OnChangeData(await _employeetRepository.FindAllByDepartment(_departmentId, "idc"));

        _employeetRepository.AddChangeListener("idc", OnChangeData);
    }

    private void OnChangeData(IEnumerable<Employee> enumerable)
    {
        Employees.Clear();
        foreach (var item in enumerable)
        {
            if (item.IdDepartment != _departmentId) continue;
            Employees.Add(item);
        }
    }

    private void BackToDepartments(object? obj)
    {
        Console.WriteLine("Back to departments");
        var viewModel = App.Current.Services.GetService<DepartmentViewModel>();
        NavigationManagement.Instance.OnChangeViewRequested(viewModel);
    }
}
