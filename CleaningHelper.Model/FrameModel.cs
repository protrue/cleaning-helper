using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CleaningHelper.Model
{
    public class FrameModel
    {
        private Frame _frameToDelete;

        /// <summary>
        /// Фреймы модели
        /// </summary>
        public ObservableCollection<Frame> Frames { get; set; }
        
        /// <summary>
        /// Домены модели
        /// </summary>
        public ObservableCollection<FrameSlotDomain> Domains { get; }

        public Frame this[int index] => Frames[index];

        public Frame this[string name] => Frames.First(f => f.Name == name);

        public FrameModel()
        {
            Domains = new ObservableCollection<FrameSlotDomain>();
            Frames = new ObservableCollection<Frame>();

            Domains.CollectionChanged += DomainsOnCollectionChanged;
            Frames.CollectionChanged += FramesOnCollectionChanged;
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
        }

        private void DomainsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (var newItem in e.NewItems)
                        {
                            var domain = newItem as FrameSlotDomain;
                            ProcessAddedDomain(domain);
                        }

                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (var oldItem in e.OldItems)
                        {
                            var domain = oldItem as FrameSlotDomain;
                            ProcessRemovedDomain(domain);
                        }

                        break;
                    }
            }
        }

        private void ProcessAddedFrame(Frame frame)
        {
            //if (frame == null)
            //    throw new ArgumentNullException(nameof(frame), "Фрейм не может быть null");

            //if (Frames.Count(f => ReferenceEquals(f, frame)) > 1)
            //{
            //    Frames.Remove(frame);
            //    throw new ArgumentException("Такой фрейм уже есть в модели", nameof(frame));
            //}

            foreach (var slot in frame.Slots)
            {
                var domain = slot.Domain;

                if (!Domains.Contains(domain))
                    Domains.Add(domain);
            }

            frame.Children.CollectionChanged += FrameChildrenOnCollectionChanged;
        }

        private void ProcessRemovedFrame(Frame frame)
        {
            //if (frame == null)
            //    throw new ArgumentNullException(nameof(frame), "Фрейм не может быть null");

            //if (!Frames.Contains(frame))
            //    throw new ArgumentException("Такого фрейма нет в модели", nameof(frame));

            _frameToDelete = frame;

            frame.Parent = null;

            foreach (var frameChild in frame.Children)
            {
                frameChild.Parent = null;
            }

            frame.Children.CollectionChanged -= FrameChildrenOnCollectionChanged;
            frame.PropertyChanged -= FrameOnPropertyChanged;
        }

        private void FrameOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is Frame frame) || e.PropertyName != nameof(Frame.Parent) || frame == _frameToDelete) 
                return;

            var parent = frame.Parent;
            ProcessExternallyAddedFrame(parent);
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
            if (!Frames.Contains(frame))
                Frames.Add(frame);
        }

        public void ProcessAddedDomain(FrameSlotDomain domain)
        {
            //if (Domains.Contains(domain))
            //{
            //    Domains.Remove(domain);
            //    throw new ArgumentException("Такой домен уже есть в модели", nameof(domain));
            //}
        }

        public void ProcessRemovedDomain(FrameSlotDomain domain)
        {
            //if (domain == null)
            //    throw new ArgumentNullException(nameof(domain), "Домен не может быть null");

            //if (!Domains.Contains(domain))
            //    throw new ArgumentException("Такого домена нет в модели", nameof(domain));

            var useList = new List<Tuple<Frame, FrameSlot>>();

            foreach (var frame in Frames)
                foreach (var slot in frame.Slots)
                    if (slot.Domain == domain)
                        useList.Add(Tuple.Create(frame, slot));

            if (useList.Count > 0)
            {
                Domains.Add(domain);
                throw new DomainIsInUseException(useList, domain);
            }
        }

        public List<Tuple<Frame, FrameSlot, FrameSlotDomain>> CheckDomainValueIntegrity(FrameSlotDomainValue valueToCheck)
        {
            var usedList = new List<Tuple<Frame, FrameSlot, FrameSlotDomain>>();

            foreach (var frame in Frames)
            {
                foreach (var slot in frame.Slots)
                {
                    var domain = slot.Domain;

                    if (slot.Value == valueToCheck)
                    {
                        usedList.Add(Tuple.Create(frame, slot, domain));
                    }
                }
            }

            return usedList;
        }

        public List<Tuple<Frame, FrameSlot, FrameSlotDomain, FrameSlotDomainValue>> RestoreDomainValueIntegrity()
        {
            var resetList = new List<Tuple<Frame, FrameSlot, FrameSlotDomain, FrameSlotDomainValue>>();

            foreach (var frame in Frames)
            {
                foreach (var slot in frame.Slots)
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
    }
}
