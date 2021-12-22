
namespace TINY_Compiler
{
    partial class HomePage
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.boxSrc = new System.Windows.Forms.TextBox();
            this.ListTokens = new System.Windows.Forms.DataGridView();
            this.lexemes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tokenClasses = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCompile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.boxErrors = new System.Windows.Forms.TextBox();
            this.btnClearList = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ListTokens)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "TINY Compiler";
            // 
            // boxSrc
            // 
            this.boxSrc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.boxSrc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.boxSrc.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.boxSrc.Location = new System.Drawing.Point(12, 43);
            this.boxSrc.Multiline = true;
            this.boxSrc.Name = "boxSrc";
            this.boxSrc.Size = new System.Drawing.Size(621, 436);
            this.boxSrc.TabIndex = 1;
            // 
            // ListTokens
            // 
            this.ListTokens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListTokens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListTokens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.lexemes,
            this.tokenClasses});
            this.ListTokens.Location = new System.Drawing.Point(639, 43);
            this.ListTokens.Name = "ListTokens";
            this.ListTokens.ReadOnly = true;
            this.ListTokens.RowHeadersWidth = 51;
            this.ListTokens.RowTemplate.Height = 29;
            this.ListTokens.Size = new System.Drawing.Size(304, 436);
            this.ListTokens.TabIndex = 2;
            // 
            // lexemes
            // 
            this.lexemes.HeaderText = "Lexeme";
            this.lexemes.MinimumWidth = 6;
            this.lexemes.Name = "lexemes";
            this.lexemes.ReadOnly = true;
            this.lexemes.Width = 125;
            // 
            // tokenClasses
            // 
            this.tokenClasses.HeaderText = "Token Class";
            this.tokenClasses.MinimumWidth = 6;
            this.tokenClasses.Name = "tokenClasses";
            this.tokenClasses.ReadOnly = true;
            this.tokenClasses.Width = 125;
            // 
            // btnCompile
            // 
            this.btnCompile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompile.Location = new System.Drawing.Point(486, 484);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(147, 30);
            this.btnCompile.TabIndex = 3;
            this.btnCompile.Text = "Compile";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(12, 550);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "Error List";
            // 
            // boxErrors
            // 
            this.boxErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.boxErrors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.boxErrors.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.boxErrors.ForeColor = System.Drawing.Color.Red;
            this.boxErrors.Location = new System.Drawing.Point(12, 582);
            this.boxErrors.Multiline = true;
            this.boxErrors.Name = "boxErrors";
            this.boxErrors.ReadOnly = true;
            this.boxErrors.Size = new System.Drawing.Size(931, 111);
            this.boxErrors.TabIndex = 5;
            // 
            // btnClearList
            // 
            this.btnClearList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearList.Location = new System.Drawing.Point(796, 484);
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.Size = new System.Drawing.Size(147, 30);
            this.btnClearList.TabIndex = 6;
            this.btnClearList.Text = "Clear";
            this.btnClearList.UseVisualStyleBackColor = true;
            this.btnClearList.Click += new System.EventHandler(this.btnClearList_Click);
            // 
            // HomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 705);
            this.Controls.Add(this.btnClearList);
            this.Controls.Add(this.boxErrors);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCompile);
            this.Controls.Add(this.ListTokens);
            this.Controls.Add(this.boxSrc);
            this.Controls.Add(this.label1);
            this.Name = "HomePage";
            this.Text = "Tiny Compiler (beta)";
            ((System.ComponentModel.ISupportInitialize)(this.ListTokens)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox boxSrc;
        private System.Windows.Forms.DataGridView ListTokens;
        private System.Windows.Forms.DataGridViewTextBoxColumn lexemes;
        private System.Windows.Forms.DataGridViewTextBoxColumn tokenClasses;
        private System.Windows.Forms.Button btnCompile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox boxErrors;
        private System.Windows.Forms.Button btnClearList;
    }
}

