using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    LCurlyBracket, RCurlyBracket,
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
            Operators.Add("{", Token_Class.LCurlyBracket);
            Operators.Add("}", Token_Class.RCurlyBracket);
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
        
        public void StartScanning(string sourceCode)
        {
            for (int i = 0; i < sourceCode.Length; i++)
            {
                Token_Class tokenClass = Token_Class.Undifined;

                bool returned = false; // True when the lexeme generates a valid token
                int j = i;
                char currentChar = sourceCode[i];
                string currentLexeme = currentChar.ToString();

                if (currentChar == ' ' || currentChar == '\r' || currentChar == '\n')
                    continue;

                //string literal
                else if (currentChar == '"')
                {
                    returned = false; // we assume string is not valid until terminated with "
                    Debug.WriteLine("Entering a string lexeme");

                    if (j + 1 < sourceCode.Length)
                    {
                        while (j + 1 < sourceCode.Length)
                        {
                            if (sourceCode[j + 1] == '\n')
                            {
                                j++;
                                continue;
                            }

                            // TODO: remove if not needed
                            /*if (!char.IsLetterOrDigit(SourceCode[j + 1]) && SourceCode[j + 1] != '"')
                                break;*/

                            currentLexeme += sourceCode[j + 1];
                            j++;

                            if (sourceCode[j] == '"')
                            {
                                returned = true;
                                break;
                            }

                        }
                        if (!returned)
                        {
                            string errorMsg = "\" expected at the end of " + currentLexeme;
                            Errors.Error_List.Add(errorMsg);
                            Debug.WriteLine(errorMsg);
                        }
                        else
                        {

                            // TODO: Validate difference

                            tokenClass = Token_Class.String;
                            Tokens.Add(new Token() { Lexeme = currentLexeme, TokenType = tokenClass });

                            //FindTokenClass(currentLexeme);
                        }
                        
                        i = j;
                    }
                }
                
                //constant (number)
                else if (char.IsDigit(currentChar))
                {
                    returned = true; // we assume number is valid Unitl otherwise 
                    Debug.WriteLine("Entering a constant(number) lexeme");

                    int points_count = 0;
                    if (j + 1 < sourceCode.Length)
                    {
                        while (char.IsLetterOrDigit(sourceCode[j + 1]) || sourceCode[j + 1] == '.')
                        {
                            if (char.IsLetter(sourceCode[j + 1]))
                                returned = false;

                            if (sourceCode[j + 1] == '.')
                                points_count++;

                            currentLexeme += sourceCode[j + 1].ToString();
                            j++;

                            if (j + 1 == sourceCode.Length)
                                break;

                        }


                        if (points_count > 1)
                            returned = false;

                        if (!returned)
                            Errors.Error_List.Add("Invalid constant  " + currentLexeme);
                        else
                            FindTokenClass(currentLexeme);

                        i = j;

                    }
                }

                //comment statement
                else if (currentChar == '/' && sourceCode[i + 1] == '*')
                {
                    returned = false; // we assume comment is not valid until terminated with */
                    Debug.WriteLine("Entering a comment lexeme");

                    if (sourceCode.Length > j + 1)
                    {
                        while (j + 1 < sourceCode.Length)
                        {
                            //any spaces or new lines shouldn't be ignored when it comes to comments

                            //if (SourceCode[j + 1] == ' ' || SourceCode[j + 1] == '\r' || SourceCode[j + 1] == '\n')
                            //{
                            //    j++;
                            //    continue;
                            //}

                            currentLexeme += sourceCode[j + 1];
                            j++;

                            if (sourceCode[j - 1] == '*' && sourceCode[j] == '/')
                            {
                                returned = true;
                                break;
                            }

                        }

                        if (!returned)
                            Errors.Error_List.Add("Invalid comment " + currentLexeme);
                        else
                            FindTokenClass(currentLexeme);
                        
                        i = j;
                    }
                }
                
                //identifier or reserved word
                else if (char.IsLetter(currentChar))
                {
                    returned = true; // we assume ident or reserved-word is valid until we encounter a symbol */
                    Debug.WriteLine("Entering reserved-word OR identifier lexeme");

                    if (j + 1 < sourceCode.Length)
                    {
                        while (j + 1 < sourceCode.Length)
                        {
                            // when we encounter a space, carrige, new line or a symbol, that's it with our res-word or ident
                            if (sourceCode[j + 1] == ' ' || 
                                sourceCode[j + 1] == '\r' || 
                                sourceCode[j + 1] == '\n' ||
                                (!char.IsLetterOrDigit(sourceCode[j + 1])))
                            {
                                break;
                            }

                            currentLexeme += sourceCode[j + 1];
                            j++;
                        }

                        if (!returned)
                            Errors.Error_List.Add("Invalid identifier  " + currentLexeme);
                        else
                            FindTokenClass(currentLexeme);

                        i = j;
                    }
                }

                //operator
                // we have 2 types of operators in TINY: 
                //    - single character operators (i.e. +, -, *, /)
                //    - double characters operators (i.e. !=, ||, &&)

                // single character operators (i.e. +, -, *, /)
                else if (Operators.ContainsKey(currentLexeme))
                {
                    FindTokenClass(currentLexeme);
                }

                // assignment operator :=
                else if (currentChar == ':' && sourceCode[i + 1] == '=')
                {
                    j++;
                    i = j;
                    FindTokenClass(":=");
                }

                // &&, ||
                else if ((currentChar == '&' && sourceCode[i + 1] == '&') ||
                    (currentChar == '|' && sourceCode[i + 1] == '|'))
                {
                    currentLexeme += sourceCode[i + 1];
                    j += 2;
                    i = j;
                    FindTokenClass(currentLexeme);
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
