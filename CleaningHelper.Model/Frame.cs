using System.Collections.Generic;

namespace CleaningHelper.Model
{
    public class Frame
    {
        public string Name { get; set; }

        public Frame Parent { get; set; }

        public List<IFrameSlot> Slots { get; set; }

        public Frame(string name, Frame parent = null, List<IFrameSlot> slots = null)
        {
            Name = name;
            Parent = parent;
            Slots = slots ?? new List<IFrameSlot>();
        }
    }
}