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

        private void ClearTokensList()
        {
            ListTokens.Rows.Clear();
            TINY_Compiler.TokenStream.Clear();
        }

        private void ClearErrorsList()
        {
            boxErrors.Clear();
            Errors.Error_List.Clear();
        }

        void PopulateTokens()
        {
            for (int i = 0; i < TINY_Compiler.TinyScanner.Tokens.Count; i++)
            {
                ListTokens.Rows.Add(
                    TINY_Compiler.TinyScanner.Tokens
                    .ElementAt(i).Lexeme, TINY_Compiler.TinyScanner.Tokens.ElementAt(i).TokenType);
            }
        }

        void PopulateErrors()
        {
            for (int i = 0; i < Errors.Error_List.Count; i++)
                boxErrors.AppendText($"{Environment.NewLine}[{i+1}] {Errors.Error_List[i]}");
        }


        //https://www.youtube.com/watch?v=gWWthx5Ow0Q&t=345s
        private void btnCompile_Click(object sender, EventArgs e)
        {
            ClearTokensList();
            ClearErrorsList();

            string srcCode = boxSrc.Text;
            TINY_Compiler.Start_Compiling(srcCode);
            PopulateTokens();
            PopulateErrors();
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            ClearTokensList();
            ClearErrorsList();
        }

    }
}
