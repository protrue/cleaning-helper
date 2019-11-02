using System.Collections.Generic;
using CleaningHelper.Model;
using CleaningHelper.OntolisAdapter.DataModel;

namespace CleaningHelper.OntolisAdapter.Tools
{
    public static class OntolisDataConverter
    {
        public static SemanticNetwork Convert(OntolisDataObject ontolisDataObject)
        {
            var semanticNetwork = new SemanticNetwork();

            var conceptsDictionary = new Dictionary<int, Concept>();
            
            foreach (var ontolisNode in ontolisDataObject.OntolisNodes)
            {
                var addedConcept = semanticNetwork.AddConcept(ontolisNode.Name);
                conceptsDictionary[ontolisNode.Id] = addedConcept;
            }

            foreach (var ontolisRelation in ontolisDataObject.OntolisRelations)
            {
                var firstConcept = conceptsDictionary[ontolisRelation.SourceNodeId];
                var secontConcept = conceptsDictionary[ontolisRelation.DestinationNodeId];
                semanticNetwork.AddRelation(ontolisRelation.Name, firstConcept, secontConcept);
            }

            return semanticNetwork;
        }
    }
}