using System.Collections.Generic;
using System.Linq;

namespace CleaningHelper.Model
{
    public static class SemanticNetworkExtensions
    {
        /// <summary>
        /// Фильтрует список отношений по имени
        /// </summary>
        /// <param name="relations">Список отношений</param>
        /// <param name="name">Имя отношения</param>
        /// <returns>Список отношений с указанным именем</returns>
        public static IEnumerable<Relation> WithName(this IEnumerable<Relation> relations, string name) =>
            relations.Where(r => r.Name == name);

        /// <summary>
        /// Фильтрует список понятий по имени
        /// </summary>
        /// <param name="concepts">Список понятий</param>
        /// <param name="name">Имя понятмя</param>
        /// <returns>Список понятий с указанным именем</returns>
        public static IEnumerable<Concept> WithName(this IEnumerable<Concept> concepts, string name) =>
            concepts.Where(c => c.Name == name);

        /// <summary>
        /// Возвращает неименованные отношения
        /// </summary>
        /// <param name="relations">Список отношений</param>
        /// <returns>Неименованные отношения</returns>
        public static IEnumerable<Relation> GetUnnamed(this IEnumerable<Relation> relations) =>
            relations.WithName("_");

        /// <summary>
        /// Возвращает неименованные понятия
        /// </summary>
        /// <param name="concepts">Список понятий</param>
        /// <returns>Неименованные понятия</returns>
        public static IEnumerable<Concept> GetUnnamed(this IEnumerable<Concept> concepts) =>
            concepts.WithName("_");
        
        /// <summary>
        /// Возвращает список понятий которые являются концами для данного понятия
        /// </summary>
        /// <param name="relations">Список отношений</param>
        /// <param name="concept">Понятие</param>
        /// <returns>Список понятий которые являются концами для данного понятия</returns>
        public static IEnumerable<Concept> GetEndConcepts(this IEnumerable<Relation> relations, Concept concept) =>
            relations.Where(r => ReferenceEquals(r.FirstConcept, concept)).Select(r => r.SecondConcept);

        /// <summary>
        /// Возвращает список понятий, которые являются началом для данного понятия
        /// </summary>
        /// <param name="relations">Список отношений</param>
        /// <param name="concept">Понятие</param>
        /// <returns>Список понятий которые являются началом для данного понятия</returns>
        public static IEnumerable<Concept> GetStartConcepts(this IEnumerable<Relation> relations, Concept concept) =>
            relations.Where(r => ReferenceEquals(r.SecondConcept, concept)).Select(r => r.FirstConcept);

        public static IEnumerable<Concept> GetDirectDescendantConcepts(this IEnumerable<Relation> relations, Concept concept)
            => relations.Where(r => ReferenceEquals(r.SecondConcept, concept) && r.Name == "is_a").Select(r => r.FirstConcept);
        
    }
}