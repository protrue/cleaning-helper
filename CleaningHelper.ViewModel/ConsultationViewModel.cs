using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleaningHelper.Core;
using CleaningHelper.Model;
using CleaningHelper.ViewModel.Annotations;

namespace CleaningHelper.ViewModel
{
    public class ConsultationViewModel : INotifyPropertyChanged
    {
        private FrameModel _frameModel;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public FrameModel FrameModel
        {
            get => _frameModel;
            set
            {
                _frameModel = value;
                OnPropertyChanged(nameof(FrameModel));
            }
        }

        public DownUpReasoner Reasoner { get; set; }

        public ConsultationViewModel(FrameModel frameModel)
        {
            FrameModel = frameModel;
            Reasoner = new DownUpReasoner(frameModel, new []{"Ингредиент", "Рецепт"});
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
