using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CleaningHelper.Model;
using CleaningHelper.ViewModel.Annotations;

namespace CleaningHelper.ViewModel
{
    public class ResultsViewModel : INotifyPropertyChanged
    {
        private List<List<Concept>> _inferringPath;
        private Concept _result;
        public event PropertyChangedEventHandler PropertyChanged;

        public Concept Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public SemanticNetwork SemanticNetwork { get; set; }


        public string ResultView
        {
            get
            {
                var slots_info = SemanticNetwork.GetSituationSlotInstancesConcepts(Result).Where(x =>
                    SemanticNetwork.IsSlotResult(SemanticNetwork.GetSlotByInstance(x))).Select(x =>
                    SemanticNetwork.GetSlotByInstance(x).Name + ": " + SemanticNetwork.GetSlotValue(x).Name);
                return string.Join(Environment.NewLine, slots_info);
            }
        }

        public List<List<Concept>> InferringPath
        {
            get => _inferringPath;
            set
            {
                _inferringPath = value;
                OnPropertyChanged(nameof(InferringPath));
                OnPropertyChanged(nameof(InferringPathView));
            }
        }

        public ObservableCollection<TreeViewItem> InferringPathView
        {
            get
            {
                var result = new ObservableCollection<TreeViewItem>();
                for (var i = 0; i < InferringPath.Count; i++)
                {
                    var tvi = new TreeViewItem() {Header = $"{i + 1}"};
                    result.Add(tvi);
                    foreach (var concept in InferringPath[i])
                    {
                        var situationText = concept.Name + ": " + SemanticNetwork.GetSituationNameConcept(concept).Name;
                        var slotsInfo = SemanticNetwork.GetSituationSlotInstancesConcepts(concept).Select(x =>
                            '\t' + SemanticNetwork.GetSlotByInstance(x).Name + ": " +
                            SemanticNetwork.GetSlotValue(x).Name);
                        situationText += Environment.NewLine + string.Join(Environment.NewLine, slotsInfo);
                        tvi.Items.Add(new TreeViewItem() {Header = situationText});
                    }
                }

                return result;
            }
        }

        public ResultsViewModel(SemanticNetwork semanticNetwork = null, Concept result = null,
            List<List<Concept>> inferringPath = null)
        {
            SemanticNetwork = semanticNetwork;
            Result = result;
            InferringPath = inferringPath ?? new List<List<Concept>>();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}