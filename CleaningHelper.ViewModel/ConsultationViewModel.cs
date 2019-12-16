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
        private string _questionText;
        private ObservableCollection<Concept> _answersList = new ObservableCollection<Concept>();
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

        public ObservableCollection<Concept> AnswersList
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


        public Command SetQuestionCommand => new Command(parameter =>
        {
            var slotType = Reasoner.GetNextValueToAsk();

            if (Reasoner.AnswerFound)
            {
                Result = Reasoner.GetResultSituation();
                return;
            }

            QuestionText = $"{slotType.Name}?";
            AnswersList.Clear();
            foreach (var slotDomainValue in SemanticNetwork.GetSlotDomainValues(slotType))
            {
                AnswersList.Add(slotDomainValue);
            }
        });

        public Command SetAnswerCommand => new Command(parameter =>
        {
            if (SelectedAnswer == null || Reasoner.AnswerFound)
                return;

            var valueId = SelectedAnswer.Identifier;
            var valueConcept = SemanticNetwork.GetConcept(valueId);
            Reasoner.SetAnswer(valueConcept);

            if (!Reasoner.AnswerFound)
                SetQuestionCommand.Execute();
            else
                Result = Reasoner.GetResultSituation();
        });

        public Command SetResultCommand => new Command(parameter => { });

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
