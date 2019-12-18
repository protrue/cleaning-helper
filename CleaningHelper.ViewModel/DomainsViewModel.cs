using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CleaningHelper.Model;
using CleaningHelper.ViewModel.Annotations;

namespace CleaningHelper.ViewModel
{
    public class DomainsViewModel : INotifyPropertyChanged
    {
        private FrameSlotDomain _selectedDomain;
        private FrameSlotDomainValue _selectedValue;
        public event PropertyChangedEventHandler PropertyChanged;

        public FrameModel FrameModel { get; set; }

        public FrameSlotDomain SelectedDomain
        {
            get => _selectedDomain;
            set
            {
                _selectedDomain = value;
                OnPropertyChanged(nameof(SelectedDomain));
            }
        }

        public FrameSlotDomainValue SelectedValue
        {
            get => _selectedValue;
            set
            {
                _selectedValue = value; 
                OnPropertyChanged(nameof(SelectedValue));
            }
        }

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
