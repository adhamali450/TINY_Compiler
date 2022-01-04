using System;
using System.Collections.Generic;
using System.Text;

namespace TINY_Compiler
{
    public class TINY_Compiler
    {
        public static Scanner TinyScanner = new Scanner();
        public static Parser TinyParser = new Parser();

        public static List<string> lexemes = new List<string>();
        public static List<Token> TokenStream = new List<Token>();

        public static Node treeroot;

        public static void Start_Compiling(string SourceCode) //character by character
        {
            //Scanner

            TinyScanner.StartScanning(SourceCode);
            //Parser
            TinyParser.StartParsing(TokenStream);
            treeroot = TinyParser.root;

            // Sematic Analysis
        }

        static void SplitLexemes(string SourceCode)
        {
            string[] Lexemes_arr = SourceCode.Split(' ');
            for (int i = 0; i < Lexemes_arr.Length; i++)
            {
                if (Lexemes_arr[i].Contains("\r\n"))
                {
                    Lexemes_arr[i] = Lexemes_arr[i].Replace("\r\n", string.Empty);
                }
                lexemes.Add(Lexemes_arr[i]);
            }

        }
    }
}
