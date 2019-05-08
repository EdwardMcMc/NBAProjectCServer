using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NbaSharedModels.Exceptions
{
    public class NullTeamReqModelException : Exception
    {
        public NullTeamReqModelException()
        {
        }

        public NullTeamReqModelException(string message)
            : base(message)
        {
        }

        public NullTeamReqModelException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
