using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using CleaningHelper.ViewModel;
using System.Windows;
using CleaningHelper.Model;

namespace CleaningHelper.Gui
{
    public partial class ConsultationWindow : Window
    {
        public ConsultationViewModel ViewModel { get; set; }

        public ConsultationWindow(FrameModel frameModel)
        {
            InitializeComponent();

            ViewModel = new ConsultationViewModel(frameModel);
            DataContext = ViewModel;
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void ShowResultsWindow()
        {
            var resultsWindow = new ResultsWindow(ViewModel.FrameModel, ViewModel.Reasoner.GetInferringPath(), ViewModel.Result);
            resultsWindow.ShowDialog();
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Result))
            {
                if (ViewModel.Result == null)
                    MessageBox.Show("Не привязялся ни один фрейм", "К сожалению, не нашлось ни одного рецепта");
                else
                    ShowResultsWindow();

                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SetQuestionCommand.Execute();
        }

        private void ExplainButton_Click(object sender, RoutedEventArgs e)
        {
            ShowResultsWindow();
        }
    }
}