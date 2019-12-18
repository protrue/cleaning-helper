using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CleaningHelper.Model.Annotations;

namespace CleaningHelper.Model
{

    public class FrameSlot : INotifyPropertyChanged
    {
        private FrameSlotDomainValue _value;
        private string _name;
        private FrameSlotDomain _domain;
        private bool _isResult;

        /// <summary>
        /// Имя слота
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
        /// Домен слота
        /// </summary>
        public FrameSlotDomain Domain
        {
            get => _domain;
            set
            {
                _domain = value; 
                OnPropertyChanged(nameof(Domain));
            }
        }

        /// <summary>
        /// Значение слота
        /// </summary>
        public FrameSlotDomainValue Value
        {
            get => _value;
            set
            {
                if (!Domain.Values.Contains(value) && value != null)
                    throw new ArgumentOutOfRangeException(nameof(value), "Значение должно быть из домена");

                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        /// <summary>
        /// Является ли слот результатом
        /// </summary>
        public bool IsResult
        {
            get => _isResult;
            set
            {
                _isResult = value;
                OnPropertyChanged(nameof(IsResult));
            }
        }

        public FrameSlot(string name, FrameSlotDomain domain, FrameSlotDomainValue value = null, bool isResult = false)
        {
            Name = name;
            Domain = domain;
            Value = value;
            IsResult = isResult;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}