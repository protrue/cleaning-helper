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
using CleaningHelper.ViewModel;
using GraphSharp.Controls;
using QuickGraph;
using CleaningHelper.Model;
using GraphSharp.Algorithms.Highlight;
using Frame = CleaningHelper.Model.Frame;

namespace CleaningHelper.Gui
{
    /// <summary>
    /// Логика взаимодействия для FramesWindow.xaml
    /// </summary>
    public partial class FramesWindow : Window
    {
        public FramesViewModel ViewModel { get; set; }

        public FramesWindow()
        {
            InitializeComponent();

            ViewModel = new FramesViewModel();
            DataContext = ViewModel;
            GraphLayout.Graph = ViewModel.Graph;
        }

        private void GraphLayout_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is VertexControl vertexControl)
            {
                ViewModel.SelectedFrame = vertexControl.Vertex as Frame;
                GraphLayout.HighlightVertex(ViewModel.SelectedFrame, "None");
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var domainWindow = new DomainsWindow(ViewModel.FrameModel);
            domainWindow.ShowDialog();
        }
    }
}
