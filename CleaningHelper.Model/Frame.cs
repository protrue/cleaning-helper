using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CleaningHelper.Model.Annotations;

namespace CleaningHelper.Model
{
    public class Frame : INotifyPropertyChanged
    {
        private Frame _parent;
        private string _name;

        /// <summary>
        /// Имя фрейма
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Родительский фрейм
        /// </summary>
        public Frame Parent
        {
            get => _parent;
            set
            {
                Frame oldParent = _parent;
                _parent = value;
                ProcessParentChange(oldParent, value);
                OnPropertyChanged(nameof(Parent));
            }
        }

        /// <summary>
        /// Список фреймов-наследников
        /// </summary>
        public ObservableCollection<Frame> Children { get; }

        /// <summary>
        /// Список слотов
        /// </summary>
        public ObservableCollection<FrameSlot> Slots { get; }

        public Frame(string name)
        {
            Name = name;
            Children = new ObservableCollection<Frame>();
            Slots = new ObservableCollection<FrameSlot>();

            Children.CollectionChanged += ChildrenOnCollectionChanged;
            Slots.CollectionChanged += SlotsOnCollectionChanged;
        }

        private void SlotsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        var addedSlot = newItem as FrameSlot;
                        ProcessAddedSlot(addedSlot);
                    }
                    break;
            }
        }

        private void ProcessAddedSlot(FrameSlot addedSlot)
        {
            //if (Slots.Contains(addedSlot))
            //{
            //    Slots.Remove(addedSlot);
            //    throw new ArgumentException("Такой слот уже есть у этого фрейма", nameof(addedSlot));
            //}
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        var addedChild = newItem as Frame;
                        ProcessAddedChild(addedChild);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        var removedChild = oldItem as Frame;
                        ProcessAddedChild(removedChild);
                    }
                    break;
            }
        }

        private void ProcessParentChange(Frame oldParent, Frame newParent)
        {
            if (newParent == null)
            {
                oldParent.Children.Remove(this);
            }
            else
            {
                newParent.Children.Add(this);
            }
        }

        private void ProcessAddedChild(Frame child)
        {
            //if (child == null)
            //    throw new ArgumentNullException(nameof(child));

            //if (Children.Contains(child))
            //    throw new ArgumentException("Фрейм уже является наследником", nameof(child));

            child._parent = this;
        }

        private void ProcessRemovedChild(Frame child)
        {
            //if (child == null)
            //    throw new ArgumentNullException(nameof(child));

            //if (!Children.Contains(child))
            //    new ArgumentException("Нет такого фрейма-наследника", nameof(child));

            child._parent = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}