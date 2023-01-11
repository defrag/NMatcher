using NMatcher.Matching;

namespace NMatcher.Activation
{
    internal interface IActivator
    {
        IMatcher CreateMatcherInstance(Parsing.AST.Type type);
    }
}
