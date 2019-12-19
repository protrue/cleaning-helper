using System;
using System.Collections.Generic;
using System.Linq;

namespace CleaningHelper.Model.Exceptions
{
    public class DomainIsInUseException : Exception
    {
        public List<Tuple<Frame,DomainSlot>> FrameSlotTuple { get; }

        public Domain Domain { get; }

        public DomainIsInUseException(List<Tuple<Frame, DomainSlot>> frameSlotTuple, Domain domain) 
            : base($"Домен {domain?.Name} используется: " +
                   $"{string.Join(Environment.NewLine, frameSlotTuple.Select(t => $"{t.Item1}.{t.Item2}"))}")
        {
            FrameSlotTuple = frameSlotTuple;
            Domain = domain;
        }
    }
}
