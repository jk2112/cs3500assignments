using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
/// <summary>
/// Evaluates arithmetic expressions using infix notation.
/// </summary>
namespace FormulaEvaluator
{
    /// <summary>
    /// A library of evaluators for different functions.
    /// </summary>
    public static class Evaluator
    {
        public delegate int Lookup(String v);

        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            // TODO...

            // remove any leading or trailing whitespace, then split the expression into substrings that we can evaluate
            Regex.Replace(exp, @"\s+", "");
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
//            foreach (string character in substrings)
 //           {
 //               character.Trim();
  //          }

            //TODO ignore empty Strings
            return 0;

        }
    }
