using System.Collections.Generic;
using System.Linq;

namespace CleaningHelper.Model
{
    public static class SemanticNetworkExtensions
    {
        public static IEnumerable<Relation> WithName(this IEnumerable<Relation> relations, string name) =>
            relations.Where(r => r.Name == name);

        public static IEnumerable<Concept> GetConceptsFrom(this IEnumerable<Relation> relations, Concept concept) =>
            relations.Where(r => ReferenceEquals(r.FirstConcept, concept)).Select(r => r.SecondConcept);

        public static IEnumerable<Concept> GetConceptsTo(this IEnumerable<Relation> relations, Concept concept) =>
            relations.Where(r => ReferenceEquals(r.SecondConcept, concept)).Select(r => r.FirstConcept);
    }
}