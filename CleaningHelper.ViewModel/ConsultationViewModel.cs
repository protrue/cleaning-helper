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
        private string _questionText;
        private ObservableCollection<DomainValue> _answersList = new ObservableCollection<DomainValue>();
        private DomainValue _selectedAnswer;
        private Frame _result;
        
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
        
        public Frame Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
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
        
        public string QuestionText
        {
            get => _questionText;
            set
            {
                _questionText = value;
                OnPropertyChanged(nameof(QuestionText));
            }
        }
        
        public ObservableCollection<DomainValue> AnswersList
        {
            get => _answersList;
            set
            {
                _answersList = value;
                OnPropertyChanged(nameof(AnswersList));
            }
        }
        
        public DomainValue SelectedAnswer
        {
            get => _selectedAnswer;
            set
            {
                _selectedAnswer = value;
                OnPropertyChanged(nameof(SelectedAnswer));
            }
        }
        
        public Command SetQuestionCommand => new Command(parameter =>
        {
            var slot = Reasoner.GetNextValueToAsk();

            if (Reasoner.AnswerFound)
            {
                Result = Reasoner.GetAnswer();
                return;
            }

            QuestionText = $"{slot.Name}?";
            AnswersList.Clear();
            foreach (var slotDomainValue in slot.Domain.Values)
            {
                AnswersList.Add(slotDomainValue);
            }

            SelectedAnswer = AnswersList.FirstOrDefault();
        });
    }
}
