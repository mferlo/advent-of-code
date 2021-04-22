using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    interface Operator
    {
        public long Eval(long lhs, long rhs);
    }

    class Plus : Operator
    {
        static Lazy<Plus> instance = new Lazy<Plus>(() => new Plus());
        public static Plus Instance => instance.Value;
        private Plus() {}
        public long Eval(long lhs, long rhs) => lhs + rhs;
        public override string ToString() => "+";
    }

    class Times : Operator
    {
        static Lazy<Times> instance = new Lazy<Times>(() => new Times());
        public static Times Instance => instance.Value;
        private Times() {}
        public long Eval(long lhs, long rhs) => lhs * rhs;
        public override string ToString() => "*";
    }

    interface Term
    {
        public long Eval();
    }

    class Number : Term
    {
        long Value;

        public Number(long value) => Value = value;
        public long Eval() => Value;
        public override string ToString() => Value.ToString();
    }
    
    class Group : Term
    {
        Expression Expression;

        public Group(Expression expression) => Expression = expression;
        public long Eval() => Expression.Eval();
        public override string ToString() => $"({Expression})";
    }

    class Expression
    {
        Stack<Term> Terms;
        Stack<Operator> Operators;

        public override string ToString() =>
            String.Join(" ", Terms.Zip(Operators, (term, op) => $"{term} {op}")) + " " + Terms.Last();

        static IEnumerable<string> TokenizeTopLevel(string input)
        {
            int i = 0;
            while (i < input.Length)
            {
                if (char.IsDigit(input[i]))
                {
                    var numberStart = i;
                    while (i < input.Length && char.IsDigit(input[i]))
                    {
                        i++;                        
                    }
                    yield return input.Substring(numberStart, i - numberStart);
                }
                else if (input[i] == '+' || input[i] == '*')
                {
                    yield return input[i].ToString();
                    i++;
                }
                else
                {
                    Debug.Assert(input[i] == '(');
                    var parenStart = i;
                    var nestingLevel = 1;
                    while (nestingLevel > 0)
                    {
                        i++;
                        if (input[i] == '(') nestingLevel++;
                        if (input[i] == ')') nestingLevel--;
                    }
                    i++;
                    yield return input.Substring(parenStart, i - parenStart);
                }

                Debug.Assert(i == input.Length || input[i] == ' ');
                i++;
            }
        }

        static IEnumerable<string> AddParenthesesForPart2(List<string> tokens)
        {
            if (tokens.Count == 3 && tokens[1] == "+")
            {
                yield return tokens[0];
                yield return tokens[1];
                yield return tokens[2];
                yield break;
            }

            while (tokens.Count > 1)
            {
                if (tokens[1] == "*")
                {
                    yield return tokens[0];
                    yield return tokens[1];
                    tokens.RemoveRange(0, 2);
                }
                else
                {
                    Debug.Assert(tokens[1] == "+");
                    var group = "(" + string.Join(" ", tokens.Take(3)) + ")";
                    tokens.RemoveRange(0, 3);
                    tokens.Insert(0, group);
                }
            }
            yield return tokens.Single();
        }

        public static Expression Parse(string input, bool part2)
        {
            var terms = new List<Term>();
            var operators = new List<Operator>();

            var tokens = TokenizeTopLevel(input).ToList();
            if (part2)
            {
                tokens = AddParenthesesForPart2(tokens).ToList();
            }

            foreach (var token in tokens)
            {
                if (token == "+")
                {
                    operators.Add(Plus.Instance);
                }
                else if (token == "*")
                {
                    operators.Add(Times.Instance);
                }
                else if (token.StartsWith("("))
                {
                    Debug.Assert(token.EndsWith(")"));
                    var subexpression = Expression.Parse(token.Substring(1, token.Length - 2), part2);
                    terms.Add(new Group(subexpression));
                }
                else
                {
                    terms.Add(new Number(long.Parse(token)));
                }
            }

            return new Expression(terms, operators);
        }

        private Expression(IEnumerable<Term> terms, IEnumerable<Operator> operators)
        {
            Debug.Assert(operators.Count() + 1 == terms.Count());

            Terms = new Stack<Term>(terms.Reverse());
            Operators = new Stack<Operator>(operators.Reverse());
        }

        public long Eval()
        {
            while (Operators.Any())
            {
                var lhs = Terms.Pop();
                var rhs = Terms.Pop();
                var op = Operators.Pop();
                var result = op.Eval(lhs.Eval(), rhs.Eval());

                Terms.Push(new Number(result));
            }

            Debug.Assert(Terms.Count == 1);
            return Terms.Pop().Eval();
        }
    }


    class Day18
    {
        static IList<string> Input;

        public static void Parse() => Input = File.ReadAllLines("18.txt");

        public static void Test()
        {
            void Test(string line)
            {
                Console.WriteLine($"Test: {line}");
                var expr = Expression.Parse(line, false);
                Console.WriteLine($"Part 1 Result: {expr.Eval()}");
                expr = Expression.Parse(line, true);
                Console.WriteLine($"Part 2 Result: {expr.Eval()}");
            }

            Test("1 + 2 * 3 + 4 * 5 + 6");
            Test("1 + (2 * 3) + (4 * (5 + 6))");
            Test("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2");
        }

        public static object Part1() => Input.Select(line => Expression.Parse(line, false)).Sum(expr => expr.Eval());
        public static object Part2() => Input.Select(line => Expression.Parse(line, true)).Sum(expr => expr.Eval());
    }
}