using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TINY_Compiler
{
    class Compiler
    {
        int main()
        {
            /* We use /s* to ignore unwated white spaces
             * Example: getName ( age , address ) = getName(age,address)
             * If you want to cancel this, just remove any \s* you find in the expressions
             */
            string
                eq = @"\s*:=\s*",

                letter = @"[a-z_]",
                number = @"[0-9]",
                signedInt = @$"[\+-]?{number}+",
                signedFloat = $@"{signedInt}(\.?{number}+)*",

                literalString = $"\".*\"",
                reservedWord = $@":int|float|string|read|write|repeat|until|if|elseif|else|then|return|endl",
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
            return 0;
        }
    }
}
