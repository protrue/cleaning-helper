using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CleaningHelper.Model.Annotations;

namespace CleaningHelper.Model
{
    /// <summary>
    /// Слот фрейма доменного (перечислимого) типа
    /// </summary>
    public class DomainSlot : Slot
    {
        private DomainValue _value;
        private Domain _domain;

        public override string TypeAsString => Domain.Name;

        public override string ValueAsString => Value?.Text ?? string.Empty;

        /// <summary>
        /// Домен слота
        /// </summary>
        public Domain Domain
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
        public DomainValue Value
        {
            get => _value;
            set
            {
                if (value != null && !Domain.Values.Contains(value))
                    throw new ArgumentOutOfRangeException(nameof(value), "Значение должно быть из домена");

                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public DomainSlot(string name, Domain domain, DomainValue value = null, bool isSystemSlot = false, bool isRequestable = false, bool isResult = false) : base(name, isSystemSlot, isRequestable, isResult)
        {
            Domain = domain;
            Value = value;
        }
    }
}