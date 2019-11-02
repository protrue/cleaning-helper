using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CleaningHelper.OntolisAdapter.DataModel
{
    public class OntolisDataObject
    {
        [JsonProperty("last_id")]
        public int LastId { get; set; }
        
        [JsonProperty("nodes")]
        public IEnumerable<OntolisNode> OntolisNodes { get; set; }
        
        [JsonProperty("relations")]
        public IEnumerable<OntolisRelation> OntolisRelations { get; set; }

        public OntolisDataObject()
        {
            OntolisNodes = new List<OntolisNode>();
            OntolisRelations = new List<OntolisRelation>();
        }

        public override string ToString() =>
            $"{LastId}" +
            $"{Environment.NewLine}{string.Join(Environment.NewLine, OntolisNodes)}" +
            $"{Environment.NewLine}{string.Join(Environment.NewLine, OntolisRelations)}";
    }
}