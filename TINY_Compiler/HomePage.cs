using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
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

            PrivateFontCollection pfc = new PrivateFontCollection();
            int fontLength = Properties.Resources.JetBrainsMono_Regular.Length;
            byte[] fontdata = Properties.Resources.JetBrainsMono_Regular;
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(fontdata, 0, data, fontLength);
            pfc.AddMemoryFont(data, fontLength);

            boxSrc.Font = new Font(pfc.Families[0], label1.Font.Size);
        }

        private void ClearTokensList()
        {
            ListTokens.Rows.Clear();
            TINY_Compiler.TokenStream.Clear();
        }

        private void ClearErrorsList()
        {
            boxErrors.Clear();
            Errors.ErrorList.Clear();
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
            for (int i = 0; i < Errors.ErrorList.Count; i++)
                boxErrors.AppendText($"[{i+1}] {Errors.ErrorList[i]}{Environment.NewLine}");
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
