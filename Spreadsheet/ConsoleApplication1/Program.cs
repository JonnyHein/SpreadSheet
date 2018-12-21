//Jonathan hein
//11532242
//HW 9
//11/26/18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;

namespace Menu
{
    class Program
    {
        /*1. The option to enter an expression string. You may assume that only valid
    expressions will be entered with no whitespace during grading.Simplified
    expressions are used for this assignment and the assumptions you are allowed
    to make are discussed later on.
    2. The option to set a variable value in the expression.This must prompt for both
    the variable name and then the variable value.
    3. The option to evaluate to the expression to a numerical value.
    4. The option to quit
    5. Should the user enter an “option” that isn’t one of these 4, simply ignore it.As
    trivial as this may seem it is vital should the assignment be graded with an
    automated grading app, or even with a scripted series of commands used by the
    graders.
    a.On that note, also avoid use of Console.ReadKey() as it can be
    problematic when grading with an automated app.Simply fall through to
    the end of main after the quit option is selected.*/
        static void Main(string[] args)
        {
            string input = "";
            ExpTree myTree = new ExpTree("((3+1)+2)-8");
            while (input != "4")
            {
                Console.WriteLine("Menu (current expression: "+myTree.Expression+")"); //add current expression with default as first
                Console.WriteLine("     1. Enter a new expression");
                Console.WriteLine("     2. Set a variable value");
                Console.WriteLine("     3. Evaluate tree");
                Console.WriteLine("     4. Quit");
                input = Console.ReadLine();
                switch(input)
                {
                    case "1":
                        Console.WriteLine("Enter new expression:");
                        myTree = new ExpTree(Console.ReadLine());
                        break;
                    case "2":
                        Console.WriteLine("Enter variable to set:");
                        string var = Console.ReadLine();
                        Console.WriteLine("Enter value to set " + var + " to:");
                        double val = Convert.ToDouble(Console.ReadLine());
                        myTree.SetVar(var, val);
                        break;
                    case "3":
                        Console.WriteLine(myTree.Eval().ToString());
                        break;
                    default:
                        break;
                        //do nothing while will handle quit
                }
            }
        }
    }
}
