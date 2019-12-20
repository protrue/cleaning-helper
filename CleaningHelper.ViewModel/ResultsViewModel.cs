using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CleaningHelper.Model;
using CleaningHelper.ViewModel.Annotations;
using QuickGraph;

namespace CleaningHelper.ViewModel
{
    public class ResultsViewModel : INotifyPropertyChanged
    {
        private string _selectedLayoutAlgorithm;
        public event PropertyChangedEventHandler PropertyChanged;

        public readonly string DefaultLayoutAlgorithm = "LinLog";

        public FrameModel FrameModel { get; set; }

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
                    graph.AddVertex(frame);

                foreach (var frame in FrameModel.Frames)
                foreach (var frameChild in frame.Children)
                    graph.AddEdge(new Edge<object>(frameChild, frame));

                return graph;
            }
        }

        public ResultsViewModel(FrameModel frameModel)
        {
            SelectedLayoutAlgorithm = DefaultLayoutAlgorithm;
            FrameModel = frameModel;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}