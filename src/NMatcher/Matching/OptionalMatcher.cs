using System;

namespace NMatcher.Matching
{
    public sealed class OptionalMatcher : IMatcher
    {
        private readonly IMatcher _matcher;

        public OptionalMatcher(IMatcher matcher)
        {
            _matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
        }
        public Result Match(object value)
        {
            if (null == value)
            {
                return Result.Success();
            }

            return _matcher.Match(value);
        }
    }
}
