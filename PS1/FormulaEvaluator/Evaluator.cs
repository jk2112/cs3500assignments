using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
/// <summary>
/// Jarem Kilby
/// 
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
            // remove any leading or trailing whitespace, then split the expression into substrings that we can evaluate
            // (removing whitespace function comes from Stack Exchange.)
            Regex.Replace(exp, @"\s+", "");
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)"); // Should separate into multi-character numbers and variables

            //Make 2 stacks and split the substrings into the stacks
            Stack<int> values = new Stack<int>();
            Stack<string> operators = new Stack<string>();
            // Used by the delegate to store the numeric values of variables.
            Dictionary<String, int> variableValues = new Dictionary<String, int>();

            // The returned int when a string is parsed.
            int intParsedValue;
            // The modified variable for returning a Dictionary value on a Dictionary key
            int keyIndex;

            foreach (string token in substrings)
            {
                //ignore empty Strings by skipping to the next loop
                if (String.IsNullOrEmpty(token))
                {
                    continue;
                }
                //Try to parse to get ints.
                if (int.TryParse(token, out intParsedValue))
                {
                    // Perform multiplication or division, then put the answer in values.
                    if (operators.Peek() == "*" || operators.Peek() == "/")
                    {
                        if (values.Count == 0)
                        {
                            throw new ArgumentException("There must be a number after each operator");
                        }
                        values.Push(MultOrDivide(int.Parse(token), operators.Pop(), values.Pop()));
                    }
                }

                // Check is the next token is a variable. If it is, use the variables dictionary to retrieve the value.
                else if (token.ToUpper()[0] >= 65 || token.ToUpper()[0] <= 90)
                {
                    // Perform multiplication or division, then put the answer in values.
                    if (operators.Peek() == "*" || operators.Peek() == "/")
                    {
                        if (values.Count == 0)
                        {
                            throw new ArgumentException("There must be a number after each operator");
                        }
                        if (!variableValues.TryGetValue(token, out keyIndex))
                        {
                            throw new ArgumentException("This variable has no value");
                        }
                        values.Push(MultOrDivide(keyIndex, operators.Pop(), values.Pop()));
                    }

                }
                // Check if the next token is an operator. If + or -, immediately do the math. If * or /, push.
                // TODO may have to handle a negative answer even though not in assignment specs.
                else if (token == "+" || token == "-")
                {
                    int lhs = values.Pop();
                    int rhs = values.Pop();

                    values.Push(AddOrSub(lhs, operators.Pop(), rhs));
                }

                else if(token == "*" || token == "/" || token == "(")
                {
                    operators.Push(token);
                }
                else if(token == ")")
                {
                    if(operators.Peek() == "+" || operators.Peek() == "-")
                    {
                        int lhs = values.Pop();
                        int rhs = values.Pop();

                        values.Push(AddOrSub(lhs, operators.Pop(), rhs));
                    }
                    if(operators.Pop() != "(")
                    {
                        throw new ArgumentException("There aren't matching parentheses.");
                    }
                    if(operators.Peek() == "*" || operators.Peek() == "/")
                    {
                        values.Push(MultOrDivide(values.Pop(), operators.Pop(), values.Pop()));
                    }
                }

                if(operators.Count == 0)
                {
                    return values.Single();
                }
                else
                {
                    return AddOrSub(values.Pop(), operators.Single(), values.Pop());
                }
            }
        }

        /// <summary>
        ///  Pop the value stack, pop the operator stack, and apply the popped operator to t 
        /// and the popped number. Push the result onto the value stack.
        /// Otherwise, push t onto the value stack.
        /// </summary>
        /// <param name="var1"> The left hand side of the multiplication or the numerator of division </param>
        /// <param name="oper"> The operator. Should be * or /.</param>
        /// <param name="var2"> The right hand side or multiplication or the numerator of division.</param>
        /// <returns></returns>
        private static int MultOrDivide(int var1, string oper, int var2)
        {
            int answer = 0;
            switch(oper)
            {
                case ("*"):
                    answer = var1 * var2;
                    break;
                case ("/"):
                    if (var2 == 0) throw new DivideByZeroException("Cannot divide by 0");
                    answer = var1 / var2;
                    break;
                default:
                    answer = var1;
                    break;

            }
            return answer;
        }
        /// <summary>
        /// Pop the value stack twice and the operator stack once. 
        /// Apply the popped operator to the popped numbers. Push the result onto the value stack. 
        /// Next, push token onto the operator stack
        /// </summary>
        /// <param name="var1">Left hand side.</param>
        /// <param name="oper">Operator, should be + or -.</param>
        /// <param name="var2"> Right hand Side</param>
        /// <returns></returns>
        private static int AddOrSub(int var1, string oper, int var2)
        {
            int answer = 0;
            switch(oper)
            {
                case ("+"):
                    answer = var1 + var2;
                    break;
                case ("-"):
                    answer = var1 - var2;
                    break;
                default:
                    throw new ArgumentException("There must be a value on either side of each operator.");
                    break;
            }
            return answer;
        }

    }
}

