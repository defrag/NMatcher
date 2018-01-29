using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMatcher.Parsing
{
    public static class ExpressionParser
    {
        internal static Parser<string> Type =>
            Parse.Letter.AtLeastOnce().Text().Contained(Parse.Char('@'), Parse.Char('@'));

        internal static Parser<AST.Expander> ExpanderWithNoArguments =>
            from dot in Parse.Char('.')
            from name in Parse.Letter.AtLeastOnce().Text()
            from b in Parse.String("()")
            select new AST.Expander(name, new List<AST.Argument>());

        internal static Parser<AST.Expander> ExpanderWithArguments =>
            from dot in Parse.Char('.')
            from name in Parse.Letter.AtLeastOnce().Text()
            from b in Parse.Char('(')
            from args in Arguments
            from e in Parse.Char(')')
            select new AST.Expander(name, args);

        internal static Parser<AST.Argument> QuotedString =>
            from open in Parse.Char('"')
            from content in Parse.CharExcept('"').Many().Text()
            from close in Parse.Char('"')
            select new AST.Argument(content);

        internal static Parser<AST.Argument> SingleQuoted =>
            from open in Parse.Char('\'')
            from content in Parse.CharExcept('\'').Many().Text()
            from close in Parse.Char('\'')
            select new AST.Argument(content);

        internal static Parser<AST.Argument> Integer =>
            from digits in Parse.Digit.AtLeastOnce().Text()
            select new AST.Argument(Convert.ToInt32(digits));

        internal static Parser<AST.Argument> Double =>
            from nom in Parse.Digit.AtLeastOnce().Text()
            from dot in Parse.Char('.')
            from den in Parse.Digit.AtLeastOnce().Text()
            select new AST.Argument(Convert.ToDouble(nom + dot + den));

        internal static Parser<AST.Argument> Argument =>
            QuotedString.Or(Double).Or(Integer);

        internal static Parser<IEnumerable<AST.Argument>> Arguments =>
            Argument.DelimitedBy(Parse.Char(',').Token());

        internal static Parser<AST.Type> Expression =>
            from type in Type
            from expanders in (ExpanderWithNoArguments.Or(ExpanderWithArguments)).XMany()
            select new AST.Type(type, expanders);

        public static AST.Type Parse(string input) =>
            Expression.Parse(input);
    }

    
}
