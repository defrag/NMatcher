using NMatcher.Matching;

namespace NMatcher.Activation
{
    public interface IActivator
    {
        IMatcher CreateMatcherInstance(Parsing.AST.Type type);
    }
}
