using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TINY_Compiler
{
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void clearList()
        {
            ListTokens.Rows.Clear();
            TINY_Compiler.TokenStream.Clear();
        }
        
        void PrintTokens()
        {
            for (int i = 0; i < TINY_Compiler.TinyScanner.Tokens.Count; i++)
            {
                ListTokens.Rows.Add(
                    TINY_Compiler.TinyScanner.Tokens
                    .ElementAt(i).Lexeme, TINY_Compiler.TinyScanner.Tokens.ElementAt(i).TokenType);
            }
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            clearList();

            string srcCode = boxSrc.Text;
            TINY_Compiler.Start_Compiling(srcCode);
            PrintTokens();
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            clearList();
        }
    }
}
