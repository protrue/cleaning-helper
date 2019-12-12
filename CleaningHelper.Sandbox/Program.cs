using System;
using System.Linq;
using CleaningHelper.Core;
using CleaningHelper.Model;
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
                var slotType = reasoner.GetNextValueToAsk();
                
                if (!reasoner.AnswerFound)
                {
                    Console.WriteLine(slotType.Name + "? Введите id нужного варианта");
                    var domainValues = semanticNetwork.GetSlotDomainValues(slotType);
                    Console.WriteLine(String.Join(", ", domainValues));
                    
                    var valueId = int.Parse(Console.ReadLine());
                    var valueConcept = semanticNetwork.GetConcept(valueId);
                    reasoner.SetAnswer(valueConcept);
                }
            }

            var resultConcept = reasoner.GetResultSituation();
            Console.WriteLine(resultConcept);
        }
    }
}