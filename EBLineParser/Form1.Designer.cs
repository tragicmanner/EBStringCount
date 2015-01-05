namespace EBLineParser
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
            this.ccsPathBox = new System.Windows.Forms.TextBox();
            this.ccsPathButton = new System.Windows.Forms.Button();
            this.logPathBox = new System.Windows.Forms.TextBox();
            this.logPathButton = new System.Windows.Forms.Button();
            this.generateButton = new System.Windows.Forms.Button();
            this.ccsPathLabel = new System.Windows.Forms.Label();
            this.logPathLabel = new System.Windows.Forms.Label();
            this.testFontsBut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ccsPathBox
            // 
            this.ccsPathBox.Location = new System.Drawing.Point(26, 48);
            this.ccsPathBox.Name = "ccsPathBox";
            this.ccsPathBox.Size = new System.Drawing.Size(486, 20);
            this.ccsPathBox.TabIndex = 0;
            this.ccsPathBox.TextChanged += new System.EventHandler(this.ccsPathBox_TextChanged);
            this.ccsPathBox.Leave += new System.EventHandler(this.ccsPathBox_Leave);
            // 
            // ccsPathButton
            // 
            this.ccsPathButton.Location = new System.Drawing.Point(518, 46);
            this.ccsPathButton.Name = "ccsPathButton";
            this.ccsPathButton.Size = new System.Drawing.Size(44, 23);
            this.ccsPathButton.TabIndex = 1;
            this.ccsPathButton.Text = "...";
            this.ccsPathButton.UseVisualStyleBackColor = true;
            this.ccsPathButton.Click += new System.EventHandler(this.ccsPathButton_Click);
            // 
            // logPathBox
            // 
            this.logPathBox.Location = new System.Drawing.Point(26, 97);
            this.logPathBox.Name = "logPathBox";
            this.logPathBox.Size = new System.Drawing.Size(486, 20);
            this.logPathBox.TabIndex = 2;
            this.logPathBox.TextChanged += new System.EventHandler(this.logPathBox_TextChanged);
            // 
            // logPathButton
            // 
            this.logPathButton.Location = new System.Drawing.Point(518, 95);
            this.logPathButton.Name = "logPathButton";
            this.logPathButton.Size = new System.Drawing.Size(44, 23);
            this.logPathButton.TabIndex = 3;
            this.logPathButton.Text = "...";
            this.logPathButton.UseVisualStyleBackColor = true;
            this.logPathButton.Click += new System.EventHandler(this.logPathButton_Click);
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(443, 141);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(125, 23);
            this.generateButton.TabIndex = 4;
            this.generateButton.Text = "Generate Report";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // ccsPathLabel
            // 
            this.ccsPathLabel.AutoSize = true;
            this.ccsPathLabel.Location = new System.Drawing.Point(23, 32);
            this.ccsPathLabel.Name = "ccsPathLabel";
            this.ccsPathLabel.Size = new System.Drawing.Size(131, 13);
            this.ccsPathLabel.TabIndex = 5;
            this.ccsPathLabel.Text = "Path to CoilSnake Project:";
            // 
            // logPathLabel
            // 
            this.logPathLabel.AutoSize = true;
            this.logPathLabel.Location = new System.Drawing.Point(23, 81);
            this.logPathLabel.Name = "logPathLabel";
            this.logPathLabel.Size = new System.Drawing.Size(138, 13);
            this.logPathLabel.TabIndex = 6;
            this.logPathLabel.Text = "Location to save LogFile to:";
            // 
            // testFontsBut
            // 
            this.testFontsBut.Enabled = false;
            this.testFontsBut.Location = new System.Drawing.Point(26, 141);
            this.testFontsBut.Name = "testFontsBut";
            this.testFontsBut.Size = new System.Drawing.Size(93, 23);
            this.testFontsBut.TabIndex = 7;
            this.testFontsBut.Text = "Visual Tester";
            this.testFontsBut.UseVisualStyleBackColor = true;
            this.testFontsBut.Click += new System.EventHandler(this.testFontsBut_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(580, 176);
            this.Controls.Add(this.testFontsBut);
            this.Controls.Add(this.logPathLabel);
            this.Controls.Add(this.ccsPathLabel);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.logPathButton);
            this.Controls.Add(this.logPathBox);
            this.Controls.Add(this.ccsPathButton);
            this.Controls.Add(this.ccsPathBox);
            this.Name = "Form1";
            this.Text = "EB Overflow Checker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ccsPathBox;
        private System.Windows.Forms.Button ccsPathButton;
        private System.Windows.Forms.TextBox logPathBox;
        private System.Windows.Forms.Button logPathButton;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Label ccsPathLabel;
        private System.Windows.Forms.Label logPathLabel;
        private System.Windows.Forms.Button testFontsBut;

    }
}

