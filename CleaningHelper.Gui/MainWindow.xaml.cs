using CleaningHelper.ViewModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace CleaningHelper.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ApplicationViewModel viewModel;

        private string applicationStatePath = "ApplicationState.json";

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new ApplicationViewModel();

            DataContext = viewModel;
        }
               
        private void SetPathToOntolisButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Ontolis executable file (ontolis.exe)|ontolis.exe";
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                viewModel.PathToOntolis = openFileDialog.FileName;
            }
        }

        private void LoadModelButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Ontolis ontology file (*.ont)|*.ont";
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                viewModel.PathToModel = openFileDialog.FileName;
            }
        }

        private void EditModelButton_Click(object sender, RoutedEventArgs e)
        {
            var ontolisRunner = new OntolisAdapter.Tools.OntolisRunner(viewModel.PathToOntolis);
            ontolisRunner.RunOntolis();
        }

        private void StartConsultationButton_Click(object sender, RoutedEventArgs e)
        {
            var consultationWindow = new ConsultationWindow();
            consultationWindow.ViewModel = viewModel;
            consultationWindow.ShowDialog();
        }

        private void ShowResultsButton_Click(object sender, RoutedEventArgs e)
        {
            var resultsWindow = new ResultsWindow();
            resultsWindow.ViewModel = viewModel;
            resultsWindow.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(applicationStatePath))
            {
                var stateJson = File.ReadAllText(applicationStatePath);
                var state = JsonConvert.DeserializeObject<ApplicationState>(stateJson);
                viewModel.PathToOntolis = state.PathToOntolis;
                viewModel.PathToModel = state.PathToModel;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var state = new ApplicationState
            {
                PathToModel = viewModel.PathToModel,
                PathToOntolis = viewModel.PathToOntolis
            };

            var stateJson = JsonConvert.SerializeObject(state);
            File.WriteAllText(applicationStatePath, stateJson);
        }
    }
}