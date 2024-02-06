using CommunityToolkit.Mvvm.ComponentModel;
using EmployeeManagementWPF.MVVM.Model;
using EmployeeManagementWPF.Service.Repository.DepartmentRepo;
using EmployeeManagementWPF.Service.Repository.EmployeeRepo;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace EmployeeManagementWPF.MVVM.ViewModel;

partial class HomeViewModel : ObservableObject
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmployeeRepository _employeeRepository;

    [ObservableProperty]
    private ObservableCollection<Department> _departments;
    [ObservableProperty]
    private ObservableCollection<Employee> _employees;

    [ObservableProperty]
    public ObservableCollection<ISeries> _pieSeries;

    [ObservableProperty]
    public ObservableCollection<ISeries> _barSeries;
    public Axis[] XAxes { get; private set; } =
    {
        new Axis
        {
            Labels = Enum.GetValues(typeof(EmployeePosition)).Cast<EmployeePosition>().Select(p => p.ToString()).ToArray(),
            LabelsRotation = 0,
            SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
            SeparatorsAtCenter = false,
            TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
            TicksAtCenter = true,
            ForceStepToMin = true,
            MinStep = 1
        }
    };


    public HomeViewModel()
    {
        _departmentRepository = App.Current.Services.GetService<IDepartmentRepository>();
        _employeeRepository = App.Current.Services.GetService<IEmployeeRepository>();

        SetUpLists();
    }

    private async void SetUpLists()
    {
        _departments = new ObservableCollection<Department>(await _departmentRepository.FindAll("idc"));
        _employees = new ObservableCollection<Employee>(await _employeeRepository.FindAll("idc"));
        _departmentRepository.AddChangeListener("idc", (deparments) =>
        {
            _departments.Clear();
            foreach (var department in deparments)
            {
                _departments.Add(department);
            }
            SetUpPieSeries();
            SetUpBarSeries();
        });
        _employeeRepository.AddChangeListener("idc", (employees) =>
        {
            _employees.Clear();
            foreach (var employee in employees)
            {
                _employees.Add(employee);
            }
            SetUpPieSeries();
            SetUpBarSeries();
        });

        PieSeries = new ObservableCollection<ISeries>();
        BarSeries = new ObservableCollection<ISeries>();
        SetUpPieSeries();
        SetUpBarSeries();
    }

    private void SetUpBarSeries()
    {
        BarSeries.Clear();
        // Type of position employee per department
        var values = Enum.GetValues(typeof(EmployeePosition));
        var valuesString = values.Cast<EmployeePosition>().Select(p => p.ToString()).ToArray();
        var valuesCount = new double[values.Length];
        foreach (var department in _departments)
        {
            foreach (var employee in _employees.Where(e => e.IdDepartment == department.Id))
            {
                //EmployeePosition is a string
                var index = Array.IndexOf(valuesString, employee.Position);
                valuesCount[index]++;
            }
            BarSeries.Add(new ColumnSeries<double> { Values = valuesCount, Name = department.Name });
            valuesCount = new double[values.Length];
        }
    }

    private void SetUpPieSeries()
    {
        PieSeries.Clear();
        // Employees per department
        foreach (var department in _departments)
        {
            PieSeries.Add(new PieSeries<double> { Values = new double[] { _employees.Where(e => e.IdDepartment == department.Id).Count() }, Name = department.Name });
        }
        System.Console.WriteLine("PieSeries: " + PieSeries.Count);
    }
}
