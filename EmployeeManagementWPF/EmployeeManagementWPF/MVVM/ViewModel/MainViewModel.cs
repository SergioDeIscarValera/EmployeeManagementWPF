using CommunityToolkit.Mvvm.ComponentModel;
using EmployeeManagementWPF.Core;
using EmployeeManagementWPF.Properties;
using EmployeeManagementWPF.Service.Navigation;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EmployeeManagementWPF.MVVM.ViewModel;

class MainViewModel : ObservableObject
{
    public RelayCommand HomeViewCommand { get; set; }
    public RelayCommand SettingsViewCommand { get; set; }
    public RelayCommand DepartmentViewCommand { get; set; }

    public RelayCommand ExitAppCommand { get; set; }

    public HomeViewModel HomeVm { get; set; }
    public SettingsViewModel SettingsVM { get; set; }
    public DepartmentViewModel DepartmentVM { get; set; }
    public EmployeeViewModel EmployeeVM { get; set; }

    private object _currentView;

    public object CurrentView
    {
        get { return _currentView; }
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        HomeVm = App.Current.Services.GetService<HomeViewModel>();
        SettingsVM = App.Current.Services.GetService<SettingsViewModel>();
        DepartmentVM = App.Current.Services.GetService<DepartmentViewModel>();
        _currentView = HomeVm;

        HomeViewCommand = new RelayCommand(o =>
        {
            CurrentView = HomeVm;
        });

        SettingsViewCommand = new RelayCommand(o =>
        {
            CurrentView = SettingsVM;
        });

        DepartmentViewCommand = new RelayCommand(o =>
        {
            CurrentView = DepartmentVM;
        });

        ExitAppCommand = new RelayCommand(o =>
        {
            Settings.Default.Save();
            Environment.Exit(0);
        });

        NavigationManagement.Instance.ChangeViewRequested += OnChangeViewRequested;
    }

    private void OnChangeViewRequested(object viewModel)
    {
        if (viewModel is not HomeViewModel && viewModel is not SettingsViewModel && viewModel is not DepartmentViewModel && viewModel is not EmployeeViewModel)
            return;
        CurrentView = viewModel;
    }
}
