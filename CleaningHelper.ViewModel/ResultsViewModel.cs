using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CleaningHelper.Model;
using CleaningHelper.ViewModel.Annotations;

namespace CleaningHelper.ViewModel
{
    public class ResultsViewModel : INotifyPropertyChanged
    {
        private List<List<Concept>> _inferringPath;
        public event PropertyChangedEventHandler PropertyChanged;

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

        public ObservableCollection<Node> InferringPathView
        {
            get
            {
                var result = new ObservableCollection<Node>();
                for (var i = 0; i < InferringPath.Count; i++)
                {
                    var node = new Node { Name = $"{i + 1}" };
                    result.Add(node);
                    foreach (var concept in InferringPath[i])
                    {
                        node.Nodes.Add(new Node { Name = concept.Name });
                    }
                }

                return result;
            }
        }

        public ResultsViewModel(List<List<Concept>> inferringPath = null)
        {
            InferringPath = inferringPath ?? new List<List<Concept>>();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
