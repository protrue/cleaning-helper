using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
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

        public readonly string DefaultLayoutAlgorithm = "LinLog";

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
                OnPropertyChanged(nameof(SelectedDomain));
                OnPropertyChanged(nameof(SelectedDomainValue));
                OnPropertyChanged(nameof(IsComboBoxValueEditable));
                OnPropertyChanged(nameof(DomainValueText));
                OnPropertyChanged(nameof(IsSlotSelected));
                OnPropertyChanged(nameof(IsSlotEditable));
                OnPropertyChanged(nameof(IsSlotTypeEditable));
                OnPropertyChanged(nameof(ComboBoxValueVisibility));
                OnPropertyChanged(nameof(TextBoxValueVisibility));
            }
        }

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

        public Domain SelectedDomain
        {
            get
            {
                if (SelectedSlot == null)
                    return null;

                if (SelectedSlot is DomainSlot domainSlot)
                    return domainSlot.Domain;

                if (SelectedSlot is FrameSlot)
                    return FrameModel.FrameSlotDomain;

                if (SelectedSlot is TextSlot)
                    return FrameModel.TextSlotDomain;

                return null;
            }
            set
            {
                if (SelectedSlot is DomainSlot selectedDomainSlot)
                {
                    if (value == FrameModel.FrameSlotDomain)
                    {
                        var frameSlot = new FrameSlot(SelectedSlot.Name);
                        SelectedFrame.ReplaceSlot(SelectedSlot, frameSlot);
                        SelectedSlot = frameSlot;
                        OnPropertyChanged(nameof(SelectedDomain));
                        OnPropertyChanged(nameof(SelectedDomainValue));
                        OnPropertyChanged(nameof(DomainValueText));
                        OnPropertyChanged(nameof(ComboBoxValueVisibility));
                        OnPropertyChanged(nameof(TextBoxValueVisibility));
                        return;
                    }

                    if (value == FrameModel.TextSlotDomain)
                    {
                        var textSlot = new TextSlot(SelectedSlot.Name);
                        SelectedFrame.ReplaceSlot(SelectedSlot, textSlot);
                        SelectedSlot = textSlot;
                        OnPropertyChanged(nameof(SelectedDomain));
                        OnPropertyChanged(nameof(SelectedDomainValue));
                        OnPropertyChanged(nameof(DomainValueText));
                        OnPropertyChanged(nameof(ComboBoxValueVisibility));
                        OnPropertyChanged(nameof(TextBoxValueVisibility));
                        return;
                    }

                    selectedDomainSlot.Domain = value;
                }

                if (SelectedSlot is FrameSlot)
                {
                    if (value == FrameModel.FrameSlotDomain)
                    {
                        return;
                    }

                    if (value == FrameModel.TextSlotDomain)
                    {
                        var textSlot = new TextSlot(SelectedSlot.Name);
                        SelectedFrame.ReplaceSlot(SelectedSlot, textSlot);
                        SelectedSlot = textSlot;
                        OnPropertyChanged(nameof(SelectedDomain));
                        OnPropertyChanged(nameof(SelectedDomainValue));
                        OnPropertyChanged(nameof(DomainValueText));
                        OnPropertyChanged(nameof(ComboBoxValueVisibility));
                        OnPropertyChanged(nameof(TextBoxValueVisibility));
                        return;
                    }

                    var domainSlot = new DomainSlot(SelectedSlot.Name, value);
                    SelectedFrame.ReplaceSlot(SelectedSlot, domainSlot);
                    SelectedSlot = domainSlot;
                    OnPropertyChanged(nameof(SelectedDomain));
                    OnPropertyChanged(nameof(SelectedDomainValue));
                    OnPropertyChanged(nameof(DomainValueText));
                    OnPropertyChanged(nameof(ComboBoxValueVisibility));
                    OnPropertyChanged(nameof(TextBoxValueVisibility));
                }

                if (SelectedSlot is TextSlot)
                {
                    if (value == FrameModel.FrameSlotDomain)
                    {
                        var frameSlot = new FrameSlot(SelectedSlot.Name);
                        SelectedFrame.ReplaceSlot(SelectedSlot, frameSlot);
                        SelectedSlot = frameSlot;
                        OnPropertyChanged(nameof(SelectedDomain));
                        OnPropertyChanged(nameof(SelectedDomainValue));
                        OnPropertyChanged(nameof(DomainValueText));
                        OnPropertyChanged(nameof(ComboBoxValueVisibility));
                        OnPropertyChanged(nameof(TextBoxValueVisibility));
                        return;
                    }

                    if (value == FrameModel.TextSlotDomain)
                    {
                        return;
                    }

                    var domainSlot = new DomainSlot(SelectedSlot.Name, value);
                    SelectedFrame.ReplaceSlot(SelectedSlot, domainSlot);
                    SelectedSlot = domainSlot;
                    OnPropertyChanged(nameof(SelectedDomain));
                    OnPropertyChanged(nameof(SelectedDomainValue));
                    OnPropertyChanged(nameof(DomainValueText));
                    OnPropertyChanged(nameof(ComboBoxValueVisibility));
                    OnPropertyChanged(nameof(TextBoxValueVisibility));
                }
            }
        }

        public ObservableCollection<DomainValue> DomainValues => SelectedDomain?.Values;

        public bool IsComboBoxValueEditable => SelectedSlot is TextSlot;
        
        public DomainValue SelectedDomainValue
        {
            get
            {
                if (SelectedSlot is DomainSlot domainSlot)
                {
                    return domainSlot.Value;
                }

                if (SelectedSlot is FrameSlot frameSlot)
                {
                    return FrameModel.FrameSlotDomain.Values.FirstOrDefault(v => v.Text == frameSlot.Frame?.Name);
                }

                return null;
            }
            set
            {
                if (SelectedSlot is DomainSlot domainSlot)
                {
                    domainSlot.Value = value;
                    OnPropertyChanged(nameof(SelectedDomainValue));
                    return;
                }

                if (SelectedSlot is FrameSlot frameSlot)
                {
                    frameSlot.Frame = FrameModel.Frames.FirstOrDefault(f => f.Name == value?.Text);
                    OnPropertyChanged(nameof(SelectedDomainValue));
                }
            }
        }

        public string DomainValueText
        {
            get
            {
                if (SelectedSlot is TextSlot textSlot)
                {
                    return textSlot.Text;
                }

                return null;
            }
            set
            {
                if (SelectedSlot is TextSlot textSlot)
                {
                    textSlot.Text = value;
                    OnPropertyChanged(nameof(DomainValueText));
                }
            }
        }

        public bool IsFrameSelected => SelectedFrame != null;

        public bool IsSlotSelected => SelectedSlot != null;

        public bool IsSlotRemovingAvailable => SelectedSlot != null && !SelectedSlot.IsSystemSlot;

        public bool IsSlotEditable => SelectedSlot != null && !SelectedSlot.IsSystemSlot;

        public bool IsSlotTypeEditable => SelectedSlot != null && !SelectedSlot.IsSystemSlot;


        public Visibility ComboBoxValueVisibility =>
            !(SelectedSlot is TextSlot) ? Visibility.Visible : Visibility.Collapsed;

        public Visibility TextBoxValueVisibility =>
            SelectedSlot is TextSlot ? Visibility.Visible : Visibility.Collapsed;

        public FramesViewModel(FrameModel frameModel)
        {
            FrameModel = frameModel;

            FrameModel.PropertyChanged += FrameModelOnPropertyChanged;

            SelectedLayoutAlgorithm = DefaultLayoutAlgorithm;
        }

        private void FrameModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Graph));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
