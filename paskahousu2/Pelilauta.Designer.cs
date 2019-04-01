namespace paskahousu2
{
    partial class Paskahousu
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
            this.jatkaVuoroaBtn = new System.Windows.Forms.Button();
            this.kenenVuoroLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // jatkaVuoroaBtn
            // 
            this.jatkaVuoroaBtn.Location = new System.Drawing.Point(785, 574);
            this.jatkaVuoroaBtn.Name = "jatkaVuoroaBtn";
            this.jatkaVuoroaBtn.Size = new System.Drawing.Size(75, 23);
            this.jatkaVuoroaBtn.TabIndex = 0;
            this.jatkaVuoroaBtn.Text = "Jatka vuoroa";
            this.jatkaVuoroaBtn.UseVisualStyleBackColor = true;
            this.jatkaVuoroaBtn.Click += new System.EventHandler(this.JatkaVuoroaBtn_Click);
            // 
            // kenenVuoroLabel
            // 
            this.kenenVuoroLabel.AutoSize = true;
            this.kenenVuoroLabel.BackColor = System.Drawing.SystemColors.Control;
            this.kenenVuoroLabel.Location = new System.Drawing.Point(782, 547);
            this.kenenVuoroLabel.Name = "kenenVuoroLabel";
            this.kenenVuoroLabel.Size = new System.Drawing.Size(35, 13);
            this.kenenVuoroLabel.TabIndex = 1;
            this.kenenVuoroLabel.Text = "label1";
            // 
            // Paskahousu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 609);
            this.Controls.Add(this.kenenVuoroLabel);
            this.Controls.Add(this.jatkaVuoroaBtn);
            this.Name = "Paskahousu";
            this.Text = "Paskahousu";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button jatkaVuoroaBtn;
        private System.Windows.Forms.Label kenenVuoroLabel;
    }
}

