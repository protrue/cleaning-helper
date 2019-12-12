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
            _currentLevelCandidates = new List<Concept> {_semanticNetwork.getRootSituationFrame()};
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
                    if (_semanticNetwork.isLeafSituation(candidate))
                    {
                        AnswerFound = true;
                        AnswerIsLeaf = true;
                        break;
                    }
                    
                    var candidateSuits = true;
                    foreach (var slot in _semanticNetwork.getSituationSlotsConcepts(candidate))
                    {
                        Concept slotType = _semanticNetwork.getSlotType(slot);
                        Concept slotValue = _semanticNetwork.getSlotValue(slot);
                
                        if (!_memory.ContainsKey(slotType))
                        {
                            _askSlot = slotType;
                            break;
                        }

                        if (_memory[slotType] != slotValue)
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

        public List<List<Concept>> InferringPath
        {
            get => _inferringPath;
        }
    }
}