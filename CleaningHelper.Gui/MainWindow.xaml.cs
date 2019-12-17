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
        public MainViewModel ViewModel;
        
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainViewModel();
            DataContext = ViewModel;
        }

        private void ShowConsultationWindow()
        {
            var consultationWindow = new ConsultationWindow(ViewModel.Model);
            
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
            ViewModel.LoadStateCommand.Execute();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.SaveStateCommand.Execute();
        }

        private void EditModelButton_Click(object sender, RoutedEventArgs e)
        {
            var editorWindow = new EditorWindow();
            editorWindow.ShowDialog();
        }
    }
}