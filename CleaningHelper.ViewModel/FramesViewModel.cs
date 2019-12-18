using System.Collections.Generic;
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
        private Frame _selectedFrame;
        private FrameSlot _selectedSlot;
        public event PropertyChangedEventHandler PropertyChanged;
        public FrameModel FrameModel { get; set; }

        public BidirectionalGraph<object, IEdge<object>> Graph
        {
            get
            {
                var graph = new BidirectionalGraph<object, IEdge<object>>();

                foreach (var frame in FrameModel.Frames)
                    graph.AddVertex(frame);

                foreach (var frame in FrameModel.Frames)
                    foreach (var frameChild in frame.Children)
                        graph.AddEdge(new Edge<object>(frame, frameChild));

                return graph;
            }
        }

        public Frame SelectedFrame
        {
            get => _selectedFrame;
            set
            {
                _selectedFrame = value;
                OnPropertyChanged(nameof(SelectedFrame));
            }
        }

        public FrameSlot SelectedSlot
        {
            get => _selectedSlot;
            set
            {
                _selectedSlot = value; 
                OnPropertyChanged(nameof(SelectedSlot));
            }
        }

        public FrameModel TestFrameModel
        {
            get
            {
                var frameModel = new FrameModel();

                var domain = new FrameSlotDomain("Логический");
                domain.Values.Add("Да");
                domain.Values.Add("Нет");
                frameModel.Domains.Add(domain);

                frameModel.Frames.Add(new Frame("Ситуация"));
                frameModel.Frames.Add(new Frame("Загрязнение"));
                frameModel.Frames.Add(new Frame("Ужасное загрязнение"));

                frameModel[0].Slots.Add(new FrameSlot("Имя слота", domain));
                frameModel[1].Parent = frameModel[0];
                frameModel[2].Parent = frameModel[1];
                
                return frameModel;
            }
        }

        public FramesViewModel()
        {
            FrameModel = TestFrameModel;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
