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
        private Stack<Stack<Frame>> _bindingStacks = new Stack<Stack<Frame>>();
        
        private Dictionary<Frame, bool> _bindedFrames = new Dictionary<Frame, bool>();
        private Slot _askSlot;

        private Frame _bindedSubframe;

        public DownUpReasoner(FrameModel model, IEnumerable<String> goalSlotNames)
        {
            _model = model;
            _goalSlotNames = goalSlotNames;
        }

        public DomainSlot GetNextValueToAsk()
        {
            if (_bindingCandidate == null)
            {
                _bindingCandidate = _getFirstCandidate();
                Console.WriteLine("First candidate is ", _bindingCandidate);
                
                _bindingStacks.Push(new Stack<Frame>(new []{_bindingCandidate}));
            }

            while (true)
            {
                if (_bindingStacks.Count == 0)
                {
                    Console.WriteLine("The binding stacks are empty");
                    return null;
                }

                if (_bindingStacks.Peek().Count == 0)
                {
                    _bindingStacks.Pop();
                    continue;
                }
                
                var topFrameSlotsSuits = checkSlotsSuits(_bindingStacks.Peek().Peek());
                if (topFrameSlotsSuits == true)
                {   
                    _bindedFrames[_bindingStacks.Peek().Peek()] = true;
                    if (_bindingStacks.Peek().Peek().Parent != null)
                        _bindingStacks.Peek().Push(_bindingStacks.Peek().Peek().Parent);
                    else
                    {
                        _bindingStacks.Peek().Clear();
                    }
                }
                else if (topFrameSlotsSuits == false)
                {
                    //_bindedFrames[_bindingStacks.Last()] = false;
                    foreach (var frame in _bindingStacks)
                    {
                        _bindedFrames[frame] = false;
                    }
                    _bindingStacks.Clear();
                }
                else
                {
                    var slot = findFirstSlotToAsk(_bindingStacks.Last());
                    _askSlot = slot;
                    return (DomainSlot) _askSlot;
                }
            }

            return null;
        }
        
        public void SetAnswer(DomainValue value)
        {
            _memory[_askSlot.Name] = value;
        }

        private Slot findFirstSlotToAsk(Frame frame)
        {
            foreach (var slot in frame.Slots)
            {
                if (!(slot is DomainSlot)) continue;
                if (!_memory.ContainsKey(slot.Name))
                {
                    return slot;
                }
            }

            throw new Exception($"Frame {frame} was expected to have unasked slots");
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
            var subframe = _getMostCommonSubframe();
            var leaf = _getAnyLeaf();
            return subframe ?? leaf;
        }

        private Frame _getMostCommonSubframe()
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
        
        private Frame _getAnyLeaf()
        {
            foreach (var frame in _model.Frames)
            {
                if (frame.Children.Count == 0)
                    return frame;
            }
            
            return null;
        }
    }
}