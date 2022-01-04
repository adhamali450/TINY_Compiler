using System.Collections.Generic;
using System.Windows.Forms;

namespace TINY_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }
        Node Program()
        {
            Node program = new Node("Program");
            program.Children.Add(Function());
            program.Children.Add(Main_function());

            // program.Children.Add(match(TokenClass.Dot));
            MessageBox.Show("Success");
            return program;
        }
        Node Function()
        {
            Node fuction = new Node("Program");
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].TokenType == TokenClass.Constant || TokenStream[InputPointer].TokenType == TokenClass.Float || TokenStream[InputPointer].TokenType == TokenClass.String))
            {
                fuction.Children.Add(Function_STMT());
                fuction.Children.Add(Function());
            }
            else
                return null;


            return fuction;
        }
        Node Function_STMT()
        {
            Node function_stmt = new Node("Function_stmt");

            function_stmt.Children.Add(Function_declaration());
            function_stmt.Children.Add(Function_body());

            return function_stmt;
        }

        Node Main_function()
        {
            Node main_function = new Node("Main function");

            main_function.Children.Add(DataType());
            main_function.Children.Add(match(TokenClass.Idenifier));
            main_function.Children.Add(match(TokenClass.LParanthesis));
            main_function.Children.Add(match(TokenClass.RParanthesis));
            main_function.Children.Add(Function_body());

            return main_function;
        }
        Node Function_body()
        {
            Node function_body = new Node("function body");

            function_body.Children.Add(match(TokenClass.LParanthesis));
            function_body.Children.Add(Statements());
            function_body.Children.Add(Return_statement());
            function_body.Children.Add(match(TokenClass.RParanthesis));

            return function_body;

        }
        Node Fuction_call()
        {
            Node function_call = new Node("function_call");
            function_call.Children.Add(match(TokenClass.Idenifier));
            function_call.Children.Add(match(TokenClass.LParanthesis));
            function_call.Children.Add(Arglist());
            function_call.Children.Add(match(TokenClass.RParanthesis));

            return function_call;
        }
        Node Arglist()
        {
            Node arglist = new Node("Arglist");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Idenifier)
                arglist.Children.Add(Arguments());
            else
                return null;

            return arglist;
        }
        Node Arguments()
        {
            Node arguments = new Node("Arguments");
            arguments.Children.Add(match(TokenClass.Idenifier));
            arguments.Children.Add(Args());
            return arguments;
        }
        Node Args()
        {
            Node args = new Node("Args");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Comma)
            {
                args.Children.Add(match(TokenClass.Comma));
                args.Children.Add(match(TokenClass.Idenifier));
                args.Children.Add(Args());
            }
            else
                return null;

            return args;
        }

        Node Term()
        {
            Node term = new Node("TERM");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Constant)
                term.Children.Add(match(TokenClass.Constant));

            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Idenifier)
                term.Children.Add(match(TokenClass.Idenifier));

            else
                term.Children.Add(Fuction_call());

            return term;
        }
        Node Arithmetic_operator()
        {
            Node arithmetic_operator = new Node("arithemtic operator");

            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.PlusOp)
                arithmetic_operator.Children.Add(match(TokenClass.PlusOp));
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.MinusOp)
                arithmetic_operator.Children.Add(match(TokenClass.MinusOp));
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.MultiplyOp)
                arithmetic_operator.Children.Add(match(TokenClass.MultiplyOp));
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.DivideOp)
                arithmetic_operator.Children.Add(match(TokenClass.DivideOp));

            return arithmetic_operator;
        }
        Node Equation()
        {
            Node equation = new Node("equation");
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].TokenType == TokenClass.Constant || TokenStream[InputPointer].TokenType == TokenClass.Idenifier))
            {
                equation.Children.Add(Term());
                equation.Children.Add(Eq());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.LParanthesis)
            {
                equation.Children.Add(match(TokenClass.LParanthesis));
                equation.Children.Add(Equation());
                equation.Children.Add(match(TokenClass.RParanthesis));
                equation.Children.Add(Eq());
            }
            return equation;
        }
        Node Eq()
        {
            Node eq = new Node("eq");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.LParanthesis)
            {
                eq.Children.Add(Arithmetic_operator());
                eq.Children.Add(Equation());
            }
            else
                return null;

            return eq;
        }
        Node Expression()
        {
            Node expression = new Node("expression");
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].TokenType == TokenClass.Constant || TokenStream[InputPointer].TokenType == TokenClass.Idenifier))
            {
                expression.Children.Add(Arithmetic_operator());
                expression.Children.Add(Equation());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.String)
            {
                expression.Children.Add(match(TokenClass.String));
            }

            else
                expression.Children.Add(Equation());

            return expression;
        }
        Node Assignment_statement()
        {
            Node assignment_statement = new Node("Assignment_statement");
            assignment_statement.Children.Add(match(TokenClass.Idenifier));
            assignment_statement.Children.Add(match(TokenClass.AssignmentOp));

            return assignment_statement;
        }
        Node DataType()
        {
            Node dataType = new Node("dataType");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Int)
            {
                dataType.Children.Add(match(TokenClass.Int));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Float)
            {
                dataType.Children.Add(match(TokenClass.Float));
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.String)
            {
                dataType.Children.Add(match(TokenClass.String));
            }
            return dataType;

        }
        Node Declaration_Statement()
        {
            Node declaration_statemet = new Node("declaration_statement");

            declaration_statemet.Children.Add(DataType());
            declaration_statemet.Children.Add(match(TokenClass.Idenifier));
            declaration_statemet.Children.Add(Declist());

            return declaration_statemet;
        }
        Node Declist()
        {
            Node declist = new Node("declist");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.AssignmentOp)
                declist.Children.Add(Assign());
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].TokenType == TokenClass.Comma || TokenStream[InputPointer].TokenType == null))
                declist.Children.Add(Declaration());
            else
                return null;

            return declist;

        }
        Node Declaration()
        {
            Node declaration = new Node("");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Comma)
            {
                declaration.Children.Add(match(TokenClass.Comma));
                declaration.Children.Add(match(TokenClass.Idenifier));
                declaration.Children.Add(Assign());
                declaration.Children.Add(Declaration());
            }
            else
                return null;
            return declaration;
        }
        Node Assign()
        {
            Node assign = new Node("assign");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.AssignmentOp)
            {
                assign.Children.Add(match(TokenClass.AssignmentOp));
                assign.Children.Add(Expression());
                assign.Children.Add(Declaration());

            }
            return assign;
        }
        Node Write_statement()
        {
            Node write_statement = new Node("write statement");
            write_statement.Children.Add(match(TokenClass.Write));
            write_statement.Children.Add(W_statement());

            return write_statement;

        }
        Node W_statement()
        {
            Node w_statement = new Node("w_statement");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Endl)
                w_statement.Children.Add(match(TokenClass.Endl));
            else
                w_statement.Children.Add(Expression());

            return w_statement;

        }
        Node Read_statement()
        {
            Node read_statement = new Node("read_statement");
            read_statement.Children.Add(match(TokenClass.Read));
            read_statement.Children.Add(match(TokenClass.Idenifier));
            return read_statement;
        }
        Node Return_statement()
        {
            Node return_statement = new Node("return statement");

            return_statement.Children.Add(match(TokenClass.Return));
            return_statement.Children.Add(Expression());

            return return_statement;

        }
        Node Condition_statement()
        {
            Node condition_statement = new Node("condition statement");
            condition_statement.Children.Add(Condition());
            condition_statement.Children.Add(Condstmts());

            return condition_statement;
        }
        Node Condstmts()
        {
            Node condstmts = new Node("cond stmts");
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].TokenType == TokenClass.AndOp || TokenStream[InputPointer].TokenType == TokenClass.OrOp))
            {
                condstmts.Children.Add(Bool_op());
                condstmts.Children.Add(Condition());
                condstmts.Children.Add(Condstmts());
            }
            else
                return null;

            return condstmts;
        }
        Node Condition()
        {
            Node condition = new Node("condition");

            condition.Children.Add(match(TokenClass.Idenifier));
            condition.Children.Add(Condition_op());
            condition.Children.Add(Term());

            return condition;
        }
        Node Condition_op()
        {
            Node condition_op = new Node("condition operator");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.IsLessThanOp)
                condition_op.Children.Add(match(TokenClass.IsLessThanOp));
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.IsGreaterThanOp)
                condition_op.Children.Add(match(TokenClass.IsGreaterThanOp));
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.IsEqualOp)
                condition_op.Children.Add(match(TokenClass.IsEqualOp));
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.IsNotEqualOp)
                condition_op.Children.Add(match(TokenClass.IsNotEqualOp));

            return condition_op;
        }
        Node Bool_op()
        {
            Node bool_op = new Node("bool operator");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.AndOp)
                bool_op.Children.Add(match(TokenClass.AndOp));
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.OrOp)
                bool_op.Children.Add(match(TokenClass.OrOp));

            return bool_op;
        }
        Node Function_declaration()
        {
            Node function_declaration = new Node("function declaration");
            function_declaration.Children.Add(DataType());
            function_declaration.Children.Add(match(TokenClass.Idenifier));
            function_declaration.Children.Add(match(TokenClass.LParanthesis));
            function_declaration.Children.Add(Parlist());
            function_declaration.Children.Add(match(TokenClass.RParanthesis));

            return function_declaration;
        }
        Node Parlist()
        {
            Node parlist = new Node("parlist");
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].TokenType == TokenClass.Constant || TokenStream[InputPointer].TokenType == TokenClass.Float || TokenStream[InputPointer].TokenType == TokenClass.String))
                parlist.Children.Add(Parameters());
            else
                return null;

            return parlist;
        }
        Node Parameters()
        {
            Node parameters = new Node("parameters");

            parameters.Children.Add(Parameter());
            parameters.Children.Add(Par());

            return parameters;
        }
        Node Parameter()
        {
            Node parameter = new Node("parameter");
            parameter.Children.Add(DataType());
            parameter.Children.Add(match(TokenClass.Idenifier));

            return parameter;
        }
        Node Par()
        {
            Node par = new Node("par");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Comma)
            {
                par.Children.Add(match(TokenClass.Comma));
                par.Children.Add(Parameter());
                par.Children.Add(Par());
            }
            else
                return null;

            return par;
        }
        Node If_statement()
        {
            Node if_statement = new Node("if statement");
            if_statement.Children.Add(match(TokenClass.If));
            if_statement.Children.Add(Condition_statement());
            if_statement.Children.Add(match(TokenClass.Then));
            if_statement.Children.Add(Statements());
            if_statement.Children.Add(Els());

            return if_statement;
        }
        Node Else_if()
        {
            Node else_if = new Node("else if");
            else_if.Children.Add(match(TokenClass.Elseif));
            else_if.Children.Add(Condition_statement());
            else_if.Children.Add(match(TokenClass.Then));
            else_if.Children.Add(Statements());
            else_if.Children.Add(Els());

            return else_if;
        }
        Node Else_statement()
        {
            Node else_statement = new Node("else statement");
            else_statement.Children.Add((match(TokenClass.Else)));
            else_statement.Children.Add(Statements());
            else_statement.Children.Add(match(TokenClass.End));

            return else_statement;
        }
        Node Repeat_statement()
        {
            Node repeat_statement = new Node("repeat statement");
            repeat_statement.Children.Add(match(TokenClass.Repeat));
            repeat_statement.Children.Add(Statements());
            repeat_statement.Children.Add(match(TokenClass.Until));
            repeat_statement.Children.Add(Condition_statement());

            return repeat_statement;
        }
        Node Els()
        {
            Node els = new Node("else");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Elseif)
                els.Children.Add(Else_if());
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Else)
                els.Children.Add(Else_statement());
            else
                els.Children.Add(match(TokenClass.End));
            return els;
        }
        Node Statements()
        {
            Node statements = new Node("statement");
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].TokenType == TokenClass.Idenifier || (TokenStream[InputPointer].TokenType == TokenClass.Constant || TokenStream[InputPointer].TokenType == TokenClass.Float || TokenStream[InputPointer].TokenType == TokenClass.String) || TokenStream[InputPointer].TokenType == TokenClass.Write || TokenStream[InputPointer].TokenType == TokenClass.Read || TokenStream[InputPointer].TokenType == TokenClass.Repeat))
            {
                statements.Children.Add(Statement());
                statements.Children.Add(Statements());

            }
            else
                return null;

            return statements;
        }
        Node Statement()
        {
            Node statement = new Node("Statement");

            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Idenifier)
                statement.Children.Add(Assignment_statement());
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].TokenType == TokenClass.Constant || TokenStream[InputPointer].TokenType == TokenClass.Float || TokenStream[InputPointer].TokenType == TokenClass.String))
                statement.Children.Add(Declaration_Statement());
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Write)
                statement.Children.Add(Write_statement());
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Read)
                statement.Children.Add(Read_statement());
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].TokenType == TokenClass.Repeat)
                statement.Children.Add(Repeat_statement());

            return statement;
        }

        /* Node Header()
         {
             Node header = new Node("Header");
             // write your code here to check the header sructure
             return header;
         }
         Node DeclSec()
         {
             Node declsec = new Node("DeclSec");
             // write your code here to check atleast the declare sturcure 
             // without adding procedures
             return declsec;
         }
         Node Block()
         {
             Node block = new Node("block");
             // write your code here to match statements
             return block;
         }*/

        // Implement your logic here

        public Node match(TokenClass ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].TokenType)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.ErrorList.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].TokenType.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.ErrorList.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
