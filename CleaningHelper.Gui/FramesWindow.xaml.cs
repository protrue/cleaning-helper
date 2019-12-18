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

            DomainComboBoxColumn.ItemsSource = ViewModel.FrameModel.Domains;
            DomainComboBoxColumn.DisplayMemberPath = "Name";
            DomainComboBoxColumn.SelectedItemBinding = new Binding("Domain");
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

        private void AddFrameButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.FrameModel.Frames.Add(new Frame("Новый фрейм"));
            GraphLayout.Graph = ViewModel.Graph;
        }

        private void RemoveFrameButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.FrameModel.Frames.Remove(ViewModel.SelectedFrame);
            GraphLayout.Graph = ViewModel.Graph;
        }

        private void AddSlotMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedFrame.Slots.Add(new FrameSlot("новый слот", ViewModel.FrameModel.Domains[0]));
        }

        private void RemoveSlotMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedFrame.Slots.Remove(ViewModel.SelectedSlot);
        }

        private void EditSlotMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var domainWindow = new DomainsWindow(ViewModel.FrameModel);
            domainWindow.ShowDialog();
        }
    }
}
