using System;
using System.Collections.Generic;
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
        private Mutex _mutex = new Mutex();
        private string _questionText;
        private List<Concept> _answersList;
        private Concept _selectedAnswer;
        private Concept _result;

        public event PropertyChangedEventHandler PropertyChanged;

        public SemanticNetwork SemanticNetwork { get; }

        public Reasoner Reasoner { get; set; }

        public string QuestionText
        {
            get => _questionText;
            set
            {
                _questionText = value;
                OnPropertyChanged(nameof(QuestionText));
            }
        }

        public List<Concept> AnswersList
        {
            get => _answersList;
            set
            {
                _answersList = value;
                OnPropertyChanged(nameof(AnswersList));
            }
        }

        public Concept SelectedAnswer
        {
            get => _selectedAnswer;
            set
            {
                _selectedAnswer = value;
                OnPropertyChanged(nameof(SelectedAnswer));
            }
        }

        public Concept Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public ConsultationViewModel(SemanticNetwork semanticNetwork)
        {
            SemanticNetwork = semanticNetwork;
            Reasoner = new Reasoner(semanticNetwork);
        }

        private Task<int> GetCurrentAnswer()
        {
            _mutex.WaitOne();
            return new Task<int>(() => SelectedAnswer.Identifier);
        }

        private async void Consult()
        {
            while (!Reasoner.AnswerFound)
            {
                var slotType = Reasoner.GetNextValueToAsk();

                if (!Reasoner.AnswerFound)
                {
                    QuestionText = $"{slotType.Name}?";
                    AnswersList = SemanticNetwork.GetSlotDomainValues(slotType).ToList();

                    var valueId = await GetCurrentAnswer();
                    var valueConcept = SemanticNetwork.GetConcept(valueId);
                    Reasoner.SetAnswer(valueConcept);
                }
            }

            Result = Reasoner.GetResultSituation();
        }

        public Command SelectAnswerCommand => new Command(parameter =>
        {
            _mutex.ReleaseMutex();
        });

        public Command ConsultCommand => new Command(parameter =>
        {
            var consultTask = new Task(Consult);
            consultTask.Start();
        });

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
