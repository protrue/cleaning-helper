using System;
using System.Collections.Generic;

namespace CleaningHelper.Model
{
    public class Frame
    {
        private readonly List<Frame> _children;
        private Frame _parent;
        private readonly List<IFrameSlot> _slots;

        /// <summary>
        /// Имя фрейма
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Родительский фрейм
        /// </summary>
        public Frame Parent => _parent;

        /// <summary>
        /// Список фреймов-наследников
        /// </summary>
        public IReadOnlyList<Frame> Children => _children.AsReadOnly();

        /// <summary>
        /// Список слотов
        /// </summary>
        public List<IFrameSlot> Slots => _slots;

        public Frame(string name)
        {
            Name = name;
            _children = new List<Frame>();
            _slots = new List<IFrameSlot>();
        }

        /// <summary>
        /// Устанавливает родителя текущего фрейма, добавляет текущий фрейм в список наследников parent
        /// </summary>
        /// <param name="parent">Родительский фрейм</param>
        public void SetParent(Frame parent)
        {
            _parent = parent;
            parent._children.Add(this);
        }

        /// <summary>
        /// Добавляет текущему фрейму в список наследников child, устанавливает родительский фрейм у child
        /// </summary>
        /// <param name="child">Фрейм-наследник</param>
        public void AddChild(Frame child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));

            if (_children.Contains(child))
                throw new ArgumentException("Фрейм уже является наследником", nameof(child));

            _children.Add(child);
            child._parent = this;
        }

        /// <summary>
        /// Удаляет у текущего фрейма из списка наследников child, устанавливает null в родительский фрейм у child 
        /// </summary>
        /// <param name="child">Фрейм-наследник</param>
        public void RemoveChild(Frame child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));

            if (!_children.Contains(child))
                new ArgumentException("Нет такого фрейма-наследника", nameof(child));

            _children.Remove(child);
            child._parent = null;
        }
    }
}