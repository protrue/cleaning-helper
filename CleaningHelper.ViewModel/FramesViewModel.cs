using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CleaningHelper.Model;
using CleaningHelper.ViewModel.Annotations;
using QuickGraph;

namespace CleaningHelper.ViewModel
{
    public class FramesViewModel : INotifyPropertyChanged
    {
        private BidirectionalGraph<object, IEdge<object>> _graph;
        private Frame _selectedFrame;
        private Slot _selectedSlot;
        private string _selectedLayoutAlgorithm;
        public event PropertyChangedEventHandler PropertyChanged;
        public FrameModel FrameModel { get; set; }

        public BidirectionalGraph<object, IEdge<object>> Graph
        {
            get
            {
                _graph = new BidirectionalGraph<object, IEdge<object>>();

                foreach (var frame in FrameModel.Frames)
                    _graph.AddVertex(frame);

                foreach (var frame in FrameModel.Frames)
                    foreach (var frameChild in frame.Children)
                        _graph.AddEdge(new Edge<object>(frameChild, frame));

                return _graph;
            }
        }

        public Frame SelectedFrame
        {
            get => _selectedFrame;
            set
            {
                _selectedFrame = value;
                OnPropertyChanged(nameof(SelectedFrame));
                OnPropertyChanged(nameof(IsFrameSelected));
            }
        }

        public Slot SelectedSlot
        {
            get => _selectedSlot;
            set
            {
                _selectedSlot = value;
                OnPropertyChanged(nameof(SelectedSlot));
                OnPropertyChanged(nameof(IsSlotSelected));
                OnPropertyChanged(nameof(IsSlotNameEditable));
                OnPropertyChanged(nameof(IsSlotTypeEditable));
            }
        }

        public List<string> LayoutAlgorithms => new List<string>(new []
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

        public Domain SelectedDomain { get; set; }

        public Domain SelectedDomainValue { get; set; }

        public string DomainValueText { get; set; }

        public bool IsFrameSelected => SelectedFrame != null;

        public bool IsSlotSelected => SelectedSlot != null;

        public bool IsSlotNameEditable => !SelectedSlot?.IsSystemSlot ?? false;

        public bool IsSlotTypeEditable => !SelectedSlot?.IsSystemSlot ?? false;

        public FrameModel TestFrameModel
        {
            get
            {
                var domains = new[]
                {
                    new Domain("Логический", new[]
                    {
                        new DomainValue("Да"),
                        new DomainValue("Нет"),
                    }),
                    new Domain("Ткань", new[]
                    {
                        new DomainValue("Хлопок"),
                        new DomainValue("Синтетика"),
                    }),
                };

                var frames = new[]
                {
                    new Frame("Ситуация"),
                    new Frame("Загрязнение"),
                    new Frame("Ужасное загрязнение"),
                    new Frame("Лёгкое загрязнение"),
                };

                var frameModel = new FrameModel();

                frames[0].Slots.Add(new DomainSlot("Имя слота", domains[0], domains[0].Values[0]));
                frames[1].Parent = frames[0];
                frames[2].Parent = frames[1];
                frames[3].Parent = frames[1];

                foreach (var domain in domains)
                {
                    frameModel.Domains.Add(domain);
                }

                foreach (var frame in frames)
                {
                    frameModel.Frames.Add(frame);
                }

                return frameModel;
            }
        }

        public FramesViewModel()
        {
            FrameModel = TestFrameModel;

            SelectedLayoutAlgorithm = LayoutAlgorithms.Last();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
