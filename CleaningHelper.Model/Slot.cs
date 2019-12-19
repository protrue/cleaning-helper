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
    /// <summary>
    /// Обобщение слота фрейма
    /// </summary>
    public abstract class Slot : INotifyPropertyChanged
    {
        private string _name;
        private bool _isResult;
        private bool _isSystemSlotSlot;
        private bool _isRequestable;

        public event PropertyChangedEventHandler PropertyChanged;

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
        /// Является ли слот системным
        /// </summary>
        public bool IsSystemSlotSlot
        {
            get => _isSystemSlotSlot;
            set
            {
                _isSystemSlotSlot = value;
                OnPropertyChanged(nameof(IsSystemSlotSlot));
            }
        }

        /// <summary>
        /// Является ли слот запрашиваемым
        /// </summary>
        public bool IsRequestable
        {
            get => _isRequestable;
            set
            {
                _isRequestable = value;
                OnPropertyChanged(nameof(IsRequestable));
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

        protected Slot(string name, bool isSystemSlot = false, bool isRequestable = false, bool isResult = false)
        {
            Name = name;
            IsSystemSlotSlot = isSystemSlot;
            IsRequestable = isRequestable;
            IsResult = isResult;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
