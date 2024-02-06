using CommunityToolkit.Mvvm.ComponentModel;
using EmployeeManagementWPF.Core;
using EmployeeManagementWPF.MVVM.Model;
using EmployeeManagementWPF.Service.Navigation;
using EmployeeManagementWPF.Service.Repository.DepartmentRepo;
using EmployeeManagementWPF.Service.Repository.EmployeeRepo;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace EmployeeManagementWPF.MVVM.ViewModel;

internal partial class DepartmentViewModel : ObservableObject
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmployeeRepository _employeeRepository;

    [ObservableProperty]
    private ObservableCollection<Department> _departments;

    public RelayCommand SaveDepartmentCommand { get; set; }
    public RelayCommand DeleteDepartmentCommand { get; set; }
    public RelayCommand CancelDepartmentCommand { get; set; }
    public RelayCommand EmployeeNavigateCommand { get; set; }


    private string _departmentUrl;
    private string _departmentName;
    private string _departmentDescription;
    public string DepartmentUrl
    {
        get { return _departmentUrl; }
        set
        {
            if (_departmentUrl != value)
            {
                _departmentUrl = value;
                OnPropertyChanged(nameof(DepartmentUrl));
            }
        }
    }
    public string DepartmentName
    {
        get { return _departmentName; }
        set
        {
            if (_departmentName != value)
            {
                _departmentName = value;
                OnPropertyChanged(nameof(DepartmentName));
            }
        }
    }
    public string DepartmentDescription
    {
        get { return _departmentDescription; }
        set
        {
            if (_departmentDescription != value)
            {
                _departmentDescription = value;
                OnPropertyChanged(nameof(DepartmentDescription));
            }
        }
    }

    private Department? _selectedDepartment;
    public Department? SelectedDepartment
    {
        get { return _selectedDepartment; }
        set
        {
            if (_selectedDepartment != value)
            {
                _selectedDepartment = value;
                if (_selectedDepartment != null)
                {
                    InitForm(_selectedDepartment);
                }
                OnPropertyChanged(nameof(SelectedDepartment));
            }
        }
    }


    public DepartmentViewModel()
    {
        ClearForm();
        _departmentRepository = App.Current.Services.GetService<IDepartmentRepository>();
        _employeeRepository = App.Current.Services.GetService<IEmployeeRepository>();

        Departments = new ObservableCollection<Department>();
        SetUpList();

        SaveDepartmentCommand = new RelayCommand(SaveDepartmentHandler, CanSaveDepartment);
        DeleteDepartmentCommand = new RelayCommand(DeleteDepartmentHandler, CanDeleteDepartment);
        CancelDepartmentCommand = new RelayCommand(ClearForm);
        EmployeeNavigateCommand = new RelayCommand(EmployeeNavigateHandler, CanEmployeeNavigate);
    }

    private void EmployeeNavigateHandler(object? obj)
    {
        if (_selectedDepartment == null)
        {
            MessageBox.Show("No se ha seleccionado un departamento", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        NavigationManagement.Instance.DepartmentId = _selectedDepartment.Id;
        var viewModel = App.Current.Services.GetService<EmployeeViewModel>();
        NavigationManagement.Instance.OnChangeViewRequested(viewModel);
    }

    private bool CanEmployeeNavigate(object? arg) =>
        _selectedDepartment != null;

    private void DeleteDepartmentHandler(object? obj)
    {
        if (_selectedDepartment == null)
        {
            MessageBox.Show("No se ha seleccionado un departamento", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        // Borrado en cascada
        _employeeRepository.DeleteByDepartment(_selectedDepartment.Id, "idc");
        _departmentRepository.DeleteById(_selectedDepartment.Id, "idc");
        ClearForm();
    }

    private bool CanDeleteDepartment(object? arg) =>
        _selectedDepartment != null;

    private void InitForm(Department department)
    {
        DepartmentName = department.Name;
        DepartmentUrl = department.UrlImage;
        DepartmentDescription = department.Description;
    }

    private void ClearForm(object? _ = null)
    {
        SelectedDepartment = null;
        DepartmentName = "";
        DepartmentUrl = "https://static.vecteezy.com/system/resources/thumbnails/007/126/739/small/question-mark-icon-free-vector.jpg";
        DepartmentDescription = "";
    }

    private bool CanSaveDepartment(object? _) =>
        _departmentName.Trim().Length > 0 && _departmentUrl.Trim().Length > 0 && _departmentDescription.Trim().Length > 0;

    private void SaveDepartmentHandler(object? _)
    {
        Department department;
        if (_selectedDepartment != null)
        {
            department = new Department(
                id: _selectedDepartment.Id,
                name: DepartmentName,
                urlImage: DepartmentUrl,
                description: DepartmentDescription
            );
        }
        else
        {
            department = new Department(
                name: DepartmentName,
                urlImage: DepartmentUrl,
                description: DepartmentDescription
            );
        }
        _departmentRepository.Save(department, "idc", department.Id);
        ClearForm();
    }

    private async void SetUpList()
    {
        OnChangeData(await _departmentRepository.FindAll("idc"));

        _departmentRepository.AddChangeListener("idc", OnChangeData);
    }

    private void OnChangeData(IEnumerable<Department> enumerable)
    {
        Departments.Clear();
        foreach (var item in enumerable)
        {
            Departments.Add(item);
        }
    }
}
