//Jonathan hein
//11532242
//HW 9
//11/26/18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CptS321
{
    public class ExpTree
    {
        public string Expression;
        private opNode root;
        public static Dictionary<string, double> varDict = new Dictionary<string, double>();
        //Support operators +,-,*, and / for addition, subtraction, multiplication, and division,
        //respectively.Again, only one type will be present in an expression that the user enters.
        private char[] supportedOps = {'+','-','*','/'};
        private abstract class Node
        {
            public abstract double eval(); 
        }
        private class valNode : Node
        {
            protected double Value;
            public double getValue() { return Value; }
            public valNode(double val)
            {
                Value = val;
            }
            public override double eval() //will be leaf node so no iteration necessary
            {
                return Value;
            }
        }
        private class varNode : Node
        {
            protected string Name;
            public string getName() { return Name; }
            public varNode(string var)
            {
                Name = var;
            }
            public override double eval() //will be leaf node so no iteration necessary
            {
              
                return varDict[Name];
            }
        }
        private class opNode : Node
        {
            protected string Name;
            public string getName() { return Name; }
            public Node left, right;
            public opNode(string var,Node l,Node r)
            {
                Name = var;
                left = l;
                right = r;
            }
            public override double eval()
            {
                double lVal = left.eval();
                double rVal = right.eval();
                
                if (this.getName() == "+")
                {
                    return lVal + rVal;
                }
                if (this.getName() == "-")
                {
                    return lVal - rVal;
                }
                if (this.getName() == "*")
                {
                    return lVal * rVal;
                }
                return lVal / rVal;
            }
        }
        public List<string> varList()
        {
            List<string> keyList = new List<string>(varDict.Keys);
            return keyList;
        }
        private Node expIden(string exp)
        {
            double num;
            if (double.TryParse(exp, out num)) //exp is integer and num contains it
            {
                valNode newnode = new valNode(num);
                return newnode;
            }
            else if (supportedOps.Contains(exp[0]))
            {
                opNode newnode = new opNode(exp, null, null);
                return newnode;
            }
            else
            {
                if (exp.Length == 2 && Char.IsNumber(exp[1])) // only time var and int allowed i.e. B2
                {
                    varNode newnode = new varNode(exp);
                    double val;
                    if(varDict.TryGetValue(exp,out val))
                    {
                        
                    }
                    else
                    {
                        varDict.Add(exp, default(double));
                    }
                    
                    return newnode;
                }
                else
                {
                    foreach (char x in exp) //all characters are not integers
                    {
                        if (Char.IsNumber(x))
                        {
                            return null;
                        }
                    }
                    varNode newnode = new varNode(exp);
                    if (!varDict.ContainsKey(exp))
                    {
                        varDict.Add(exp, default(double));
                    }
                    
                    return newnode;
                    
                }
            }
        }
        private List<string> rpn(string exp)
        {
            Stack<string> opstack = new Stack<string>();
            List<string> formula = new List<string>();
            char[] tobreak = { '/', '+', '*', '*','-', '(', ')' };
            for (int i = 0; i < exp.Length; i++)
            {
                char c = exp[i];
                if (c == '(')
                {
                    opstack.Push(Char.ToString(c));
                }
                else if (c == ')')
                {
                    while (opstack.Count > 0 && opstack.Peek() != "(")
                    {
                        formula.Add(opstack.Pop());
                    }
                    if (opstack.Count != 0)
                    {
                        opstack.Pop();
                    }
                }
                else if (supportedOps.Contains(c))
                {
                    while (opstack.Count > 0 && opstack.Peek() != "(" && Precendence(Char.ToString(c)) <= Precendence(opstack.Peek()))
                    {
                        formula.Add(opstack.Pop());
                    }
                    opstack.Push(Char.ToString(c));
                }
                else if ((exp[i] >= '0' && exp[i] <= '9') || (exp[i] >='a' && exp[i]<='z') || (exp[i] >= 'A' && exp[i] <= 'Z')) 
                {
                    StringBuilder var = new StringBuilder();
                    int j = 0;
                    while(!tobreak.Contains(exp[i+j])) //support var or integers up to length of expression
                    {
                        var.Append(exp[i + j]);
                        j++;
                        if (i+j == exp.Length)
                        {
                            break;
                        }
                    }
                    formula.Add(var.ToString());
                    i = (i + j) - 1; // -1 to account for loop
                }
                else
                {
                    string x = opstack.Pop();
                    if (x != "(")
                    {
                        formula.Add(x);
                    }
                }
            }
            while(opstack.Count>0)
            {
                formula.Add(opstack.Pop());
            }
            return formula;
        }
        static int Precendence(string c) //PEMDAS
        {
            switch (c)
            {
                case "+":
                    return 1;
                case "-":
                    return 2;
                case "*":
                    return 3;
                case "/":
                    return 4;
                default:
                    return 0; //error
            }
        }
        public ExpTree(string exp) //
        {
            Expression = exp;
            List<string> rpnExp = rpn(exp); //from here we have a rpn list of strings that has the tree printed in postfix
            Stack<Node> rpnNodes = new Stack<Node>();
            foreach (string item in rpnExp) // iterates through rpnExp and populates tree
            {
                if (supportedOps.Contains(item[0])) // if operator, pops off nodes from stack and then puts opnode onto stack
                {
                    Node r = rpnNodes.Pop(); // since [i-2] is left and [i-1] is right for operator
                    Node l = rpnNodes.Pop();
                    root = new opNode(item, l, r);
                    rpnNodes.Push(root);
                }
                else
                {
                    rpnNodes.Push(expIden(item));
                }
            }
        }
        //Sets the specified variable within the ExpTree variables dictionary
        public void SetVar(string varName,double varValue)
        {
            if (varDict.ContainsKey(varName))
            {
                varDict[varName] = varValue;
            }
        }
        public double Eval()
        {
            return root.eval();
        }
        
    }
}
