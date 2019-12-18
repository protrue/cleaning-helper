using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningHelper.Model
{
    public class DomainIsInUseException : Exception
    {
        public List<Tuple<Frame,FrameSlot>> FrameSlotTuple { get; }

        public FrameSlotDomain Domain { get; }

        public DomainIsInUseException(List<Tuple<Frame, FrameSlot>> frameSlotTuple, FrameSlotDomain domain) 
            : base($"Домен {domain?.Name} используется: " +
                   $"{string.Join(Environment.NewLine, frameSlotTuple.Select(t => $"{t.Item1}.{t.Item2}"))}")
        {
            FrameSlotTuple = frameSlotTuple;
            Domain = domain;
        }
    }
}
