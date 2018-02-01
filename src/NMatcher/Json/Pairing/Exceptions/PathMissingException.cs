using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Json.Pairing.Exceptions
{
    internal class PathMissingException : Exception
    {
        public PathMissingException(string message) : base(message)
        {
        }
    }
}
