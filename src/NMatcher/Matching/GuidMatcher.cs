using System;

namespace NMatcher.Matching
{
    public sealed class GuidMatcher : IMatcher
    {
        public Result Match(object value)
        {
            try
            {
                Guid.Parse(value.ToString());
                return Result.Success();
            }
            catch (FormatException e)
            {
                return Result.Failure($"{value} is not a valid Guid.");
            }
        }
    }
}
