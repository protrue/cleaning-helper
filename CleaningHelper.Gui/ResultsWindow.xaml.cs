using CleaningHelper.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CleaningHelper.Model;
using Frame = CleaningHelper.Model.Frame;

namespace CleaningHelper.Gui
{
    /// <summary>
    /// Логика взаимодействия для ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        public ResultsViewModel ViewModel { get; set; }

        public ResultsWindow(FrameModel frameModel, List<Frame> selectedFrames, Frame result)
        {
            InitializeComponent();

            ViewModel = new ResultsViewModel(frameModel, selectedFrames, result);
            DataContext = ViewModel;
            GraphLayout.Graph = ViewModel.Graph;
        }
        private void AutoLayoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            GraphLayout.UpdateLayout();
            GraphLayout.RecalculateOverlapRemoval();


        }
    }
}
