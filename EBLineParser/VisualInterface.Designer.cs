namespace EBLineParser
{
    partial class VisualInterface
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.windowBox = new System.Windows.Forms.ComboBox();
            this.winLabel = new System.Windows.Forms.Label();
            this.fontLabel = new System.Windows.Forms.Label();
            this.fontBox = new System.Windows.Forms.ComboBox();
            this.lengthLabel = new System.Windows.Forms.Label();
            this.totalLengthValue = new System.Windows.Forms.Label();
            this.rowLabel = new System.Windows.Forms.Label();
            this.overflowLabel = new System.Windows.Forms.Label();
            this.rowsValue = new System.Windows.Forms.Label();
            this.overflowText = new System.Windows.Forms.TextBox();
            this.visualButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.selectedLengthValue = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.DetectUrls = false;
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(469, 82);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.SelectionChanged += new System.EventHandler(this.richTextBox1_SelectionChanged);
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // windowBox
            // 
            this.windowBox.FormattingEnabled = true;
            this.windowBox.Location = new System.Drawing.Point(98, 106);
            this.windowBox.Name = "windowBox";
            this.windowBox.Size = new System.Drawing.Size(121, 21);
            this.windowBox.TabIndex = 1;
            this.windowBox.Visible = false;
            // 
            // winLabel
            // 
            this.winLabel.AutoSize = true;
            this.winLabel.Location = new System.Drawing.Point(12, 109);
            this.winLabel.Name = "winLabel";
            this.winLabel.Size = new System.Drawing.Size(49, 13);
            this.winLabel.TabIndex = 2;
            this.winLabel.Text = "Window:";
            this.winLabel.Visible = false;
            // 
            // fontLabel
            // 
            this.fontLabel.AutoSize = true;
            this.fontLabel.Location = new System.Drawing.Point(12, 145);
            this.fontLabel.Name = "fontLabel";
            this.fontLabel.Size = new System.Drawing.Size(31, 13);
            this.fontLabel.TabIndex = 3;
            this.fontLabel.Text = "Font:";
            // 
            // fontBox
            // 
            this.fontBox.FormattingEnabled = true;
            this.fontBox.Location = new System.Drawing.Point(98, 142);
            this.fontBox.Name = "fontBox";
            this.fontBox.Size = new System.Drawing.Size(121, 21);
            this.fontBox.TabIndex = 4;
            this.fontBox.SelectedIndexChanged += new System.EventHandler(this.fontBox_SelectedIndexChanged);
            // 
            // lengthLabel
            // 
            this.lengthLabel.AutoSize = true;
            this.lengthLabel.Location = new System.Drawing.Point(12, 198);
            this.lengthLabel.Name = "lengthLabel";
            this.lengthLabel.Size = new System.Drawing.Size(70, 13);
            this.lengthLabel.TabIndex = 5;
            this.lengthLabel.Text = "Total Length:";
            // 
            // totalLengthValue
            // 
            this.totalLengthValue.AutoSize = true;
            this.totalLengthValue.Location = new System.Drawing.Point(105, 191);
            this.totalLengthValue.Name = "totalLengthValue";
            this.totalLengthValue.Size = new System.Drawing.Size(0, 13);
            this.totalLengthValue.TabIndex = 6;
            // 
            // rowLabel
            // 
            this.rowLabel.AutoSize = true;
            this.rowLabel.Location = new System.Drawing.Point(12, 219);
            this.rowLabel.Name = "rowLabel";
            this.rowLabel.Size = new System.Drawing.Size(74, 13);
            this.rowLabel.TabIndex = 7;
            this.rowLabel.Text = "Current Rows:";
            // 
            // overflowLabel
            // 
            this.overflowLabel.AutoSize = true;
            this.overflowLabel.Location = new System.Drawing.Point(12, 265);
            this.overflowLabel.Name = "overflowLabel";
            this.overflowLabel.Size = new System.Drawing.Size(76, 13);
            this.overflowLabel.TabIndex = 8;
            this.overflowLabel.Text = "Overflow Text:";
            // 
            // rowsValue
            // 
            this.rowsValue.AutoSize = true;
            this.rowsValue.Location = new System.Drawing.Point(105, 209);
            this.rowsValue.Name = "rowsValue";
            this.rowsValue.Size = new System.Drawing.Size(0, 13);
            this.rowsValue.TabIndex = 9;
            // 
            // overflowText
            // 
            this.overflowText.Location = new System.Drawing.Point(98, 262);
            this.overflowText.Name = "overflowText";
            this.overflowText.ReadOnly = true;
            this.overflowText.Size = new System.Drawing.Size(383, 20);
            this.overflowText.TabIndex = 10;
            // 
            // visualButton
            // 
            this.visualButton.Location = new System.Drawing.Point(408, 286);
            this.visualButton.Name = "visualButton";
            this.visualButton.Size = new System.Drawing.Size(75, 23);
            this.visualButton.TabIndex = 11;
            this.visualButton.Text = "Visualize";
            this.visualButton.UseVisualStyleBackColor = true;
            this.visualButton.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(225, 100);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 135);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // selectedLengthValue
            // 
            this.selectedLengthValue.AutoSize = true;
            this.selectedLengthValue.Location = new System.Drawing.Point(105, 174);
            this.selectedLengthValue.Name = "selectedLengthValue";
            this.selectedLengthValue.Size = new System.Drawing.Size(0, 13);
            this.selectedLengthValue.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Selected Length:";
            // 
            // VisualInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 322);
            this.Controls.Add(this.selectedLengthValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.visualButton);
            this.Controls.Add(this.overflowText);
            this.Controls.Add(this.rowsValue);
            this.Controls.Add(this.overflowLabel);
            this.Controls.Add(this.rowLabel);
            this.Controls.Add(this.totalLengthValue);
            this.Controls.Add(this.lengthLabel);
            this.Controls.Add(this.fontBox);
            this.Controls.Add(this.fontLabel);
            this.Controls.Add(this.winLabel);
            this.Controls.Add(this.windowBox);
            this.Controls.Add(this.richTextBox1);
            this.Name = "VisualInterface";
            this.Text = "Font Playground";
            this.Load += new System.EventHandler(this.VisualInterface_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox windowBox;
        private System.Windows.Forms.Label winLabel;
        private System.Windows.Forms.Label fontLabel;
        private System.Windows.Forms.ComboBox fontBox;
        private System.Windows.Forms.Label lengthLabel;
        private System.Windows.Forms.Label totalLengthValue;
        private System.Windows.Forms.Label rowLabel;
        private System.Windows.Forms.Label overflowLabel;
        private System.Windows.Forms.Label rowsValue;
        private System.Windows.Forms.TextBox overflowText;
        private System.Windows.Forms.Button visualButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label selectedLengthValue;
        private System.Windows.Forms.Label label2;
    }
}