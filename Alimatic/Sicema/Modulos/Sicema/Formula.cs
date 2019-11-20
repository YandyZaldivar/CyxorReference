using System;
using System.Linq;
using System.Collections.Generic;

using org.mariuszgromada.math.mxparser;

namespace Sicema
{
    public class Formula
    {
        public static Dictionary<string, Formula> Formulas = new Dictionary<string, Formula>();

        public Argument Argument { get; set; }

        public string Value { get; set; }

        public bool Computed { get; set; }

        public Dictionary<string, Formula> Dependents = new Dictionary<string, Formula>();

        //public double Calculate(bool modified)
        //{
        //    if (Argument.checkSyntax())
        //        return argument.getArgumentValue();

        //    var expression = new Expression(argument.getArgumentExpressionString());

        //    var argumentNames = expression.getMissingUserDefinedArguments();

        //    if (argumentNames.Length > 0)
        //    {
        //        var arguments = Arguments.Where(p => argumentNames.Contains(p.getArgumentName()));
        //        //argument.addArguments(arguments.ToArray());

        //        foreach (var arg in arguments)
        //        {
        //            if (argument.checkSyntax())
        //                expression.addArguments(arg);
        //            else
        //            {
        //                var value = Calculate(arg);
        //                argument.addArguments(arg);

        //                if (!arg.checkSyntax())
        //                    throw new InvalidOperationException();
        //                else
        //                    expression.addArguments(arg);
        //            }
        //        }
        //    }

        //    return expression.calculate();
        //}
    }
}
