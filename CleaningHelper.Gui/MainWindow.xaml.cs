using System.Configuration;
using CleaningHelper.ViewModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CleaningHelper.Core;
using CleaningHelper.Tools;

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
            var consultationWindow = new ConsultationWindow(ViewModel.FrameModel);
            
            consultationWindow.ShowDialog();
        }

        private void ShowResultsWindow()
        {
            //var resultsWindow = new ResultsWindow(ViewModel.FrameModel);
            
            //resultsWindow.ShowDialog();
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

        private void EditDomainsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var domainsWindow = new DomainsWindow(ViewModel.TestFrameModel);
            domainsWindow.ShowDialog();
        }

        private void EditFramesButton_OnClick(object sender, RoutedEventArgs e)
        {
            var editorWindow = new FramesWindow(ViewModel.TestFrameModel);
            editorWindow.ShowDialog();
        }
    }
}