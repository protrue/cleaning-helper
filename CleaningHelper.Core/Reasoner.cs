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
        
        private readonly Dictionary<Concept, Concept> _memory = new Dictionary<Concept, Concept>();

        private Concept _askSlot;

        public bool AnswerFound;
        public bool AnswerIsLeaf;


        public Reasoner(SemanticNetwork semanticNetwork)
        {
            _semanticNetwork = semanticNetwork;
            _inferringPath.Add(new List<Concept>());
            _currentLevelCandidates = new List<Concept> {_semanticNetwork.GetRootSituationFrame()};
        }

        private List<Concept> GetDescendantConcepts(Concept concept)
        {
            var descendants = _semanticNetwork.Relations.GetDirectDescendantConcepts(concept);
            return new List<Concept>(descendants);
        }

        public Concept GetNextValueToAsk()
        {
            _askSlot = null;
            
            while (!AnswerFound && _askSlot == null)
            {
                var foundCandidate = false;
                foreach (var candidate in _currentLevelCandidates)
                {
                    _inferringPath.Last().Add(candidate);
                    if (_semanticNetwork.IsLeafSituation(candidate))
                    {
                        AnswerFound = true;
                        AnswerIsLeaf = true;
                        break;
                    }
                    
                    var candidateSuits = true;
                    foreach (var slotInstance in _semanticNetwork.GetSituationSlotInstancesConcepts(candidate))
                    {
                        Concept slot = _semanticNetwork.GetSlotByInstance(slotInstance);
                        Concept slotValue = _semanticNetwork.GetSlotValue(slotInstance);
                
                        if (_semanticNetwork.IsSlotResult(slot))
                            continue;
                        
                        if (!_memory.ContainsKey(slot))
                        {
                            _askSlot = slot;
                            break;
                        }

                        if (_memory[slot] != slotValue)
                        {
                            candidateSuits = false;
                            break;
                        }
                    }
                    
                    if (_askSlot != null)
                        break;

                    if (candidateSuits)
                    {
                        _currentLevelCandidates = GetDescendantConcepts(candidate);
                        _inferringPath.Add(new List<Concept>());
                        foundCandidate = true;
                        break;
                    }
                }
                if (_askSlot != null || AnswerFound)
                    break;
                
                if (foundCandidate)
                    continue;
                
                // Если дошли до сюда, значит, ни один кандидат не подошел, 
                // выдаем рекомендацию из текущей вершины
                AnswerFound = true;
                AnswerIsLeaf = false;
            }

            return _askSlot;
        }

        public void SetAnswer(Concept value)
        {
            _memory[_askSlot] = value;
        }

        public Concept GetResultSituation()
        {
            if (!AnswerFound) return null;
            if (AnswerIsLeaf)
                return _inferringPath.Last().Last();
            return _inferringPath[_inferringPath.Count - 1].Last();

        }
        
        public Concept GetLowestSituationWithResultSlots()
        {
            if (!AnswerFound) return null;
            var startIndex = AnswerIsLeaf ? _inferringPath.Count - 1 : _inferringPath.Count - 2;
            
            while (startIndex > 0)
            {
                if (_semanticNetwork.GetResultSlotsOfSituation(_inferringPath[startIndex].Last()).Any())
                {
                    startIndex--;
                }
            }
            
            return _inferringPath[startIndex].Last();

        }

        public List<List<Concept>> InferringPath
        {
            get => _inferringPath;
        }
    }
}