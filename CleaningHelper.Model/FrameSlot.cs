using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningHelper.Model
{
    /// <summary>
    /// Слот фреймого типа (указатель на другой фрейм)
    /// </summary>
    public class FrameSlot : Slot
    {
        private Frame _frame;

        public Frame Frame
        {
            get => _frame;
            set
            {
                _frame = value;
                OnPropertyChanged(nameof(Frame));
            }
        }

        public FrameSlot(string name, Frame frame = null, bool isSystemSlot = false, bool isRequestable = false, bool isResult = false) :
            base(name, isSystemSlot, isRequestable, isResult)
        {
            Frame = frame;
        }
    }
}
