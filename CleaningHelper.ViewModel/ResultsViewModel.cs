﻿using System;
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
                return string.Join(Environment.NewLine, SemanticNetwork.GetResultSlotsOfSituation(Result));
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
                    //var node = new Node { Name = $"{i + 1}" };
                    var tvi = new TreeViewItem() {Header = $"{i + 1}"};
                    result.Add(tvi);
                    foreach (var concept in InferringPath[i])
                    {
                        tvi.Items.Add(new TreeViewItem() {Header = concept.Name});
                        //node.Nodes.Add(new Node { Name = concept.Name });
                    }
                }

                return result;
            }
        }

        public ResultsViewModel(SemanticNetwork semanticNetwork = null, Concept result = null, List<List<Concept>> inferringPath = null)
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
