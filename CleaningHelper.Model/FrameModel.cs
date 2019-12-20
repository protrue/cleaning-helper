using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using CleaningHelper.Model.Annotations;
using CleaningHelper.Model.Exceptions;

namespace CleaningHelper.Model
{
    /// <summary>
    /// Фреймовая модель
    /// </summary>
    [Serializable]
    public class FrameModel : INotifyPropertyChanged
    {
        private readonly List<Delegate> _serializableDelegates;

        private readonly Domain _frameSlotDomain;
        private readonly Domain _textSlotDomain;
        private Frame _frameToDelete;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Фреймы модели
        /// </summary>
        public ObservableCollection<Frame> Frames { get; set; }
        
        /// <summary>
        /// Домены модели
        /// </summary>
        public ObservableCollection<Domain> Domains { get; }

        /// <summary>
        /// Фрейм по индексу
        /// </summary>
        /// <param name="index">Индекс</param>
        /// <returns>Фрейм</returns>
        public Frame this[int index] => Frames[index];

        /// <summary>
        /// Фрейм по имени
        /// </summary>
        /// <param name="name">Имя фрейма</param>
        /// <returns>Фрейм</returns>
        public Frame this[string name] => Frames.First(f => f.Name == name);

        public Domain FrameSlotDomain => _frameSlotDomain;

        public Domain TextSlotDomain => _textSlotDomain;

        public FrameModel()
        {
            Domains = new ObservableCollection<Domain>();
            Frames = new ObservableCollection<Frame>();

            _frameSlotDomain = new Domain(FrameSlot.TypeFriendlyName);
            _textSlotDomain = new Domain(TextSlot.TypeFriendlyName);

            _frameSlotDomain.Values.Add(string.Empty);

            Domains.Add(_frameSlotDomain);
            Domains.Add(_textSlotDomain);

            Domains.CollectionChanged += DomainsOnCollectionChanged;
            Frames.CollectionChanged += FramesOnCollectionChanged;

            _serializableDelegates = new List<Delegate>();
        }

        private void FramesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (var newItem in e.NewItems)
                        {
                            var frame = newItem as Frame;
                            ProcessAddedFrame(frame);
                        }

                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (var oldItem in e.OldItems)
                        {
                            var frame = oldItem as Frame;
                            ProcessRemovedFrame(frame);
                        }

                        break;
                    }
            }

            OnPropertyChanged(nameof(Frames));
        }

        private void DomainsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (var newItem in e.NewItems)
                        {
                            var domain = newItem as Domain;
                            ProcessAddedDomain(domain);
                        }

                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (var oldItem in e.OldItems)
                        {
                            var domain = oldItem as Domain;
                            ProcessRemovedDomain(domain);
                        }

                        break;
                    }
            }

            OnPropertyChanged(nameof(Domains));
        }

        private void ProcessAddedFrame(Frame frame)
        {
            if (frame == null)
                throw new ArgumentNullException(nameof(frame), "Фрейм не может быть null");

            if (Frames.Count(f => ReferenceEquals(f, frame)) > 1)
            {
                Frames.Remove(frame);
                throw new ArgumentException("Такой фрейм уже есть в модели", nameof(frame));
            }

            foreach (var slot in frame.Slots.OfType<DomainSlot>())
            {
                var domain = slot.Domain;

                if (!Domains.Contains(domain))
                    Domains.Add(domain);
            }

            _frameSlotDomain.Values.Add(frame.Name);

            frame.Children.CollectionChanged += FrameChildrenOnCollectionChanged;
            frame.PropertyChanged += FrameOnPropertyChanged;
        }

        private void ProcessRemovedFrame(Frame frame)
        {
            _frameToDelete = frame;

            frame.Parent = null;

            foreach (var frameChild in frame.Children)
            {
                frameChild.Parent = null;
            }

            var domainValue = _frameSlotDomain.Values.First(v => v.Text == frame.Name);
            _frameSlotDomain.Values.Remove(domainValue);

            frame.Children.CollectionChanged -= FrameChildrenOnCollectionChanged;
            frame.PropertyChanged -= FrameOnPropertyChanged;
        }

        private void FrameOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is Frame frame) || e.PropertyName != nameof(Frame.Parent) || frame == _frameToDelete) 
                return;

            var parent = frame.Parent;
            ProcessExternallyAddedFrame(parent);

            OnPropertyChanged(nameof(Frames));
        }

        private void FrameChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        var addedFrame = newItem as Frame;
                        ProcessExternallyAddedFrame(addedFrame);
                    }
                    break;
            }
        }

        private void ProcessExternallyAddedFrame(Frame frame)
        {
            if (frame != null && !Frames.Contains(frame))
                Frames.Add(frame);
        }

        private void ProcessAddedDomain(Domain domain)
        {
            if (Domains.Count(d => ReferenceEquals(d, domain)) > 1)
            {
                Domains.Remove(domain);
                throw new ArgumentException("Такой домен уже есть в модели", nameof(domain));
            }

            domain.PropertyChanged += DomainOnPropertyChanged;
        }

        private void DomainOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Domains));
        }

        private void ProcessRemovedDomain(Domain domain)
        {
            var useList = new List<Tuple<Frame, DomainSlot>>();

            foreach (var frame in Frames)
                foreach (var slot in frame.Slots.OfType<DomainSlot>())
                    if (ReferenceEquals(slot.Domain, domain))
                        useList.Add(Tuple.Create(frame, slot));

            if (useList.Count > 0)
            {
                Domains.Add(domain);
                throw new DomainIsInUseException(useList, domain);
            }

            domain.PropertyChanged -= DomainOnPropertyChanged;
        }

        public List<Tuple<Frame, DomainSlot, Domain>> CheckDomainValueIntegrity(DomainValue valueToCheck)
        {
            var usedList = new List<Tuple<Frame, DomainSlot, Domain>>();

            foreach (var frame in Frames)
            {
                foreach (var slot in frame.Slots.OfType<DomainSlot>())
                {
                    var domain = slot.Domain;

                    if (ReferenceEquals(slot.Value, valueToCheck))
                    {
                        usedList.Add(Tuple.Create(frame, slot, domain));
                    }
                }
            }

            return usedList;
        }

        public List<Tuple<Frame, DomainSlot, Domain, DomainValue>> RestoreDomainValueIntegrity()
        {
            var resetList = new List<Tuple<Frame, DomainSlot, Domain, DomainValue>>();

            foreach (var frame in Frames)
            {
                foreach (var slot in frame.Slots.OfType<DomainSlot>())
                {
                    var domain = slot.Domain;
                    var value = slot.Value;

                    if (!domain.Values.Contains(value))
                    {
                        resetList.Add(Tuple.Create(frame, slot, domain, value));
                        slot.Value = null;
                    }
                }
            }

            return resetList;
        }
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [OnSerializing]
        public void OnSerializing(StreamingContext context)
        {
            _serializableDelegates.Clear();
            var handler = PropertyChanged;

            if (handler != null)
            {
                foreach (var invocation in handler.GetInvocationList())
                {
                    if (invocation.Target.GetType().IsSerializable)
                    {
                        _serializableDelegates.Add(invocation);
                    }
                }
            }
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (_serializableDelegates == null) return;

            foreach (var invocation in _serializableDelegates)
            {
                PropertyChanged += (PropertyChangedEventHandler)invocation;
            }
        }
    }
}
