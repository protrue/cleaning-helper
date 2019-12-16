using System.ComponentModel;
using CleaningHelper.ViewModel;
using System.Windows;
using CleaningHelper.Model;

namespace CleaningHelper.Gui
{
    public partial class ConsultationWindow : Window
    {
        public ConsultationViewModel ViewModel { get; set; }
        
        public ConsultationWindow(SemanticNetwork semanticNetwork)
        {
            InitializeComponent();
            ViewModel = new ConsultationViewModel(semanticNetwork);
            DataContext = ViewModel;
            AnswersListBox.ItemsSource = ViewModel.AnswersList;
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void ShowResultsWindow()
        {
            var resultsWindow = new ResultsWindow(ViewModel.Reasoner.InferringPath);
            resultsWindow.ShowDialog();
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Result))
            {
                ShowResultsWindow();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.ConsultCommand.Execute();
        }

        private void ExplainButton_Click(object sender, RoutedEventArgs e)
        {
            ShowResultsWindow();
        }
    }
}