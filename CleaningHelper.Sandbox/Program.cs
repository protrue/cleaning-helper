using System;
using System.Collections.Generic;
using System.Linq;
using CleaningHelper.Core;
using CleaningHelper.Model;

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
                    new Frame("Жирное пятно"),
                    new Frame("Свежее жирное пятно"),
                    new Frame("Старое жирное пятно"),
                };
                
                var frameModel = new FrameModel();
                
                frames[1].Slots.Add(new DomainSlot("Тип ткани", domains[1], domains[1][0]));
                frames[1].Parent = frames[0];
                
                frames[2].Slots.Add(new DomainSlot("Деликатная", domains[0], domains[0][0]));
                frames[2].Parent = frames[1];
                
                frames[3].Slots.Add(new DomainSlot("Цвет ткани", domains[2], domains[2][0], false, true));
                frames[3].Slots.Add(new DomainSlot("Ткань", domains[3], domains[3][0], false, true));
                frames[3].Parent = frames[2];
                
                frames[4].Slots.Add(new DomainSlot("Цвет ткани", domains[2], domains[2][1], false, true));
                frames[4].Slots.Add(new DomainSlot("Ткань", domains[3], domains[3][0], false, true));
                frames[4].Parent = frames[2];
                
                frames[8].Slots.Add(new DomainSlot("Вещество", domains[4], domains[4][0], false, true));
                
                frames[9].Slots.Add(new DomainSlot("Возраст пятна", domains[5], domains[5][0], false, true));
                frames[9].Parent = frames[8];
                frames[10].Slots.Add(new DomainSlot("Возраст пятна", domains[5], domains[5][1], false, true));
                frames[10].Parent = frames[8];
                
                frames[5].Slots.Add(new FrameSlot("Тип пятна", frames[9]));
                frames[5].Parent = frames[3];
                
                frames[6].Slots.Add(new FrameSlot("Тип пятна", frames[10]));
                frames[6].Parent = frames[3];
                
                frames[7].Slots.Add(new FrameSlot("Тип пятна", frames[10]));
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

            while (true)
            {
                var slot = reasoner.GetNextValueToAsk();
                if (slot == null)
                    break;
                Console.WriteLine(slot + "?");
                Console.WriteLine(String.Join(", ", slot.Domain.Values.Select(x => x.Text)));
                var valId = int.Parse(Console.ReadLine());
                reasoner.SetAnswer(slot.Domain.Values[valId]);
            }
            
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