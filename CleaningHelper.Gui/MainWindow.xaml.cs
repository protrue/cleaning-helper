using System.Configuration;
using CleaningHelper.ViewModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CleaningHelper.Core;

namespace CleaningHelper.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainViewModel _viewModel;
        
        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();

            DataContext = _viewModel;
        }

        private void ShowConsultationWindow()
        {
            var consultationWindow = new ConsultationWindow(_viewModel.Model);
            
            consultationWindow.ShowDialog();
        }

        private void ShowResultsWindow()
        {
            var resultsWindow = new ResultsWindow();
            
            resultsWindow.ShowDialog();
        }

        private void StartConsultationButton_Click(object sender, RoutedEventArgs e)
        {
            ShowConsultationWindow();
        }

        private void ShowResultsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowResultsWindow();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadStateCommand.Execute();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _viewModel.SaveStateCommand.Execute();
        }
    }
}