using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CleaningHelper.Model.Annotations;

namespace CleaningHelper.Model
{
    public class FrameSlotDomainValue : INotifyPropertyChanged
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

        public FrameSlotDomainValue(string text)
        {
            Text = text;
        }

        public static implicit operator FrameSlotDomainValue(string text) =>
            text == null ? null : new FrameSlotDomainValue(text);

        protected bool Equals(FrameSlotDomainValue other)
        {
            return _text == other._text;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FrameSlotDomainValue) obj);
        }

        public override int GetHashCode()
        {
            return (_text != null ? _text.GetHashCode() : 0);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
