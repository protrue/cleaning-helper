using System;
using System.Collections.Generic;
using System.Linq;

namespace CleaningHelper.Model
{
    /// <summary>
    /// Семантическая сеть
    /// </summary>
    public class SemanticNetwork
    {
        private List<Concept> _concepts;
        private List<Relation> _relations;
        
        /// <summary>
        /// Список понятий
        /// </summary>
        public IReadOnlyCollection<Concept> Concepts => _concepts.ToArray();
        
        /// <summary>
        /// Список отношений между понятиями
        /// </summary>
        public IReadOnlyCollection<Relation> Relations => _relations.ToArray();

        public SemanticNetwork()
        {
            _concepts = new List<Concept>();
            _relations = new List<Relation>();
        }

        /// <summary>
        /// Возвращает идентификатор, который можно присвоить добавляемому понятию
        /// </summary>
        /// <returns>Идентификатор, который можно присвоить добавляемому понятию</returns>
        public int GetNextConceptIdentifier()
        {
            var result = 0;
            if (_concepts.Count > 0)
                result = _concepts.Select(c => c.Identifier).Max() + 1;
            return result;
        }
        
        /// <summary>
        /// Возвращает идентификатор, который можно присвоить добавляемому отношению
        /// </summary>
        /// <returns>Идентификатор, который можно присвоить добавляемому отношению</returns>
        public int GetNextRelationIdentifier()
        {
            var result = 0;
            if (_relations.Count > 0)
                result = _relations.Select(r => r.Identifier).Max() + 1;
            return result;
        }

        /// <summary>
        /// Проверяет наличие понятия в семсети
        /// </summary>
        /// <param name="concept">Проверяемое понятие</param>
        /// <returns>true, если понятие присутствует в семсети, false иначе</returns>
        public bool ContainsConcept(Concept concept) => _concepts.Any(c => c.Identifier == concept.Identifier);

        /// <summary>
        /// Возвращает понятие по идентификатору
        /// </summary>
        /// <param name="identifier">Идентификатор</param>
        /// <returns>Понятие</returns>
        public Concept GetConcept(int identifier) => _concepts.FirstOrDefault(c => c.Identifier == identifier);

        /// <summary>
        /// Возвращает понятие по имени
        /// </summary>
        /// <param name="name">Имя понятия</param>
        /// <returns>Понятие</returns>
        public Concept GetConcept(string name) => _concepts.FirstOrDefault(c => c.Name == name);

        /// <summary>
        /// Проверяет наличие отношения в семсети
        /// </summary>
        /// <param name="relation">Проверяемое отношение</param>
        /// <returns>true, если отношение присутствует в семсети, false иначе</returns>
        public bool ContainsRelation(Relation relation) => _relations.Any(r => r.Identifier == relation.Identifier);

        /// <summary>
        /// Возвращает отношение по идентификатору
        /// </summary>
        /// <param name="identifier">Идентификатор</param>
        /// <returns>Отношение</returns>
        public Relation GetRelation(int identifier) => _relations.FirstOrDefault(r => r.Identifier == identifier);

        /// <summary>
        /// Возвращает отношение по имени
        /// </summary>
        /// <param name="name">Имя</param>
        /// <returns>Отношение</returns>
        public Relation GetRelation(string name) => _relations.FirstOrDefault(r => r.Name == name);

        /// <summary>
        /// Добавляет понятие в семсеть
        /// </summary>
        /// <param name="concept">Понятие</param>
        /// <returns>Добалвенное понятие</returns>
        /// <exception cref="ArgumentException"></exception>
        public Concept AddConcept(Concept concept)
        {
            if (ContainsConcept(concept))
                throw new ArgumentException("Семсеть уже содержит понятие с таким идентификатором", nameof(concept));

            _concepts.Add(concept);

            return concept;
        }

        /// <summary>
        /// Добавляет понятие в семсеть
        /// </summary>
        /// <param name="name">Имя понятия</param>
        /// <returns>Добавленное понятие</returns>
        public Concept AddConcept(string name)
        {
            var concept = new Concept(GetNextConceptIdentifier(), name);
            var addedConcept = AddConcept(concept);
            return addedConcept;
        }

        /// <summary>
        /// Добавляет отношение в семсеть
        /// </summary>
        /// <param name="relation">Отношение</param>
        /// <returns>Добавленное отношение</returns>
        /// <exception cref="ArgumentException"></exception>
        public Relation AddRelation(Relation relation)
        {
            if (ContainsRelation(relation))
                throw new ArgumentException("Семсеть уже содержит отношение с таким идентификатором", nameof(relation));

            var firstExistingConcept = GetConcept(relation.FirstConcept.Identifier);
            var secondExistingConcept = GetConcept(relation.SecondConcept.Identifier);
            if (!ReferenceEquals(firstExistingConcept, relation.FirstConcept) ||
                !ReferenceEquals(secondExistingConcept, relation.SecondConcept))
                throw new ArgumentException("Понятия в добавляемом отношении не равны понятиям, которые содержит семсеть", nameof(relation));

            _relations.Add(relation);

            return relation;
        }

        /// <summary>
        /// Добавляет отношение в семсеть
        /// </summary>
        /// <param name="relationName">Имя отношения</param>
        /// <param name="firstConcept">Понятие, из которого исходит отнношение</param>
        /// <param name="secondConcept">Понятие, в которое входит отношение</param>
        /// <returns>Добавленное отношение</returns>
        public Relation AddRelation(string relationName, Concept firstConcept, Concept secondConcept)
        {
            var relationIdentifier = GetNextRelationIdentifier();
            var relation = new Relation(relationIdentifier, relationName, firstConcept, secondConcept);
            return AddRelation(relation);
        }

        /// <summary>
        /// Возвращает список всех отношений, связанных с данным понятием
        /// </summary>
        /// <param name="concept">Понятие</param>
        /// <returns>Список отношений</returns>
        public IEnumerable<Relation> GetRelations(Concept concept) =>
            _relations.Where(r => ReferenceEquals(r.FirstConcept, concept)
                                  || ReferenceEquals(r.SecondConcept, concept));

        /// <summary>
        /// Получает список отношений, которые исходят из данного понятия
        /// </summary>
        /// <param name="concept">Понятие</param>
        /// <returns>Список исходящих отношений</returns>
        public IEnumerable<Relation> GetOutgoingRelations(Concept concept) =>
            GetRelations(concept)
                .Where(r => ReferenceEquals(r.FirstConcept, concept));

        /// <summary>
        /// Возвращет список отношений, которые входят в данное понятие
        /// </summary>
        /// <param name="concept">Понятие</param>
        /// <returns>Список входящих отношшений</returns>
        public IEnumerable<Relation> GetIncomingTo(Concept concept) =>
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