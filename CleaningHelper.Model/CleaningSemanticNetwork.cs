using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CleaningHelper.Model
{
    public static class CleaningSemanticNetwork
    {

        public static Concept getRootSituationFrame(this SemanticNetwork semanticNetwork)
        {
            var situationNameConcepts = semanticNetwork.Concepts.WithName("Ситуация");
            if (situationNameConcepts.Count() != 1)
                throw new Exception("Cleaning semantic network must contain exactly one concept \"Ситуация\"");
            var situationNameConcept = situationNameConcepts.First();
            var situationNode = semanticNetwork.Relations.GetStartConcepts(situationNameConcept).First();
            return situationNode;
        }

        public static IEnumerable<Concept> getSituationSlotsConcepts(this SemanticNetwork semanticNetwork, Concept situationFrame)
        {
            var unnamedSlotsConcept = semanticNetwork.Relations.WithName("has").GetEndConcepts(situationFrame).First();
            var situationSlotsConcepts = semanticNetwork.Relations.GetEndConcepts(unnamedSlotsConcept);
            return situationSlotsConcepts;
        }

        public static Concept getSlotType(this SemanticNetwork semanticNetwork, Concept slot)
        {
            return semanticNetwork.Relations.WithName("name").GetEndConcepts(slot).First();
        }

        public static Concept getSlotValue(this SemanticNetwork semanticNetwork, Concept slot)
        {
            return semanticNetwork. Relations.WithName("value").GetEndConcepts(slot).First();
        }
        
        public static IEnumerable<Concept> getSlotDomainValues(this SemanticNetwork semanticNetwork, Concept slot)
        {
            var slotEndConcepts = semanticNetwork.Relations.WithName("has").GetEndConcepts(slot);
            var unnamedValuesConcept = slotEndConcepts.First(x => semanticNetwork.Concepts.GetUnnamed().Contains(x));
            return semanticNetwork.Relations.GetEndConcepts(unnamedValuesConcept);
        }
        
        public static bool isLeafSituation(this SemanticNetwork semanticNetwork, Concept situation)
        {
            return !semanticNetwork.Relations.GetDirectDescendantConcepts(situation).Any();
        }
    }
}