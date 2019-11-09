namespace CleaningHelper.Model
{
    /// <summary>
    /// Понятие, концепт, узел семсети
    /// </summary>
    public class Concept
    {
        public readonly int Identifier;
        
        public string Name { get; set; }
        
        public Concept(int identifier, string name = null)
        {
            Identifier = identifier;
            Name = name;
        }

        public override string ToString() => $"#{Identifier}: {Name}";
    }
}