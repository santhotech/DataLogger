namespace DataLogger
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fldrBrwsBtn = new System.Windows.Forms.Button();
            this.strtBtn = new System.Windows.Forms.Button();
            this.fileSizeTxt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.prtTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fldrNameTxt = new System.Windows.Forms.TextBox();
            this.lgrNameTxt = new System.Windows.Forms.TextBox();
            this.ipTxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.logrList = new DataLogger.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(7, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(356, 278);
            this.panel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fldrBrwsBtn);
            this.groupBox2.Controls.Add(this.strtBtn);
            this.groupBox2.Controls.Add(this.fileSizeTxt);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.prtTxt);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.fldrNameTxt);
            this.groupBox2.Controls.Add(this.lgrNameTxt);
            this.groupBox2.Controls.Add(this.ipTxt);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.SteelBlue;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(345, 141);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logger Options";
            // 
            // fldrBrwsBtn
            // 
            this.fldrBrwsBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldrBrwsBtn.ForeColor = System.Drawing.Color.Black;
            this.fldrBrwsBtn.Location = new System.Drawing.Point(265, 84);
            this.fldrBrwsBtn.Name = "fldrBrwsBtn";
            this.fldrBrwsBtn.Size = new System.Drawing.Size(63, 20);
            this.fldrBrwsBtn.TabIndex = 3;
            this.fldrBrwsBtn.Text = "Browse";
            this.fldrBrwsBtn.UseVisualStyleBackColor = true;
            this.fldrBrwsBtn.Click += new System.EventHandler(this.fldrBrwsBtn_Click);
            // 
            // strtBtn
            // 
            this.strtBtn.ForeColor = System.Drawing.Color.Black;
            this.strtBtn.Location = new System.Drawing.Point(98, 110);
            this.strtBtn.Name = "strtBtn";
            this.strtBtn.Size = new System.Drawing.Size(75, 23);
            this.strtBtn.TabIndex = 4;
            this.strtBtn.Text = "Start Loggin";
            this.strtBtn.UseVisualStyleBackColor = true;
            this.strtBtn.Click += new System.EventHandler(this.strtBtn_Click);
            // 
            // fileSizeTxt
            // 
            this.fileSizeTxt.Location = new System.Drawing.Point(265, 54);
            this.fileSizeTxt.Name = "fileSizeTxt";
            this.fileSizeTxt.Size = new System.Drawing.Size(40, 20);
            this.fileSizeTxt.TabIndex = 1;
            this.fileSizeTxt.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(186, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Max File Size :";
            // 
            // prtTxt
            // 
            this.prtTxt.Location = new System.Drawing.Point(265, 26);
            this.prtTxt.Name = "prtTxt";
            this.prtTxt.Size = new System.Drawing.Size(63, 20);
            this.prtTxt.TabIndex = 1;
            this.prtTxt.Text = "9000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(227, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Port :";
            // 
            // fldrNameTxt
            // 
            this.fldrNameTxt.Location = new System.Drawing.Point(92, 84);
            this.fldrNameTxt.Name = "fldrNameTxt";
            this.fldrNameTxt.ReadOnly = true;
            this.fldrNameTxt.Size = new System.Drawing.Size(167, 20);
            this.fldrNameTxt.TabIndex = 1;
            this.fldrNameTxt.Text = "C:\\Users\\santhosh\\Desktop\\big\\logs";
            // 
            // lgrNameTxt
            // 
            this.lgrNameTxt.Location = new System.Drawing.Point(92, 54);
            this.lgrNameTxt.MaxLength = 15;
            this.lgrNameTxt.Name = "lgrNameTxt";
            this.lgrNameTxt.Size = new System.Drawing.Size(88, 20);
            this.lgrNameTxt.TabIndex = 2;
            this.lgrNameTxt.Text = "foo";
            // 
            // ipTxt
            // 
            this.ipTxt.Location = new System.Drawing.Point(92, 26);
            this.ipTxt.Name = "ipTxt";
            this.ipTxt.Size = new System.Drawing.Size(88, 20);
            this.ipTxt.TabIndex = 0;
            this.ipTxt.Text = "localhost";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(6, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Output Folder :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(44, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Name :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(306, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "MB";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(54, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "IP :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.logrList);
            this.groupBox1.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.SteelBlue;
            this.groupBox1.Location = new System.Drawing.Point(3, 150);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 123);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Active Loggers";
            // 
            // logrList
            // 
            this.logrList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.logrList.Location = new System.Drawing.Point(6, 19);
            this.logrList.Name = "logrList";
            this.logrList.Size = new System.Drawing.Size(333, 98);
            this.logrList.TabIndex = 0;
            this.logrList.UseCompatibleStateImageBehavior = false;
            this.logrList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "IP Address";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Port";
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Status";
            this.columnHeader4.Width = 80;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(7, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(356, 33);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Logger";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 319);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "System Insights - Data Logger";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button strtBtn;
        private System.Windows.Forms.TextBox prtTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ipTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button fldrBrwsBtn;
        private System.Windows.Forms.TextBox fldrNameTxt;
        private System.Windows.Forms.TextBox lgrNameTxt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private ListViewEx logrList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox fileSizeTxt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}

