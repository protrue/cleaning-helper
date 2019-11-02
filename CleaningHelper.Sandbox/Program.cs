using System;
using CleaningHelper.OntolisAdapter.Tools;

namespace CleaningHelper.Sandbox
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var ontolisDataObject = OntolisFileDeserializer.DeserializeOntolisFile("trans.ont");
            Console.WriteLine(ontolisDataObject);
            var semanticNetwork = OntolisDataConverter.Convert(ontolisDataObject);
            Console.WriteLine(semanticNetwork);
        }
    }
}