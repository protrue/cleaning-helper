using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public FramesWindow(FrameModel frameModel)
        {
            InitializeComponent();

            ViewModel = new FramesViewModel(frameModel);
            DataContext = ViewModel;
            GraphLayout.Graph = ViewModel.Graph;

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //UpdateGraphLayout();
        }

        private void EditDomainsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var domainWindow = new DomainsWindow(ViewModel.FrameModel);
            domainWindow.ShowDialog();
        }

        private void AddFrameButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.FrameModel.Frames.Add(new Frame("Новый фрейм"));
                UpdateGraphLayout();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void RemoveFrameButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.FrameModel.Frames.Remove(ViewModel.SelectedFrame);
                UpdateGraphLayout();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void UpdateGraphLayout()
        {
            GraphLayout.Graph = ViewModel.Graph;
            //GraphLayout.RecalculateOverlapRemoval();
            //GraphLayout.UpdateLayout();
        }

        private void AutoLayoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.AllChanged();
            GraphLayout.UpdateLayout();
            GraphLayout.RecalculateOverlapRemoval();
        }

        private void AddSlotButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.SelectedFrame.Slots.Add(new TextSlot("Новый слот"));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void RemoveSlotButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedSlot.IsSystemSlot)
                MessageBox.Show("Нельзя удалить системный слот");

            try
            {
                ViewModel.SelectedFrame.Slots.Remove(ViewModel.SelectedSlot);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void AddParentMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            //try
            //{
            var frame = new Frame("Новый фрейм");
            ViewModel.FrameModel.Frames.Add(frame);
            frame.Parent = ViewModel.SelectedFrame;
            
            UpdateGraphLayout();
            //}
            //catch (Exception exception)
            //{
            //    MessageBox.Show(exception.Message);
            //}
        }


        private void AddChildMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            //try
            //{
            var frame = new Frame("Новый фрейм");
            ViewModel.FrameModel.Frames.Add(frame);
            ViewModel.SelectedFrame.Parent = frame;
            UpdateGraphLayout();
            //}
            //catch (Exception exception)
            //{
            //    MessageBox.Show(exception.Message);
            //}
        }

        private void GraphLayout_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is VertexControl vertexControl
                && (e.ChangedButton == MouseButton.Left
                    || e.ChangedButton == MouseButton.Right))
            {
                ViewModel.SelectedFrame = vertexControl.Vertex as Frame;
                GraphLayout.HighlightVertex(ViewModel.SelectedFrame, "None");
            }
            else
            {
                ViewModel.SelectedFrame = null;
            }
        }
    }
}
