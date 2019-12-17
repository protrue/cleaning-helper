using System;

namespace CleaningHelper.Model
{
    public class FrameSlot<T> : IFrameSlot
    {
        /// <summary>
        /// Имя слота
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Является ли слот результатом
        /// </summary>
        public bool IsResult { get; set; }

        /// <summary>
        /// Значение слота
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Тип слота
        /// </summary>
        public Type Type => typeof(T);

        public FrameSlot(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}