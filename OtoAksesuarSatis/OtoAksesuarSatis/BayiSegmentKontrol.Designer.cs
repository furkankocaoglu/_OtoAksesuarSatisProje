namespace OtoAksesuarSatis
{
    partial class BayiSegmentKontrol
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
            this.cbSegmentSecim = new System.Windows.Forms.ComboBox();
            this.btnXmlOlustur = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbSegmentSecim
            // 
            this.cbSegmentSecim.FormattingEnabled = true;
            this.cbSegmentSecim.Location = new System.Drawing.Point(174, 118);
            this.cbSegmentSecim.Name = "cbSegmentSecim";
            this.cbSegmentSecim.Size = new System.Drawing.Size(121, 21);
            this.cbSegmentSecim.TabIndex = 0;
            // 
            // btnXmlOlustur
            // 
            this.btnXmlOlustur.Location = new System.Drawing.Point(206, 180);
            this.btnXmlOlustur.Name = "btnXmlOlustur";
            this.btnXmlOlustur.Size = new System.Drawing.Size(75, 23);
            this.btnXmlOlustur.TabIndex = 1;
            this.btnXmlOlustur.Text = "button1";
            this.btnXmlOlustur.UseVisualStyleBackColor = true;
            // 
            // BayiSegmentKontrol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnXmlOlustur);
            this.Controls.Add(this.cbSegmentSecim);
            this.Name = "BayiSegmentKontrol";
            this.Text = "BayiSegmentKontrol";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbSegmentSecim;
        private System.Windows.Forms.Button btnXmlOlustur;
    }
}