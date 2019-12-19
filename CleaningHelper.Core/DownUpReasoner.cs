using System;
using System.Collections.Generic;
using System.Linq;
using CleaningHelper.Model;

namespace CleaningHelper.Core
{
    public class DownUpReasoner
    {
        private readonly FrameModel _model;
        private readonly IEnumerable<string> _goalSlotNames;
        private Dictionary<String, DomainValue> _memory = new Dictionary<string, DomainValue>();

        private Frame _bindingCandidate = null;
        private List<Frame> _bindingStack = new List<Frame>();
        
        private Dictionary<Frame, bool> _bindedFrames = new Dictionary<Frame, bool>(); 

        public DownUpReasoner(FrameModel model, IEnumerable<String> goalSlotNames)
        {
            _model = model;
            _goalSlotNames = goalSlotNames;
        }

        public string GetNextValueToAsk()
        {
            if (_bindingCandidate == null)
            {
                _bindingCandidate = _getFirstCandidate();
                Console.WriteLine("First candidate is ", _bindingCandidate);
                
                _bindingStack.Add(_bindingCandidate);
            }

            if (_bindingStack.Count == 0)
            {
                Console.WriteLine("The binding stack is empty");
                return "";
            }


            var topFrameSlotsSuits = checkSlotsSuits(_bindingStack.Last());
            if (topFrameSlotsSuits == true)
            {   
                _bindedFrames[_bindingStack.Last()] = true;
                _bindingStack.RemoveAt(_bindingStack.Count - 1);
            }
            else if (topFrameSlotsSuits == false)
            {
                
            }
            
            
        }

        private bool? checkSlotsSuits(Frame frame)
        {
            var hasUnsetSlots = false;
            foreach (var slot in frame.Slots)
            {
                if (slot is DomainSlot)
                {
                    var domainSlot = slot as DomainSlot;
                    if (_memory.ContainsKey(slot.Name))
                    {
                        if (_memory[slot.Name] != domainSlot.Value)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        hasUnsetSlots = true;
                    }
                }
            }

            if (hasUnsetSlots)
                return null;
            return true;
        }

        private Frame _getFirstCandidate()
        {
            var used_subframes = new List<Slot>();

            foreach (var frame in _model.Frames)
            {
                used_subframes.AddRange(frame.Slots.Where(x => x is FrameSlot));
            }

            var maxUsed = used_subframes.Max(x => used_subframes.Count(y => y == x));
            var result = used_subframes.FirstOrDefault(x => used_subframes.Count(y => y == x) == maxUsed);
            var frameSlot = result as FrameSlot;
            return frameSlot?.Frame;
        }
    }
}