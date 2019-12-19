using System;
using System.Collections.Generic;
using System.Linq;
using CleaningHelper.Core;
using CleaningHelper.Model;
using CleaningHelper.OntolisAdapter.Tools;

namespace CleaningHelper.Sandbox
{
    
    internal class Program
    {
        private static Domain[] domains = new[]
        {
            new Domain("Логический", new[]
            {
                new DomainValue("Да"),
                new DomainValue("Нет"),
            }),
            new Domain("Тип ткани", new[]
            {
                new DomainValue("Натуральная"),
                new DomainValue("Синтетика"),
            }),
            new Domain("Цвет ткани", new[]
            {
                new DomainValue("Светлая"),
                new DomainValue("Тёмная"),
            }),
            new Domain("Ткань", new[]
            {
                new DomainValue("Хлопок"),
                new DomainValue("Шёлк"),
                new DomainValue("Лён"),
            }),
            new Domain("Вещество", new[]
            {
                new DomainValue("Жир"),
                new DomainValue("Кровь"),
            }),
            new Domain("Возраст пятна", new[]
            {
                new DomainValue("Свежее"),
                new DomainValue("Старое"),
            }),
            new Domain("Тип пятна", new[]
            {
                new DomainValue("Свежее жирное"),
                new DomainValue("Старое жирное"),
            }),
        };
        private static FrameModel TestFrameModel
        {
            get
            {
                var frames = new[]
                {
                    new Frame("Ткань"),
                    new Frame("Натуральная ткань"),
                    new Frame("Деликатная ткань"),
                    new Frame("Светлый хлопок"),
                    new Frame("Тёмный хлопок"),
                    new Frame("Свежее жирное на светлом хлопке"),
                    new Frame("Старое жирное на светлом хлопке"),
                    new Frame("Старое жирное на тёмном хлопке"),
                };
                
                var frameModel = new FrameModel();
                
                frames[1].Slots.Add(new DomainSlot("Тип ткани", domains[1], domains[1][0]));
                frames[1].Parent = frames[0];
                
                frames[1].Slots.Add(new DomainSlot("Деликатная", domains[0], domains[0][0]));
                frames[2].Parent = frames[1];
                
                frames[3].Slots.Add(new DomainSlot("Цвет ткани", domains[2], domains[2][0]));
                frames[3].Parent = frames[2];
                
                frames[4].Slots.Add(new DomainSlot("Цвет ткани", domains[2], domains[2][1]));
                frames[4].Parent = frames[2];
                
                frames[5].Slots.Add(new DomainSlot("Тип пятна", domains[6], domains[6][0]));
                frames[5].Parent = frames[3];
                
                frames[6].Slots.Add(new DomainSlot("Тип пятна", domains[6], domains[6][1]));
                frames[6].Parent = frames[3];
                
                frames[7].Slots.Add(new DomainSlot("Тип пятна", domains[6], domains[6][1]));
                frames[7].Parent = frames[4];

                foreach (var domain in domains)
                {
                    frameModel.Domains.Add(domain);
                }

                foreach (var frame in frames)
                {
                    frameModel.Frames.Add(frame);
                }

                return frameModel;
            }
        }
        public static void Main(string[] args)
        {
            
            var reasoner = new DownUpReasoner(TestFrameModel, new []{"Ингредиент"});

            var slot = reasoner.GetNextValueToAsk();
            Console.WriteLine(slot + "?");
            reasoner.SetAnswer(domains[6][0]);
            slot = reasoner.GetNextValueToAsk();
            Console.WriteLine(slot + "?");
            
            // while (!reasoner.AnswerFound)
            // {
            //     var slotType = reasoner.GetNextValueToAsk();
            //     
            //     if (!reasoner.AnswerFound)
            //     {
            //         Console.WriteLine(slotType.Name + "? Введите id нужного варианта");
            //         var domainValues = semanticNetwork.GetSlotDomainValues(slotType);
            //         Console.WriteLine(String.Join(", ", domainValues));
            //         
            //         var valueId = int.Parse(Console.ReadLine());
            //         var valueConcept = semanticNetwork.GetConcept(valueId);
            //         reasoner.SetAnswer(valueConcept);
            //     }
            // }
            //
            // var resultConcept = reasoner.GetResultSituation();
            // Console.WriteLine(resultConcept);
            // Console.WriteLine(String.Join(", ", semanticNetwork.GetResultSlotsOfSituation(resultConcept)));
            //
            // foreach (var list in reasoner.InferringPath)
            // {
            //     foreach (var concept in list)
            //     {
            //         Console.Write(concept + " ");
            //     }
            //     Console.WriteLine();
            // }
            //
            // Console.ReadKey();
        }
    }
}