using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TINY_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;

        public Node(string name)
        {
            Name = name;
        }
    }

    public class Parser
    {
        int tokenIndex = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.tokenIndex = 0;
            this.TokenStream = TokenStream;
            root = Program();
            return root;
        }
        Node Program()
        {
            Node node = new Node("Program");

            node.Children.Add(Functions());
            node.Children.Add(MainFunction());

            return node;
        }

        private Node Functions()
        {
            Node node = new Node("Functions");

            if (tokenIndex < TokenStream.Count)
            {
                if (TokenStream[tokenIndex + 1].Lexeme != "main")
                {
                    if (equalTokenClasses(TokenStream[tokenIndex].TokenType, TokenClass.Int, TokenClass.Float, TokenClass.String))
                    {
                        node.Children.Add(FunctionStatement());
                        node.Children.Add(Functions());
                    }
                }
            }


            return node;
        }

        private Node FunctionStatement()
        {
            Node node = new Node("FunctionSt");

            node.Children.Add(FunctionDeclaration());
            node.Children.Add(FunctionBody());

            return node;
        }

        private Node FunctionDeclaration()
        {
            Node node = new Node("FnDecl");

            node.Children.Add(DataType());
            node.Children.Add(match(TokenClass.Idenifier));
            node.Children.Add(match(TokenClass.LParanthesis));
            node.Children.Add(Parameters());
            node.Children.Add(match(TokenClass.RParanthesis));

            return node;
        }

        private Node DataType()
        {
            Node node = new Node("DtType");

            if (tokenIndex < TokenStream.Count)
            {
                var tokenType = TokenStream[tokenIndex].TokenType;
                if (tokenType == TokenClass.Int)
                    node.Children.Add(match(TokenClass.Int));
                else if (tokenType == TokenClass.Float)
                    node.Children.Add(match(TokenClass.Float));
                else if (tokenType == TokenClass.String)
                    node.Children.Add(match(TokenClass.String));
            }

            return node;
        }


        private Node FunctionBody() // FnBody -> { Statements ReturnSt }
        {
            Node node = new Node("FnBody");

            node.Children.Add(match(TokenClass.LCurlyBracket)); // {

            node.Children.Add(Statements());

            node.Children.Add(ReturnStatement());

            node.Children.Add(match(TokenClass.RCurlyBracket)); // }

            return node;
        }

        private Node Statements()
        {
            Node node = new Node("Statements");

            if (tokenIndex < TokenStream.Count)
            {
                while (TokenStream[tokenIndex].TokenType == TokenClass.Comment) // ignoring comments
                {
                    tokenIndex++;
                }

                if (IsStatement(TokenStream[tokenIndex].TokenType))
                {
                    node.Children.Add(Statement());
                    node.Children.Add(Statements());
                }
                else
                    return null;
            }
            else
                return null;

            return node;
        }

        private bool IsStatement(TokenClass ExpectedToken)
        {
            bool found = false;
            while (TokenStream[tokenIndex].TokenType == TokenClass.Comment)
                tokenIndex++;

            if (equalTokenClasses(TokenStream[tokenIndex].TokenType, TokenClass.Int, TokenClass.Float, TokenClass.String)
            || ExpectedToken == TokenClass.If
            || ExpectedToken == TokenClass.Write
            || ExpectedToken == TokenClass.Read
            || ExpectedToken == TokenClass.Repeat
            || ExpectedToken == TokenClass.Idenifier
            || ExpectedToken == TokenClass.Return
            )
            {
                found = true;
            }
            else
                found = false;

            return found;
        }

        private Node Statement()
        {
            Node node = new Node("Statement");

            if (tokenIndex < TokenStream.Count)
            {
                var tokenType = TokenStream[tokenIndex].TokenType;

                if (equalTokenClasses(tokenType, TokenClass.Int, TokenClass.Float, TokenClass.String))
                    node.Children.Add(DeclarationStatement());

                else if (equalTokenClasses(tokenType, TokenClass.Idenifier))
                    node.Children.Add(AssignmentStatement());

                else if (equalTokenClasses(tokenType, TokenClass.Repeat))
                    node.Children.Add(RepeatStatement());

                else if (equalTokenClasses(tokenType, TokenClass.Read))
                    node.Children.Add(ReadStatement());

                else if (equalTokenClasses(tokenType, TokenClass.Write))
                    node.Children.Add(WriteStatement());

                else if (equalTokenClasses(tokenType, TokenClass.If))
                    node.Children.Add(IfStatement());

                else if (equalTokenClasses(tokenType, TokenClass.Comment))
                    node.Children.Add(match(TokenClass.Comment));
            }

            return node;
        }

        private Node Assignment()
        {
            Node node = new Node("Assignment");

            if (equalTokenClasses(TokenStream[tokenIndex].TokenType, TokenClass.AssignmentOp))
            {
                node.Children.Add(match(TokenClass.AssignmentOp)); // :=
                node.Children.Add(Expression()); // Exp
            }

            return node;
        }

        #region Statement Types (assignment, declaration, read, write, repeat, if, comment)

        private Node AssignmentStatement()
        {
            Node node = new Node("AssignmentSt");

            node.Children.Add(match(TokenClass.Idenifier)); // ident
            node.Children.Add(match(TokenClass.AssignmentOp)); // :=
            node.Children.Add(Expression()); // Exp
            node.Children.Add(match(TokenClass.Semicolon)); // ;

            return node;
        }

        private Node DeclarationStatement()
        {
            Node node = new Node("DeclarationSt");

            node.Children.Add(DataType()); // int
            node.Children.Add(Declarations()); // x:=6, y
            node.Children.Add(match(TokenClass.Semicolon)); // ;

            return node;
        }

        private Node ReadStatement()
        {
            Node node = new Node("ReadSt");

            node.Children.Add(match(TokenClass.Read)); // read
            node.Children.Add(match(TokenClass.Idenifier)); // ident
            node.Children.Add(match(TokenClass.Semicolon)); // ;


            return node;
        }

        private Node WriteStatement()
        {
            Node node = new Node("WriteSt");

            node.Children.Add(match(TokenClass.Write)); // write

            var tokenType = TokenStream[tokenIndex].TokenType;

            if (equalTokenClasses(tokenType, TokenClass.Endl)) // endl
                node.Children.Add(match(TokenClass.Endl));
            else
                node.Children.Add(Expression()); // Exp

            node.Children.Add(match(TokenClass.Semicolon)); // ;

            return node;
        }

        private Node RepeatStatement()
        {
            Node node = new Node("RepeatSt");

            node.Children.Add(match(TokenClass.Repeat)); // repeat
            node.Children.Add(Statements()); // statements
            node.Children.Add(match(TokenClass.Until)); //until
            node.Children.Add(ConditionalStatement()); // cond

            return node;
        }

        private Node ConditionalStatement()
        {
            Node node = new Node("ConditionalSt");

            node.Children.Add(Condition());
            node.Children.Add(ConditionalStatement_Dash());

            return node;
        }

        private Node Condition()
        {
            Node node = new Node("Condition");

            node.Children.Add(match(TokenClass.Idenifier)); //ident
            node.Children.Add(ConditionalOperator()); //ConditionalOp
            node.Children.Add(Term()); //Term

            return node;
        }

        private Node ConditionalOperator()
        {
            Node node = new Node("ConditionalOp");

            if (tokenIndex < TokenStream.Count)
            {
                var tokenType = TokenStream[tokenIndex].TokenType;
                if (tokenType == TokenClass.IsGreaterThanOp)
                    node.Children.Add(match(TokenClass.IsGreaterThanOp));
                else if (tokenType == TokenClass.IsLessThanOp)
                    node.Children.Add(match(TokenClass.IsLessThanOp));
                else if (tokenType == TokenClass.IsEqualOp)
                    node.Children.Add(match(TokenClass.IsEqualOp));
                else if (tokenType == TokenClass.IsNotEqualOp)
                    node.Children.Add(match(TokenClass.IsNotEqualOp));
            }

            return node;
        }

        private Node ConditionalStatement_Dash()
        {
            Node node = new Node("ConditionalSt'");

            if (tokenIndex < TokenStream.Count)
            {

                var tokenClass = TokenStream[tokenIndex].TokenType;
                if (equalTokenClasses(tokenClass, TokenClass.AndOp))
                {
                    node.Children.Add(match(TokenClass.AndOp));
                    node.Children.Add(Condition());
                    node.Children.Add(ConditionalStatement_Dash());
                }
                else if (equalTokenClasses(tokenClass, TokenClass.OrOp))
                {
                    node.Children.Add(match(TokenClass.OrOp));
                    node.Children.Add(Condition());
                    node.Children.Add(ConditionalStatement_Dash());
                }
            }

            return node;
        }

        private Node IfStatement()
        {
            Node node = new Node("IfSt");

            node.Children.Add(match(TokenClass.If)); // if
            node.Children.Add(ConditionalStatement()); // cond
            node.Children.Add(match(TokenClass.Then)); // then
            node.Children.Add(Statements()); // statements
            node.Children.Add(match(TokenClass.End)); // end

            node.Children.Add(ElseifStatement()); // else if

            node.Children.Add(ElseStatement()); // statements

            return node;
        }

        private Node ElseifStatement()
        {
            Node node = new Node("ElseifSt");

            if (tokenIndex < TokenStream.Count)
            {
                if (equalTokenClasses(TokenStream[tokenIndex].TokenType, TokenClass.Elseif))
                {
                    node.Children.Add(match(TokenClass.Elseif)); // else if
                    node.Children.Add(ConditionalStatement()); // cond
                    node.Children.Add(match(TokenClass.Then)); // then
                    node.Children.Add(Statements()); // statements
                    node.Children.Add(match(TokenClass.End)); // end

                    node.Children.Add(ElseifStatement()); // else if
                }
            }

            return node;
        }

        private Node ElseStatement()
        {
            Node node = new Node("ElseSt");

            if (tokenIndex < TokenStream.Count)
            {
                if (equalTokenClasses(TokenStream[tokenIndex].TokenType, TokenClass.Else))
                {
                    node.Children.Add(match(TokenClass.Else)); // else
                    node.Children.Add(Statements()); // statements
                    node.Children.Add(match(TokenClass.End)); // end
                }
            }

            return node;
        }

        #endregion

        private Node Statements_Dash()
        {
            Node node = new Node("A_Statements");

            if (tokenIndex < TokenStream.Count)
            {
                if (equalTokenClasses(TokenStream[tokenIndex].TokenType,
                    TokenClass.Int, TokenClass.Float, TokenClass.String,
                    TokenClass.Read, TokenClass.Write, TokenClass.Repeat,
                    TokenClass.If, TokenClass.Comment, TokenClass.Idenifier) && TokenStream[tokenIndex].TokenType != TokenClass.Until)
                {
                    node.Children.Add(Statement());
                    node.Children.Add(Statements_Dash());
                }
            }

            return node;
        }

        private Node ReturnStatement()
        {
            Node node = new Node("ReturnSt");

            node.Children.Add(match(TokenClass.Return)); // return

            node.Children.Add(Expression()); // Exp

            node.Children.Add(match(TokenClass.Semicolon)); // ;

            return node;
        }

        private Node Expression()
        {
            Node node = new Node("Exp");

            if (tokenIndex < TokenStream.Count)
            {
                var tokenClass = TokenStream[tokenIndex + 1].TokenType;

                if (equalTokenClasses(tokenClass, TokenClass.String)) // string
                {
                    node.Children.Add(match(TokenClass.String));
                }
                else // equation
                    node.Children.Add(Equation());
            }

            return node;
        }

        private Node Term()
        {
            Node node = new Node("Term");

            if (tokenIndex < TokenStream.Count)
            {
                var tokenClass = TokenStream[tokenIndex].TokenType;
                if (equalTokenClasses(tokenClass, TokenClass.Constant)) // num
                {
                    node.Children.Add(match(TokenClass.Constant));
                }
                else if (equalTokenClasses(tokenClass, TokenClass.Idenifier)) // ident
                {
                    node.Children.Add(match(TokenClass.Idenifier));
                }
                else
                    node.Children.Add(FunctionCall());
            }

            return node;
        }

        private Node FunctionCall()
        {
            Node node = new Node("FnCall");

            node.Children.Add(match(TokenClass.Idenifier));
            node.Children.Add(match(TokenClass.LParanthesis));
            node.Children.Add(Arguments());
            node.Children.Add(match(TokenClass.RParanthesis));

            return node;
        }

        private Node Declarations()
        {
            Node node = new Node("Decls");

            node.Children.Add(Declaration());
            node.Children.Add(SecondDecls());

            return node;
        }

        private Node Declaration()
        {
            Node node = new Node("Decl");

            node.Children.Add(match(TokenClass.Idenifier));
            node.Children.Add(Assignment());

            return node;
        }

        private Node SecondDecls()
        {
            Node node = new Node("SDecls");

            var tokenClass = TokenStream[tokenIndex].TokenType;
            if (equalTokenClasses(tokenClass, TokenClass.Comma))
            {
                node.Children.Add(match(TokenClass.Comma));

                node.Children.Add(Declaration());
                node.Children.Add(SecondDecls());
            }

            return node;
        }

        private Node Parameters()
        {
            Node node = new Node("Params");

            var tokenClass = TokenStream[tokenIndex].TokenType;
            if (equalTokenClasses(tokenClass, TokenClass.Int, TokenClass.Float, TokenClass.String))
            {
                node.Children.Add(ParamsList());
            }

            return node;
        }

        private Node ParamsList()
        {
            Node node = new Node("ParamsList");

            node.Children.Add(DataType());
            node.Children.Add(match(TokenClass.Idenifier));
            node.Children.Add(SecondParams());

            return node;
        }

        private Node SecondParams()
        {
            Node node = new Node("SParams");

            var tokenClass = TokenStream[tokenIndex].TokenType;
            if (equalTokenClasses(tokenClass, TokenClass.Comma))
            {
                node.Children.Add(match(TokenClass.Comma));
                node.Children.Add(DataType());
                node.Children.Add(match(TokenClass.Idenifier));

                node.Children.Add(SecondParams());
            }

            return node;
        }

        private Node Arguments()
        {
            Node node = new Node("Args");

            if (tokenIndex < TokenStream.Count)
            {
                var tokenClass = TokenStream[tokenIndex].TokenType;
                if (equalTokenClasses(tokenClass, TokenClass.Idenifier))
                {
                    node.Children.Add(ArgumentsList());
                }
            }

            return node;
        }

        private Node ArgumentsList()
        {
            Node node = new Node("ArgsList");

            node.Children.Add(match(TokenClass.Idenifier));
            node.Children.Add(SecondArguments());

            return node;
        }

        private Node SecondArguments()
        {
            Node node = new Node("SArgs");

            var tokenClass = TokenStream[tokenIndex].TokenType;
            if (equalTokenClasses(tokenClass, TokenClass.Idenifier))
            {
                node.Children.Add(match(TokenClass.Comma));
                node.Children.Add(match(TokenClass.Idenifier));
                node.Children.Add(SecondArguments());
            }

            return node;
        }

        private Node Equation()
        {
            // Term ArithmeticOp Equation | (Equation) | Term
            Node node = new Node("Equation");

            var tokenType = TokenStream[tokenIndex].TokenType;
            if (tokenType == TokenClass.LParanthesis)
            {
                node.Children.Add(match(TokenClass.LParanthesis));
                node.Children.Add(Equation());
                node.Children.Add(match(TokenClass.RParanthesis));
            }
            else
            {
                node.Children.Add(Term());

                if (equalTokenClasses(TokenStream[tokenIndex].TokenType, TokenClass.PlusOp,
                    TokenClass.MinusOp, TokenClass.MultiplyOp, TokenClass.DivideOp))
                {
                    node.Children.Add(ArithmeticOperator());
                    node.Children.Add(Equation());
                }
            }

            return node;
        }

        private Node ArithmeticOperator()
        {
            Node node = new Node("ArithmeticOp");

            if (tokenIndex < TokenStream.Count)
            {
                var tokenType = TokenStream[tokenIndex].TokenType;
                if (tokenType == TokenClass.PlusOp)
                    node.Children.Add(match(TokenClass.PlusOp));

                else if (tokenType == TokenClass.MinusOp)
                    node.Children.Add(match(TokenClass.MinusOp));

                else if (tokenType == TokenClass.MultiplyOp)
                    node.Children.Add(match(TokenClass.MultiplyOp));

                else if (tokenType == TokenClass.DivideOp)
                    node.Children.Add(match(TokenClass.DivideOp));
            }

            return node;
        }

        private Node MainFunction()
        {
            Node node = new Node("MainFn");

            node.Children.Add(DataType());
            node.Children.Add(match(TokenClass.Idenifier));
            node.Children.Add(match(TokenClass.LParanthesis));
            node.Children.Add(match(TokenClass.RParanthesis));
            node.Children.Add(FunctionBody());

            return node;
        }

        public Node match(TokenClass ExpectedToken)
        {
            if (tokenIndex == 18)
            {
                Debug.WriteLine("18");
            }
            if (tokenIndex < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[tokenIndex].TokenType)
                {
                    //Node newNode = new Node(ExpectedToken.ToString());
                    Node node = new Node(TokenStream[tokenIndex].Lexeme);

                    tokenIndex++;

                    return node;
                }

                else
                {
                    Errors.ErrorList.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[tokenIndex].TokenType.ToString() +
                        "  found\r\n");
                    tokenIndex++;
                    return null;
                }
            }
            else
            {
                Errors.ErrorList.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                tokenIndex++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree = treeRoot;
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

        private static bool equalTokenClasses(TokenClass currentClass, params TokenClass[] classes)
        {
            foreach (var tclass in classes)
            {
                if (currentClass == tclass)
                    return true;
            }

            return false;
        }
    }
}
