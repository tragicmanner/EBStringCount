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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.totalSizeLabel = new System.Windows.Forms.Label();
            this.firstSizeLabel = new System.Windows.Forms.Label();
            this.secondSizeLabel = new System.Windows.Forms.Label();
            this.thirdSizeLabel = new System.Windows.Forms.Label();
            this.visualizeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Line:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Total Size:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "First Line Size:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Second Line Size:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Third Line Size:";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(27, 239);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(176, 13);
            this.errorLabel.TabIndex = 6;
            this.errorLabel.Text = "Current line does not exceed 3 rows";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(20, 25);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(252, 65);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // totalSizeLabel
            // 
            this.totalSizeLabel.AutoSize = true;
            this.totalSizeLabel.Location = new System.Drawing.Point(90, 102);
            this.totalSizeLabel.Name = "totalSizeLabel";
            this.totalSizeLabel.Size = new System.Drawing.Size(0, 13);
            this.totalSizeLabel.TabIndex = 8;
            // 
            // firstSizeLabel
            // 
            this.firstSizeLabel.AutoSize = true;
            this.firstSizeLabel.Location = new System.Drawing.Point(108, 133);
            this.firstSizeLabel.Name = "firstSizeLabel";
            this.firstSizeLabel.Size = new System.Drawing.Size(0, 13);
            this.firstSizeLabel.TabIndex = 9;
            // 
            // secondSizeLabel
            // 
            this.secondSizeLabel.AutoSize = true;
            this.secondSizeLabel.Location = new System.Drawing.Point(126, 158);
            this.secondSizeLabel.Name = "secondSizeLabel";
            this.secondSizeLabel.Size = new System.Drawing.Size(0, 13);
            this.secondSizeLabel.TabIndex = 10;
            // 
            // thirdSizeLabel
            // 
            this.thirdSizeLabel.AutoSize = true;
            this.thirdSizeLabel.Location = new System.Drawing.Point(114, 183);
            this.thirdSizeLabel.Name = "thirdSizeLabel";
            this.thirdSizeLabel.Size = new System.Drawing.Size(0, 13);
            this.thirdSizeLabel.TabIndex = 11;
            // 
            // visualizeButton
            // 
            this.visualizeButton.Location = new System.Drawing.Point(30, 207);
            this.visualizeButton.Name = "visualizeButton";
            this.visualizeButton.Size = new System.Drawing.Size(75, 23);
            this.visualizeButton.TabIndex = 12;
            this.visualizeButton.Text = "Visualize";
            this.visualizeButton.UseVisualStyleBackColor = true;
            this.visualizeButton.Click += new System.EventHandler(this.visualizeButton_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(284, 299);
            this.Controls.Add(this.visualizeButton);
            this.Controls.Add(this.thirdSizeLabel);
            this.Controls.Add(this.secondSizeLabel);
            this.Controls.Add(this.firstSizeLabel);
            this.Controls.Add(this.totalSizeLabel);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label totalSizeLabel;
        private System.Windows.Forms.Label firstSizeLabel;
        private System.Windows.Forms.Label secondSizeLabel;
        private System.Windows.Forms.Label thirdSizeLabel;
        private System.Windows.Forms.Button visualizeButton;
    }
}

