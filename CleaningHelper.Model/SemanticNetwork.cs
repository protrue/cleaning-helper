using System;
using System.Collections.Generic;
using System.Linq;

namespace CleaningHelper.Model
{
    public class SemanticNetwork
    {
        private List<Concept> _concepts;
        private List<Relation> _relations;

        public IReadOnlyCollection<Concept> Concepts => _concepts.ToArray();
        public IReadOnlyCollection<Relation> Relations => _relations.ToArray();

        public SemanticNetwork()
        {
            _concepts = new List<Concept>();
            _relations = new List<Relation>();
        }

        private int GetNextConceptIdentifier()
        {
            var result = 0;
            if (_concepts.Count > 0)
                result = _concepts.Select(c => c.Identifier).Max() + 1;
            return result;
        }

        private int GetNextRelationIdentifier()
        {
            var result = 0;
            if (_relations.Count > 0)
                result = _relations.Select(r => r.Identifier).Max() + 1;
            return result;
        }

        public bool ContainsConcept(Concept concept) => _concepts.Any(c => c.Identifier == concept.Identifier);

        public Concept GetConcept(int identifier) => _concepts.FirstOrDefault(c => c.Identifier == identifier);

        public Concept GetConcept(string name) => _concepts.FirstOrDefault(c => c.Name == name);

        public bool ContainsRelation(Relation relation) => _relations.Any(r => r.Identifier == relation.Identifier);

        public Relation GetRelation(int identifier) => _relations.FirstOrDefault(r => r.Identifier == identifier);

        public Relation GetRelation(string name) => _relations.FirstOrDefault(r => r.Name == name);

        public Concept AddConcept(Concept concept)
        {
            if (ContainsConcept(concept))
                throw new ArgumentException("Network already contains concept with same identifier");

            _concepts.Add(concept);

            return concept;
        }

        public Concept AddConcept(string name)
        {
            var concept = new Concept(GetNextConceptIdentifier(), name);
            var addedConcept = AddConcept(concept);
            return addedConcept;
        }

        public Relation AddRelation(Relation relation)
        {
            if (ContainsRelation(relation))
                throw new ArgumentException("Network already contains relation with same identifier");

            var firstExistingConcept = GetConcept(relation.FirstConcept.Identifier);
            var secondExistingConcept = GetConcept(relation.SecondConcept.Identifier);
            if (!object.ReferenceEquals(firstExistingConcept, relation.FirstConcept) ||
                !object.ReferenceEquals(secondExistingConcept, relation.SecondConcept))
                throw new ArgumentException("Concepts in the relation are not the same with existing concepts",
                    nameof(relation));

            _relations.Add(relation);

            return relation;
        }

        public Relation AddRelation(string relationName, Concept firstConcept, Concept secondConcept)
        {
            var relationIdentifier = GetNextRelationIdentifier();
            var relation = new Relation(relationIdentifier, relationName, firstConcept, secondConcept);
            return AddRelation(relation);
        }

        public IEnumerable<Relation> GetRelations(Concept concept) =>
            _relations.Where(r => ReferenceEquals(r.FirstConcept, concept)
                                  || ReferenceEquals(r.SecondConcept, concept));

        public IEnumerable<Relation> GetRelationsFrom(Concept concept) =>
            GetRelations(concept)
                .Where(r => ReferenceEquals(r.FirstConcept, concept));

        public IEnumerable<Relation> GetRelationsTo(Concept concept) =>
            GetRelations(concept)
                .Where(r => ReferenceEquals(r.SecondConcept, concept));

        public override string ToString()
        {
            return
                $"{string.Join(Environment.NewLine, _concepts)}" +
                $"{Environment.NewLine}" +
                $"{string.Join(Environment.NewLine, _relations)}";
        }
    }
}