using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using CleaningHelper.Model;
using CleaningHelper.ViewModel.Annotations;
using QuickGraph;
using Frame = CleaningHelper.Model.Frame;

namespace CleaningHelper.ViewModel
{
    public class ResultsViewModel : INotifyPropertyChanged
    {
        private string _selectedLayoutAlgorithm;
        public event PropertyChangedEventHandler PropertyChanged;

        public readonly string DefaultLayoutAlgorithm = "LinLog";

        public FrameModel FrameModel { get; set; }
        public List<Frame> SelectedFrames { get; }

        public List<string> LayoutAlgorithms => new List<string>(new[]
        {
            "BoundedFR",
            "Circular",
            "CompoundFDP",
            "EfficientSugiyama",
            "FR",
            "ISOM",
            "KK",
            "LinLog",
            "Tree",
        });

        public string SelectedLayoutAlgorithm
        {
            get => _selectedLayoutAlgorithm;
            set
            {
                _selectedLayoutAlgorithm = value;
                OnPropertyChanged(nameof(SelectedLayoutAlgorithm));
            }
        }

        public BidirectionalGraph<object, IEdge<object>> Graph
        {
            get
            {
                var graph = new BidirectionalGraph<object, IEdge<object>>();

                foreach (var frame in FrameModel.Frames)
                     if (SelectedFrames.Contains(frame))
                        graph.AddVertex(frame);

                foreach (var frame in SelectedFrames)
                foreach (var frameChild in frame.Children.Where(x => SelectedFrames.Contains(x)))
                    graph.AddEdge(new Edge<object>(frameChild, frame));

                foreach (var frame in SelectedFrames)
                {
                    foreach (var slot in frame.Slots)
                    {
                        if (slot is FrameSlot frameSlot && frameSlot.Frame != null && SelectedFrames.Contains(frameSlot.Frame) && !frameSlot.IsSystemSlot)
                        {
                            graph.AddEdge(new AlternateEdge(frameSlot.Frame, frame)
                            {
                                EdgeColor = Colors.Blue
                            });
                        }
                    }
                }
                
                return graph;
            }
        }

        public ResultsViewModel(FrameModel frameModel, List<Frame> selectedFrames)
        {
            SelectedLayoutAlgorithm = DefaultLayoutAlgorithm;
            SelectedFrames = selectedFrames;
            FrameModel = frameModel;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}