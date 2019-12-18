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
    public class FrameSlotDomain: INotifyPropertyChanged
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ObservableCollection<FrameSlotDomainValue> Values { get; }

        public FrameSlotDomainValue this[int index]
        {
            get => Values[index];
            set => Values[index] = value;
        }

        public FrameSlotDomain(string name, IEnumerable<FrameSlotDomainValue> values = null)
        {
            Name = name;
            Values = values == null
                ? new ObservableCollection<FrameSlotDomainValue>()
                : new ObservableCollection<FrameSlotDomainValue>(values);
            Values.CollectionChanged += ValuesOnCollectionChanged;
        }

        private void ValuesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newItem in e.NewItems)
                {
                    var domainValue = newItem as FrameSlotDomainValue;
                    ProcessAddedDomainValue(domainValue);
                }
            }
        }

        private void ProcessAddedDomainValue(FrameSlotDomainValue domainValue)
        {
            //if (Values.Contains(domainValue))
            //{
            //    Values.Remove(domainValue);
            //    throw new ArgumentException("Одинаковые значения доменов недопустимы", nameof(domainValue));
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
