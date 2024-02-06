using CommunityToolkit.Mvvm.ComponentModel;
using EmployeeManagementWPF.Core;
using EmployeeManagementWPF.Properties;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace EmployeeManagementWPF.MVVM.ViewModel
{
    class SettingsViewModel : ObservableObject
    {
        public RelayCommand LanguageSpanishCommand { get; set; }
        public RelayCommand LanguageEnglisCommand { get; set; }

        private string _currentLanguage;

        public string CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                _currentLanguage = value;
                OnPropertyChanged();
            }
        }

        public bool IsSpanish
        {
            get { return CurrentLanguage == "es"; }
        }

        public bool IsEnglish
        {
            get { return CurrentLanguage == "en"; }
        }

        public SettingsViewModel()
        {
            CurrentLanguage = Settings.Default.current_languaje ?? "en";

            LanguageSpanishCommand = new RelayCommand(o =>
            {
                ChangeLanguage("es");
            });

            LanguageEnglisCommand = new RelayCommand(o =>
            {
                ChangeLanguage("en");
            });
        }

        private void ChangeLanguage(string language)
        {
            if (CurrentLanguage == language) return;
            CurrentLanguage = language;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CurrentLanguage);
            Settings.Default.current_languaje = CurrentLanguage;
            Settings.Default.Save();

            RestartApplication();
        }

        private void RestartApplication()
        {
            Process.Start(Environment.ProcessPath);

            // Cerrar la instancia actual
            Application.Current.Shutdown();
        }
    }
}
