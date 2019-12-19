using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningHelper.Model
{
    /// <summary>
    /// Слот текстового типа
    /// </summary>
    public class TextSlot : Slot
    {
        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public TextSlot(string name, string text = null, bool isSystemSlot = false, bool isRequestable = false, bool isResult = false) : base(name, isSystemSlot, isRequestable, isResult)
        {
            Text = text ?? string.Empty;
        }
    }
}
