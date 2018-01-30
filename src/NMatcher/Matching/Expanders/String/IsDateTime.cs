using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Matching.Expanders.String
{
    public sealed class IsDateTime : IStringExpander
    {

        public bool Matches(string value)
        {
            DateTime dateValue;

            try
            {
                var result = DateTime.Parse(value, CultureInfo.InvariantCulture);
                return Result.Success();
            }
            catch (FormatException e)
            {
                return Result.Failure($"Value '{value} is not a valid DateTime.'");
            }
        }
    }
}
