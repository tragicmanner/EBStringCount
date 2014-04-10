namespace EBLineParser
{
    partial class Visualizer
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
            this.line1 = new System.Windows.Forms.Label();
            this.line2 = new System.Windows.Forms.Label();
            this.line3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // line1
            // 
            this.line1.AutoSize = true;
            this.line1.BackColor = System.Drawing.Color.Transparent;
            this.line1.Font = new System.Drawing.Font("EBMain", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.line1.ForeColor = System.Drawing.Color.White;
            this.line1.Location = new System.Drawing.Point(12, 10);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(0, 12);
            this.line1.TabIndex = 0;
            // 
            // line2
            // 
            this.line2.AutoSize = true;
            this.line2.BackColor = System.Drawing.Color.Transparent;
            this.line2.Font = new System.Drawing.Font("EBMain", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.line2.ForeColor = System.Drawing.Color.White;
            this.line2.Location = new System.Drawing.Point(12, 26);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(0, 12);
            this.line2.TabIndex = 1;
            // 
            // line3
            // 
            this.line3.AutoSize = true;
            this.line3.BackColor = System.Drawing.Color.Transparent;
            this.line3.Font = new System.Drawing.Font("EBMain", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.line3.ForeColor = System.Drawing.Color.White;
            this.line3.Location = new System.Drawing.Point(12, 42);
            this.line3.Name = "line3";
            this.line3.Size = new System.Drawing.Size(0, 12);
            this.line3.TabIndex = 2;
            // 
            // Visualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::EBLineParser.Properties.Resources.EBTextTemplate;
            this.ClientSize = new System.Drawing.Size(152, 64);
            this.Controls.Add(this.line3);
            this.Controls.Add(this.line2);
            this.Controls.Add(this.line1);
            this.Name = "Visualizer";
            this.Text = "Visualizer";
            this.Load += new System.EventHandler(this.Visualizer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label line1;
        private System.Windows.Forms.Label line2;
        private System.Windows.Forms.Label line3;
    }
}