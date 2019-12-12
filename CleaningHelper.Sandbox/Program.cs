using System;
using CleaningHelper.Core;
using CleaningHelper.OntolisAdapter.Tools;

namespace CleaningHelper.Sandbox
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var ontolisDataObject = OntolisFileDeserializer.DeserializeOntolisFile("cleaning.ont");
            Console.WriteLine(ontolisDataObject);
            var semanticNetwork = OntolisDataConverter.Convert(ontolisDataObject);
            Console.WriteLine(semanticNetwork);

            var reasoner = new Reasoner(semanticNetwork);
            while (!reasoner.AnswerFound)
            {
                Console.WriteLine(reasoner.GetNextValueToAsk());
                if (!reasoner.AnswerFound)
                    reasoner.SetAnswer(Console.ReadLine());
            }

            var resultConcept = reasoner.GetResultSituation();
            Console.WriteLine(resultConcept);
        }
    }
}