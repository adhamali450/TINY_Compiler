using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public enum Token_Class
{
    Idenifier, Constant,

    Int, Float, String, 

    Read, Write, 
    Repeat, Until, 
    If, Elseif, Else, Then,
    Return, Endl,
    
    LParanthesis, RParanthesis,
    Semicolon, Comma,

    AndOp, OrOp,
    IsEqualOp, IsLessThanOp, IsGreaterThanOp, IsNotEqualOp,
    PlusOp, MinusOp, MultiplyOp, DivideOp,
    AssignmentOp,

    Comment, Undifined,
}

namespace TINY_Compiler
{
    public class Token
    {
        public string Lexeme;
        public Token_Class TokenType;
    }


    public class Scanner
    {
        private class regexHelper
        {
            /* We use /s* to ignore unwated white spaces
             * Example: getName ( age , address ) = getName(age,address)
             * If you want to cancel this, just remove any \s* you find in the expressions
             */
            public static string
                eq = @"\s*:=\s*",

                letter = @"[a-z_]",
                number = @"[0-9]",
                signedInt = @$"[\+-]?{number}+",
                signedFloat = $@"{signedInt}(\.?{number}+)*",

                literalString = $"\".*\"",
                reservedWord = $@"int|float|string|read|write|repeat|until|if|elseif|else|then|return|endl",
                commentStatement = $@"/\*(?s).*\*/",
                ident = $@"{letter}+({letter}|{number})*",
                functionCall = $@"{ident}\s*\(\s*({ident}(\s*,\s*{ident})*)*\s*\)",
                term = $@"({signedFloat}|{functionCall}|{ident})",
                arithmaticOperator = $@"\+|-|\*|/",
                equation = $@"", //TODO
                                 //expression = $@"{literalString}|{term}|{equation}", //SHOULD_BE
                expression = $@"({literalString}|{term})",
                assignmentSt = $@"{ident}{eq}{expression}\s*;",
                optionalAssignmentSt = $@"{ident}({eq}{expression})*",
                dtType = $@"(int|float|string)",

                declarationSt = $@"{dtType}\s{optionalAssignmentSt}(\s*,\s*{optionalAssignmentSt})*\s*;",
                writeSt = $@"write\s((endl)|{expression})\s*;",
                readSt = $@"read\s{ident}\s*;",
                returnSt = $@"return\s{expression}\s*;",
                statement = $@"({declarationSt}|{writeSt}|{readSt}|{returnSt}|{assignmentSt})",
                conditionalOperator = $@"(<|>|=|<>)",
                condition = $@"{ident}\s*{conditionalOperator}\s*{term}",
                booleanOperator = $@"(&&|\|\|)",
                conditionalSt = $@"{condition}(\s*{booleanOperator}\s*{condition})*",

                ifTemplate = $@"#\s{conditionalSt}\sthen\s({statement}\s*)*\send",
                elseifSt = $@"{ifTemplate.Replace("#", "elseif")}",
                elseSt = $@"else\sthen\s({statement}\s*)*\send",
                ifSt = $@"{ifTemplate.Replace("#", "if")}\s({elseifSt})*\s({elseSt})*",

                repeatSt = $@"repeat\s({statement})*\suntil\s{conditionalSt}",

                fnName = ident,
                parameter = $@"{dtType}\s{ident}",

                fnDeclarationTemplate = $@"{dtType}\s#NAME#\s*\(\s*#PARAMETERS#\)",
                fnDeclaration = fnDeclarationTemplate.Replace("#NAME#", fnName)
                    .Replace("#PARAMETERS#", $@"({parameter}(\s*,\s*{parameter})*)*\s*"),
                fnBody = $@"\{{\s*({statement}\s*)*{returnSt}\s*\}}",
                fnSt = $@"{fnDeclaration}\s*{fnBody}",
                mainFn = fnDeclarationTemplate.Replace("#NAME#", "main")
                    .Replace("#PARAMETERS#", "") + $@"\s*{fnBody}",
                program = $@"({fnSt})*\s*{mainFn}";
        }

        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();


        public Scanner()
        {
            ReservedWords.Add("INT", Token_Class.Int);
            ReservedWords.Add("FLOAT", Token_Class.Float);
            ReservedWords.Add("STRING", Token_Class.String);

            ReservedWords.Add("READ", Token_Class.Read);
            ReservedWords.Add("WRITE", Token_Class.Write);
            ReservedWords.Add("REPEAT", Token_Class.Repeat);
            ReservedWords.Add("UNITL", Token_Class.Until);
            ReservedWords.Add("IF", Token_Class.If);
            ReservedWords.Add("ELSEIF", Token_Class.Elseif);
            ReservedWords.Add("ELSE", Token_Class.Else);
            ReservedWords.Add("THEN", Token_Class.Then);
            ReservedWords.Add("RETURN", Token_Class.Return);
            ReservedWords.Add("ENDL", Token_Class.Endl);

            Operators.Add(":=", Token_Class.AssignmentOp);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("=", Token_Class.IsEqualOp);
            Operators.Add("<", Token_Class.IsLessThanOp);
            Operators.Add(">", Token_Class.IsGreaterThanOp);
            Operators.Add("<>", Token_Class.IsNotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add("&&", Token_Class.AndOp);
        }
        
        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                bool flag = false;
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;
                
                //string literal
                else if (CurrentChar == '"')
                {
                    if (j + 1 < SourceCode.Length)
                    {
                        while (j + 1 < SourceCode.Length)
                        {
                            if (SourceCode[j + 1] == ' ' || SourceCode[j + 1] == '\r' || SourceCode[j + 1] == '\n')
                            {
                                j++;
                                continue;
                            }

                            if (!char.IsLetterOrDigit(SourceCode[j + 1]) && SourceCode[j + 1] != '"')
                                break;


                            CurrentLexeme += SourceCode[j + 1];
                            j++;

                            if (SourceCode[j] == '"')
                            {

                                i = j;
                                flag = true;
                                break;
                            }

                        }
                        if (!flag)
                            Errors.Error_List.Add("Invalid String  " + CurrentLexeme);
                        else
                            FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                }
                
                //constant (number)
                else if (char.IsDigit(CurrentChar))
                {
                    int count = 0;
                    if (SourceCode.Length > j + 1)
                    {
                        while (char.IsLetterOrDigit(SourceCode[j + 1]) || SourceCode[j + 1] == '.')
                        {
                            if (char.IsLetter(SourceCode[j + 1]))
                                flag = true;
                            if (SourceCode[j + 1] == '.')
                                count++;


                            CurrentLexeme += SourceCode[j + 1].ToString();
                            j++;
                            if (SourceCode.Length == j + 1)
                                break;

                        }
                        //Errors.Error_List.Add("i " + i);
                        if (flag || count > 1)
                            Errors.Error_List.Add("Invalid number  " + CurrentLexeme);
                        else
                            FindTokenClass(CurrentLexeme);
                        i = j;

                    }
                }

                //comment statement
                else if (CurrentChar == '/' && SourceCode[i + 1] == '*')
                {

                    if (SourceCode.Length > j + 1)
                    {
                        while (j + 1 < SourceCode.Length)
                        {
                            if (SourceCode[j + 1] == ' ' || SourceCode[j + 1] == '\r' || SourceCode[j + 1] == '\n')
                            {
                                j++;
                                continue;
                            }


                            CurrentLexeme += SourceCode[j + 1];

                            j++;
                            if (SourceCode[j - 1] == '*' && SourceCode[j] == '/')
                            {
                                /*FindTokenClass(CurrentLexeme);*/

                                flag = true;

                                break;
                            }

                        }
                        if (!flag)
                            Errors.Error_List.Add("Invalid comment  " + CurrentLexeme);
                        else
                            FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                }
                
                //identifier or reserved word
                else if (char.IsLetter(CurrentChar))
                {
                    if (SourceCode[j + 1] == ' ' || SourceCode[j + 1] == '\r' || SourceCode[j + 1] == '\n')
                        FindTokenClass(CurrentLexeme);

                    else if (SourceCode.Length > j + 1)
                    {
                        while (j + 1 < SourceCode.Length)
                        {

                            if (!char.IsLetterOrDigit(SourceCode[j + 1]))
                                break;

                            CurrentLexeme += SourceCode[j + 1];

                            j++;
                            flag = true;
                        }

                        FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                }

                //operator (except for assignment operator := and boolean operators &&, ||)
                else if (Operators.ContainsKey(CurrentLexeme))
                {
                    j++;
                    i = j;
                    FindTokenClass(CurrentLexeme);
                }

                //assignment operator :=
                else if (CurrentChar == ':' && SourceCode[i + 1] == '=')
                {
                    j+=2;
                    i = j;
                    FindTokenClass(":=");
                }

                //assignment operator :=
                else if ((CurrentChar == '&' && SourceCode[i + 1] == '&') || 
                    (CurrentChar == '|' && SourceCode[i + 1] == '|'))
                {
                    CurrentLexeme += SourceCode[i + 1];
                    j += 2;
                    i = j;
                    FindTokenClass(CurrentLexeme);
                }

            }

            TINY_Compiler.TokenStream = Tokens;
        }

        void FindTokenClass(string lex)
        {
            lex = lex.ToUpper();
            Token_Class tokenClass = Token_Class.Undifined;

            //Is it a reserved word?
            if(ReservedWords.ContainsKey(lex))
            {
                tokenClass = ReservedWords[lex];
            }
            // Is it a string?
            else if (isString(lex))
            {
                tokenClass = Token_Class.String;
            }
            //Is it a comment?
            else if (isComment(lex))
            {
                tokenClass = Token_Class.Comment;
            }
            //Is it an identifier?
            else if (isIdentifier(lex))
            {
                tokenClass = Token_Class.Idenifier;
            }
            //Is it a Constant (number)?
            else if (isConstant(lex))
            {
                tokenClass = Token_Class.Constant;
            }
            //Is it an operator?
            else if (Operators.ContainsKey(lex))
            {
                tokenClass = Operators[lex];
            }
            //Is it an undefined?
            else { }

            Tokens.Add(new Token() { Lexeme = lex, TokenType = tokenClass });
        }


        bool isIdentifier(string lex)
        {
            // Check if the lex is an identifier or not.
            if (new Regex(regexHelper.ident, RegexOptions.Compiled | RegexOptions.IgnoreCase)
                .IsMatch(lex))
                return true;

            return false;
        }
        bool isConstant(string lex)
        {
            // Check if the lex is a constant (Number) or not.

            if (new Regex(regexHelper.number, RegexOptions.Compiled | RegexOptions.IgnoreCase)
                .IsMatch(lex))
                return true;

            return false;
        }
        bool isString(string lex)
        {
            // Check if the lex is a constant (Number) or not.
            if (new Regex(regexHelper.literalString, RegexOptions.Compiled | RegexOptions.IgnoreCase)
                .IsMatch(lex))
                return true;

            return false;
        }
        bool isComment(string lex)
        {
            // Check if the lex is a constant (Number) or not.

            if (new Regex(regexHelper.commentStatement, RegexOptions.Compiled | RegexOptions.IgnoreCase)
                .IsMatch(lex))
                return true;

            return false;
        }
    }
}
