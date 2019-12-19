using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningHelper.Model.Exceptions
{
    public class RemoveSystemSlotAttemptException : Exception
    {
        public RemoveSystemSlotAttemptException() : base("Нельзя удалить системный слот")
        {

        }
    }
}
