using Newtonsoft.Json;

namespace CleaningHelper.OntolisAdapter.DataModel
{
    public class OntolisRelation
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("source_node_id")]
        public int SourceNodeId { get; set; }
        
        [JsonProperty("destination_node_id")]
        public int DestinationNodeId { get; set; }

        public override string ToString() => $"#{Id}: {Name} {SourceNodeId} -> {DestinationNodeId}";
    }
}