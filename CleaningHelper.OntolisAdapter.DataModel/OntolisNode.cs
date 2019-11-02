using Newtonsoft.Json;

namespace CleaningHelper.OntolisAdapter.DataModel
{
    public class OntolisNode
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("position_x")]
        public double PositionX { get; set; }
        
        [JsonProperty("position_y")]
        public double PositionY { get; set; }

        public override string ToString() => $"#{Id}: {Name} X: {PositionX} Y: {PositionY}";
    }
}