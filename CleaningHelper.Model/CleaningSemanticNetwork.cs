using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CleaningHelper.Model
{
    public static class CleaningSemanticNetwork
    {

        public static Concept GetRootSituationFrame(this SemanticNetwork semanticNetwork)
        {
            var situationNameConcepts = semanticNetwork.Concepts.WithName("Ситуация");
            if (situationNameConcepts.Count() != 1)
                throw new Exception("Cleaning semantic network must contain exactly one concept \"Ситуация\"");
            var situationNameConcept = situationNameConcepts.First();
            var situationNode = semanticNetwork.Relations.GetStartConcepts(situationNameConcept)
                                    .FirstOrDefault() ?? throw new Exception("Cleaning ontology has no node that has name \"Ситуация\"");
            return situationNode;
        }

        public static IEnumerable<Concept> GetSituationSlotInstancesConcepts(this SemanticNetwork semanticNetwork, Concept situationFrame)
        {
            var unnamedSlotsConcept =
                semanticNetwork.Relations.WithName("has").GetEndConcepts(situationFrame).FirstOrDefault() ??
                throw new Exception("Situation must have unnamed concept for slots");
            var situationSlotsConcepts = semanticNetwork.Relations.GetEndConcepts(unnamedSlotsConcept);
            return situationSlotsConcepts;
        }

        public static Concept GetSlotByInstance(this SemanticNetwork semanticNetwork, Concept slotInstance)
        {
            return semanticNetwork.Relations.WithName("name").GetEndConcepts(slotInstance).FirstOrDefault() ??
                   throw new Exception("Slot instance node has no name relation");
        }

        public static Concept GetSlotValue(this SemanticNetwork semanticNetwork, Concept slotInstance)
        {
            return semanticNetwork.Relations.WithName("value").GetEndConcepts(slotInstance).FirstOrDefault() ??
                   throw new Exception("Slot instance node has no value relation");
        }
        
        public static IEnumerable<Concept> GetSlotDomainValues(this SemanticNetwork semanticNetwork, Concept slot)
        {
            var slotEndConcepts = semanticNetwork.Relations.WithName("has").GetEndConcepts(slot);
            var unnamedValuesConcept =
                slotEndConcepts.FirstOrDefault(x => semanticNetwork.Concepts.GetUnnamed().Contains(x)) ??
                throw new Exception("Slot has no unnamed node for values");
            return semanticNetwork.Relations.GetEndConcepts(unnamedValuesConcept);
        }
        
        public static bool IsLeafSituation(this SemanticNetwork semanticNetwork, Concept situation)
        {
            return !semanticNetwork.Relations.GetDirectDescendantConcepts(situation).Any();
        }
    }
}