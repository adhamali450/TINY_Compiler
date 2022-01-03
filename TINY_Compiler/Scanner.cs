using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

public enum TokenClass
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

    Comment,

    Invalid, // invalid token is like a comment missing */ or a string missing a "
    Undefined, // undefined token is a lexeme that don't have an equivalent token (i.e. $=)
}

namespace TINY_Compiler
{
    public class Token
    {
        public string Lexeme;
        public TokenClass TokenType;
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
        Dictionary<string, TokenClass> ReservedWords = new Dictionary<string, TokenClass>();
        Dictionary<string, TokenClass> Operators = new Dictionary<string, TokenClass>();


        public Scanner()
        {
            ReservedWords.Add("INT", TokenClass.Int);
            ReservedWords.Add("FLOAT", TokenClass.Float);
            ReservedWords.Add("STRING", TokenClass.String);

            ReservedWords.Add("READ", TokenClass.Read);
            ReservedWords.Add("WRITE", TokenClass.Write);
            ReservedWords.Add("REPEAT", TokenClass.Repeat);
            ReservedWords.Add("UNITL", TokenClass.Until);
            ReservedWords.Add("IF", TokenClass.If);
            ReservedWords.Add("ELSEIF", TokenClass.Elseif);
            ReservedWords.Add("ELSE", TokenClass.Else);
            ReservedWords.Add("THEN", TokenClass.Then);
            ReservedWords.Add("RETURN", TokenClass.Return);
            ReservedWords.Add("ENDL", TokenClass.Endl);

            Operators.Add(":=", TokenClass.AssignmentOp);
            Operators.Add(";", TokenClass.Semicolon);
            Operators.Add(",", TokenClass.Comma);
            Operators.Add("(", TokenClass.LParanthesis);
            Operators.Add(")", TokenClass.RParanthesis);
            Operators.Add("{", TokenClass.LCurlyBracket);
            Operators.Add("}", TokenClass.RCurlyBracket);
            Operators.Add("=", TokenClass.IsEqualOp);
            Operators.Add("<>", TokenClass.IsNotEqualOp);
            Operators.Add("<", TokenClass.IsLessThanOp);
            Operators.Add(">", TokenClass.IsGreaterThanOp);
            Operators.Add("+", TokenClass.PlusOp);
            Operators.Add("-", TokenClass.MinusOp);
            Operators.Add("*", TokenClass.MultiplyOp);
            Operators.Add("/", TokenClass.DivideOp);
            Operators.Add("||", TokenClass.OrOp);
            Operators.Add("&&", TokenClass.AndOp);
        }
        
        public void StartScanning(string sourceCode)
        {
            sourceCode += " "; // to avoid any IndexOutOfRangeException

            for (int i = 0; i < sourceCode.Length; i++)
            {
                TokenClass tokenClass = TokenClass.Undefined;

                bool returned; // True when the lexeme generates a valid token
                int j = i;
                char currentChar = sourceCode[i];
                string currentLexeme = currentChar.ToString();

                if (currentChar == ' ' || currentChar == '\r' || currentChar == '\n' || currentChar == '\t')
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
                            tokenClass = TokenClass.Invalid;
                            string errorMsg = $"\" expected at the end of {currentLexeme}";
                            Errors.ErrorList.Add(errorMsg);
                            Debug.WriteLine(errorMsg);
                        }
                        else
                        {
                            tokenClass = TokenClass.String;
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
                        {
                            tokenClass = TokenClass.Invalid;
                            Errors.ErrorList.Add($"Invalid constant  {currentLexeme}");
                        }
                        else
                            tokenClass = TokenClass.Constant;

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

                            currentLexeme += sourceCode[j + 1];
                            j++;

                            if (sourceCode[j - 1] == '*' && sourceCode[j] == '/')
                            {
                                returned = true;
                                break;
                            }

                        }

                        if (!returned)
                        {
                            tokenClass = TokenClass.Invalid;
                            Errors.ErrorList.Add($"Invalid comment {currentLexeme}");
                        }
                        else
                            tokenClass = TokenClass.Comment;

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
                        {
                            tokenClass = TokenClass.Invalid;
                            Errors.ErrorList.Add($"Invalid identifier {currentLexeme}");
                        }
                        else
                        {
                            tokenClass = getReservedWordTokenClass(currentLexeme);
                            if (tokenClass == TokenClass.Undefined)
                                tokenClass = TokenClass.Idenifier;
                        }

                        i = j;
                    }
                }

                //operator
                // we have 2 types of operators in TINY: 
                //    - single character operators (i.e. +, -, *, /, <, >)
                //    - double characters operators (i.e. !=, ||, &&)
                //    - composite characters operators (i.e. <>)


                else if (Operators.ContainsKey(currentLexeme))
                {
                    // composite characters operators 
                    if (currentChar == '<' && sourceCode[i + 1] == '>')
                    {
                        currentLexeme += sourceCode[i + 1];
                        tokenClass = Operators[currentLexeme];

                        j += 2;
                        i = j;
                    }

                    // single character operators
                    else
                    {
                        tokenClass = Operators[currentLexeme];
                    }
                }

                // double character (i.e. :=, &&, ||)
                else if ((currentChar == ':' && sourceCode[i + 1] == '=') || 
                         ((currentChar == '&' && sourceCode[i + 1] == '&')) || 
                         ((currentChar == '|' && sourceCode[i + 1] == '|')))
                {
                    currentLexeme += sourceCode[i + 1];
                    tokenClass = Operators[currentLexeme];

                    j += 2;
                    i = j;

                }


                if (tokenClass == TokenClass.Invalid)
                    continue;
                else if(tokenClass == TokenClass.Undefined)
                    Errors.ErrorList.Add($"Un-identified token: {currentLexeme}");
                else // valid token
                    Tokens.Add(new Token() { Lexeme = currentLexeme, TokenType = tokenClass });
            }

            TINY_Compiler.TokenStream = Tokens;
        }

        TokenClass getReservedWordTokenClass(string lexeme)
        {
            if (ReservedWords.ContainsKey(lexeme.ToUpper()))
                return ReservedWords[lexeme.ToUpper()];

            return TokenClass.Undefined;
        }
    }
}
