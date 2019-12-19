using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CleaningHelper.Model.Annotations;

namespace CleaningHelper.Model
{
    /// <summary>
    /// Фрейм
    /// </summary>
    public class Frame : INotifyPropertyChanged
    {
        private readonly TextSlot _nameSystemSlot;
        private readonly FrameSlot _parentSystemSlot;

        public readonly string NameSlotName = "Имя";
        public readonly string ParentSlotName = "Родитель";
        public readonly string RecipeSlotName = "Рецепт";

        /// <summary>
        /// Слот по индексу
        /// </summary>
        /// <param name="index">Индекс</param>
        /// <returns>Слот</returns>
        public Slot this[int index] => Slots[index];

        /// <summary>
        /// Слот по имени
        /// </summary>
        /// <param name="name">Имя слота</param>
        /// <returns>Слот</returns>
        public Slot this[string name] => Slots.First(s => s.Name == name);

        /// <summary>
        /// Имя фрейма
        /// </summary>
        public string Name
        {
            get => _nameSystemSlot.Text;
            set
            {
                _nameSystemSlot.Text = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Родительский фрейм
        /// </summary>
        public Frame Parent
        {
            get => _parentSystemSlot.Frame;
            set
            {
                var oldParent = _parentSystemSlot.Frame;
                _parentSystemSlot.Frame = value;
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
        public ObservableCollection<Slot> Slots { get; }

        public Frame(string name)
        {
            Name = name;
            Children = new ObservableCollection<Frame>();
            Slots = new ObservableCollection<Slot>();

            _nameSystemSlot = new TextSlot(NameSlotName, isSystemSlot: true);
            _parentSystemSlot = new FrameSlot(ParentSlotName, isSystemSlot: true);

            Slots.Add(_nameSystemSlot);
            Slots.Add(_parentSystemSlot);
            Slots.Add(new TextSlot(RecipeSlotName, isSystemSlot: true));

            Children.CollectionChanged += ChildrenOnCollectionChanged;
            Slots.CollectionChanged += SlotsOnCollectionChanged;
        }

        public TextSlot GetNameSystemSlot() =>
            Slots.OfType<TextSlot>().First(s => s.Name == NameSlotName && s.IsSystemSlotSlot);

        public FrameSlot GetParentSystemSlot() =>
            Slots.OfType<FrameSlot>().First(s => s.Name == ParentSlotName && s.IsSystemSlotSlot);

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

        private void ProcessAddedSlot(FrameSlot addedFrameSlot)
        {
            if (Slots.Count(s => ReferenceEquals(s, addedFrameSlot)) > 1)
            {
                Slots.Remove(addedFrameSlot);
                throw new ArgumentException("Такой слот уже есть у этого фрейма", nameof(addedFrameSlot));
            }
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
                        ProcessRemovedChild(removedChild, e.OldItems);
                    }
                    break;
            }
        }

        private void ProcessParentChange(Frame oldParent, Frame newParent)
        {
            if (newParent == null)
            {
                oldParent?.Children.Remove(this);
            }
            else
            {
                newParent.Children.Add(this);
            }
        }

        private void ProcessAddedChild(Frame child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));

            if (Children.Count(c => ReferenceEquals(c, child)) > 1)
                throw new ArgumentException("Фрейм уже является наследником", nameof(child));

            child._parentSystemSlot.Frame = this;
        }

        private void ProcessRemovedChild(Frame child, IList oldItems)
        {
            child._parentSystemSlot.Frame = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}