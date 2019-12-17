using System;

namespace CleaningHelper.Model
{
    public class FrameSlot<T> : IFrameSlot
    {
        public string Name { get; set; }

        public T Value { get; set; }

        public Type Type => typeof(T);

        public FrameSlot(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}