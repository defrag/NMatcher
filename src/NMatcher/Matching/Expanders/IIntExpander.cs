using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Matching.Expanders
{
    public interface IIntExpander
    {
        bool Matches(int value);
    }
}
