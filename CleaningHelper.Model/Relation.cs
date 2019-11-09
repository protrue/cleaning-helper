using System;

namespace CleaningHelper.Model
{
    /// <summary>
    /// Отношение между понятиями, дуга семсети
    /// </summary>
    public class Relation
    {
        public readonly int Identifier;
        private Concept _firstConcept;
        private Concept _secondConcept;

        public string Name { get; set; }

        public Concept FirstConcept
        {
            get => _firstConcept;
            set => _firstConcept = value ?? throw new ArgumentNullException();
        }

        public Concept SecondConcept
        {
            get => _secondConcept;
            set => _secondConcept = value ?? throw new ArgumentNullException();
        }

        public Relation(int identifier, string name, Concept firstConcept, Concept secondConcept)
        {
            Identifier = identifier;
            Name = name;
            FirstConcept = firstConcept;
            SecondConcept = secondConcept;
        }

        public override string ToString() => $"#{Identifier}: {FirstConcept.Name} {Name} {SecondConcept.Name}";
    }
}