
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.label1.ForeColor = System.Drawing.Color.White;
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
            this.boxSrc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.boxSrc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.boxSrc.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.boxSrc.ForeColor = System.Drawing.Color.White;
            this.boxSrc.Location = new System.Drawing.Point(12, 43);
            this.boxSrc.Multiline = true;
            this.boxSrc.Name = "boxSrc";
            this.boxSrc.Size = new System.Drawing.Size(619, 436);
            this.boxSrc.TabIndex = 1;
            // 
            // ListTokens
            // 
            this.ListTokens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListTokens.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ListTokens.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ListTokens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListTokens.ColumnHeadersVisible = false;
            this.ListTokens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.lexemes,
            this.tokenClasses});
            this.ListTokens.EnableHeadersVisualStyles = false;
            this.ListTokens.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ListTokens.Location = new System.Drawing.Point(640, 43);
            this.ListTokens.Name = "ListTokens";
            this.ListTokens.RowHeadersWidth = 51;
            this.ListTokens.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ListTokens.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ListTokens.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ListTokens.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.ListTokens.RowTemplate.Height = 25;
            this.ListTokens.RowTemplate.ReadOnly = true;
            this.ListTokens.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ListTokens.Size = new System.Drawing.Size(303, 436);
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
            this.btnCompile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnCompile.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(91)))), ((int)(((byte)(153)))));
            this.btnCompile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompile.ForeColor = System.Drawing.Color.White;
            this.btnCompile.Location = new System.Drawing.Point(484, 488);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(147, 30);
            this.btnCompile.TabIndex = 3;
            this.btnCompile.Text = "Compile";
            this.btnCompile.UseVisualStyleBackColor = false;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 534);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "Error List";
            // 
            // boxErrors
            // 
            this.boxErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.boxErrors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.boxErrors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.boxErrors.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.boxErrors.ForeColor = System.Drawing.Color.Red;
            this.boxErrors.Location = new System.Drawing.Point(12, 560);
            this.boxErrors.Multiline = true;
            this.boxErrors.Name = "boxErrors";
            this.boxErrors.ReadOnly = true;
            this.boxErrors.Size = new System.Drawing.Size(931, 133);
            this.boxErrors.TabIndex = 5;
            // 
            // btnClearList
            // 
            this.btnClearList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnClearList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClearList.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(91)))), ((int)(((byte)(153)))));
            this.btnClearList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearList.ForeColor = System.Drawing.Color.White;
            this.btnClearList.Location = new System.Drawing.Point(796, 488);
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.Size = new System.Drawing.Size(147, 30);
            this.btnClearList.TabIndex = 6;
            this.btnClearList.Text = "Clear";
            this.btnClearList.UseVisualStyleBackColor = false;
            this.btnClearList.Click += new System.EventHandler(this.btnClearList_Click);
            // 
            // HomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
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

