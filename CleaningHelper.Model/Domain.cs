using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CleaningHelper.Model.Annotations;

namespace CleaningHelper.Model
{
    /// <summary>
    /// Домен допустимых значений
    /// </summary>
    public class Domain
    {
        private string _name;

        /// <summary>
        /// Имя домена
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
        /// Допустимые значения домена
        /// </summary>
        public ObservableCollection<DomainValue> Values { get; }
        
        /// <summary>
        /// Значение домена по индексу
        /// </summary>
        /// <param name="index">Индекс</param>
        /// <returns>Значение домена</returns>
        public DomainValue this[int index]
        {
            get => Values[index];
            set => Values[index] = value;
        }

        /// <summary>
        /// Значение домена по тексту значения домена
        /// </summary>
        /// <param name="text">Текст</param>
        /// <returns>Значение домена</returns>
        public DomainValue this[string text] =>
            Values.First(v => v.Text == text);

        public event PropertyChangedEventHandler PropertyChanged;

        public Domain(string name, IEnumerable<DomainValue> values = null)
        {
            Values = values == null
                ? new ObservableCollection<DomainValue>()
                : new ObservableCollection<DomainValue>(values);
            Values.CollectionChanged += ValuesOnCollectionChanged;
        }

        private void ValuesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newItem in e.NewItems)
                {
                    var domainValue = newItem as DomainValue;
                    ProcessAddedDomainValue(domainValue);
                }
            }
        }

        private void ProcessAddedDomainValue(DomainValue domainValue)
        {
            if (Values.Count(dv => ReferenceEquals(dv, domainValue)) > 1)
            {
                Values.Remove(domainValue);
                throw new ArgumentException("Одинаковые значения доменов недопустимы", nameof(domainValue));
            }
        }
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
