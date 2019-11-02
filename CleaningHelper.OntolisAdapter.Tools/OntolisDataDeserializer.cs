using System.IO;
using CleaningHelper.OntolisAdapter.DataModel;
using Newtonsoft.Json;

namespace CleaningHelper.OntolisAdapter.Tools
{
    public static class OntolisFileDeserializer
    {
        public static OntolisDataObject DeserializeOntolisJson(string json)
        {
            return JsonConvert.DeserializeObject<OntolisDataObject>(json);
        }

        public static OntolisDataObject DeserializeOntolisFile(string filePath)
        {
            var fileContent =  File.ReadAllText(filePath);
            return DeserializeOntolisJson(fileContent);
        }
    }
}