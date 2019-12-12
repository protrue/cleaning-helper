using System;
using System.Collections.Generic;
using System.Linq;
using CleaningHelper.Model;

namespace CleaningHelper.Core
{
    public class Reasoner
    {
        private readonly SemanticNetwork _semanticNetwork;
        private readonly List<List<Concept>> _inferringPath = new List<List<Concept>>();
        private List<Concept> _currentLevelCandidates;
        
        private readonly Dictionary<String, String> _memory = new Dictionary<string, string>();

        private string _askValue;

        public bool AnswerFound;
        public bool AnswerIsLeaf;


        public Reasoner(SemanticNetwork semanticNetwork)
        {
            _semanticNetwork = semanticNetwork;
            _inferringPath.Add(new List<Concept>());
            _currentLevelCandidates = new List<Concept> {_semanticNetwork.getRootSituationFrame()};
        }

        private List<Concept> GetDescendantConcepts(Concept concept)
        {
            var descendants = _semanticNetwork.Relations.GetDirectDescendantConcepts(concept);
            return new List<Concept>(descendants);
        }

        public string GetNextValueToAsk()
        {
            _askValue = null;
            
            while (!AnswerFound && _askValue == null)
            {
                foreach (var candidate in _currentLevelCandidates)
                {
                    _inferringPath.Last().Add(candidate);
                    if (_semanticNetwork.isLeafSituation(candidate))
                    {
                        AnswerFound = true;
                        AnswerIsLeaf = true;
                        break;
                    }
                    
                    var candidateSuits = true;
                    foreach (var slot in _semanticNetwork.getSituationSlotsConcepts(candidate))
                    {
                        string slotName = _semanticNetwork.getSlotName(slot);
                        string slotValue = _semanticNetwork.getSlotValue(slot);
                
                        if (!_memory.ContainsKey(slotName))
                        {
                            _askValue = slotName;
                            break;
                        }

                        if (_memory[slotName] != slotValue)
                        {
                            candidateSuits = false;
                            break;
                        }
                    }
                    
                    if (_askValue != null)
                        break;

                    if (candidateSuits)
                    {
                        _currentLevelCandidates = GetDescendantConcepts(candidate);
                        _inferringPath.Add(new List<Concept>());
                        break;
                    }
                }   
                if (_askValue != null || AnswerFound)
                    break;
                
                // Если дошли до сюда, значит, ни один кандидат не подошел, 
                // выдаем рекомендацию из текущей вершины
                AnswerIsLeaf = false;
            }

            return _askValue;
        }

        public void SetAnswer(string ans)
        {
            _memory[_askValue] = ans;
        }

        public Concept GetResultSituation()
        {
            if (!AnswerFound) return null;
            if (AnswerIsLeaf)
                return _inferringPath.Last().Last();
            return _inferringPath[_inferringPath.Count - 1].Last();

        }
    }
}