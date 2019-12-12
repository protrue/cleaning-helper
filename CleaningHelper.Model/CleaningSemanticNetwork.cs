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
            return semanticNetwork.Relations.GetEndConcepts(unnamedSlotsConcept);
        }

        public static string getSlotName(this SemanticNetwork semanticNetwork, Concept slot)
        {
            return semanticNetwork.Relations.WithName("name").GetEndConcepts(slot).First().Name;
        }

        public static string getSlotValue(this SemanticNetwork semanticNetwork, Concept slot)
        {
            return semanticNetwork. Relations.WithName("value").GetEndConcepts(slot).First().Name;
        }
        
        public static bool isLeafSituation(this SemanticNetwork semanticNetwork, Concept situation)
        {
            return !semanticNetwork.Relations.GetDirectDescendantConcepts(situation).Any();
        }
    }
}