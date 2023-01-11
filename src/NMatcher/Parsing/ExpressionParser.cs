using Sprache;
using System;
using System.Collections.Generic;

namespace NMatcher.Parsing
{
    public static class ExpressionParser
    {
        internal static Parser<Tuple<string, bool>> Type =>
            from open in Parse.Char('@')
            from name in Parse.Letter.AtLeastOnce().Text()
            from optional in Parse.Char('?').Optional()
            from close in Parse.Char('@')
            select Tuple.Create(name, optional.IsDefined);

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

        internal static Parser<AST.Argument> DoubleQuotedString =>
            from open in Parse.Char('"')
            from content in Parse.CharExcept('"').Many().Text()
            from close in Parse.Char('"')
            select new AST.Argument(content);

        internal static Parser<AST.Argument> SingleQuotedString =>
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
            DoubleQuotedString.Or(SingleQuotedString).Or(Double).Or(Integer);

        internal static Parser<IEnumerable<AST.Argument>> Arguments =>
            Argument.DelimitedBy(Parse.Char(',').Token());

        internal static Parser<AST.Type> Expression =>
            from type in Type
            from expanders in (ExpanderWithNoArguments.Or(ExpanderWithArguments)).Many()
            select new AST.Type(type.Item1, expanders, type.Item2);

        internal static Parser<IEnumerable<AST.INode>> Expressions =>
            from expr in Expression.Or<AST.INode>(ExpressionSpace).Many()
            select expr;

        internal static Parser<AST.Literal> ExpressionSpace =>
           from txt in Parse.AnyChar.Except(Expression).Many().Text()
           select new AST.Literal(txt, AST.Literal.LiteralType.String);

        internal static Parser<AST.Literal> StringLiteral =>
            from txt in Parse.AnyChar.Many().Text()
            select new AST.Literal(txt, AST.Literal.LiteralType.String);

        internal static Parser<AST.Literal> IntLiteral =>
            from value in Parse.Number.Text().End()
            select new AST.Literal(value, AST.Literal.LiteralType.Int);

        internal static Parser<AST.Literal> DoubleLiteral =>
            from nom in Parse.Digit.AtLeastOnce().Text()
            from dot in Parse.Char('.')
            from den in Parse.Digit.AtLeastOnce().Text()
            select new AST.Literal($"{nom}{dot}{den}", AST.Literal.LiteralType.Double);

        internal static Parser<AST.Literal> BoolLiteral =>
            from value in Parse.String("true").Or(Parse.String("false")).Text()
            select new AST.Literal(value, AST.Literal.LiteralType.Bool);

        internal static Parser<AST.Literal> Literal =>
            DoubleLiteral.Or(BoolLiteral).Or(IntLiteral).Or(StringLiteral).Token();

        internal static AST.Literal ParseLiteral(string input) =>
            Literal.Parse(input);

        internal static IEnumerable<AST.INode> ParseExpressions(string input) =>
            Expressions.Parse(input);
    }
}
