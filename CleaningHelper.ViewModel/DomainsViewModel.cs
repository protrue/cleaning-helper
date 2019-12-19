using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CleaningHelper.Model;
using CleaningHelper.ViewModel.Annotations;

namespace CleaningHelper.ViewModel
{
    public class DomainsViewModel : INotifyPropertyChanged
    {
        private Domain _selectedDomain;
        private DomainValue _selectedValue;
        public event PropertyChangedEventHandler PropertyChanged;

        public FrameModel FrameModel { get; set; }

        public Domain SelectedDomain
        {
            get => _selectedDomain;
            set
            {
                _selectedDomain = value;
                OnPropertyChanged(nameof(SelectedDomain));
                OnPropertyChanged(nameof(IsRemoveDomainAvailable));
                OnPropertyChanged(nameof(IsAddValueAvailable));
                OnPropertyChanged(nameof(IsRemoveValueAvailable));
                OnPropertyChanged(nameof(IsValuesReadOnly));
            }
        }

        public DomainValue SelectedValue
        {
            get => _selectedValue;
            set
            {
                _selectedValue = value;
                OnPropertyChanged(nameof(SelectedValue));
                OnPropertyChanged(nameof(IsRemoveValueAvailable));
            }
        }

        private bool IsSelectedDomainSpecial => SelectedDomain == FrameModel.FrameSlotDomain ||
                                                SelectedDomain == FrameModel.TextSlotDomain;


        public bool IsRemoveDomainAvailable => SelectedDomain != null && !IsSelectedDomainSpecial;

        public bool IsAddValueAvailable => SelectedDomain != null && !IsSelectedDomainSpecial;

        public bool IsRemoveValueAvailable => SelectedValue != null && !IsSelectedDomainSpecial;

        public bool IsValuesReadOnly => IsSelectedDomainSpecial;

        public DomainsViewModel(FrameModel frameModel)
        {
            FrameModel = frameModel;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
