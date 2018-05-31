namespace SlajdyZdziec.GUI.Comon
{
    partial class PoleVektor
    {
        /// <summary> 
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod wygenerowany przez Projektanta składników

        /// <summary> 
        /// Wymagana metoda Wsparcia projektanta - nie należy modyfikować 
        /// zawartość tej metody z edytorem kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.WartoscY = new SlajdyZdziec.GUI.Comon.PoleWartosci();
            this.WartoscX = new SlajdyZdziec.GUI.Comon.PoleWartosci();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-3, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "x";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "label3";
            // 
            // WartoscY
            // 
            this.WartoscY.AktywnyKlawisz1 = false;
            this.WartoscY.Location = new System.Drawing.Point(111, 36);
            this.WartoscY.Name = "WartoscY";
            this.WartoscY.Size = new System.Drawing.Size(80, 20);
            this.WartoscY.TabIndex = 3;
            this.WartoscY.Text = "50";
            this.WartoscY.Zmiana = 1F;
            this.WartoscY.TextChanged += new System.EventHandler(this.WartoscY_TextChanged);
            this.WartoscY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WartoscX_KeyDown);
            // 
            // WartoscX
            // 
            this.WartoscX.AktywnyKlawisz1 = false;
            this.WartoscX.Location = new System.Drawing.Point(0, 36);
            this.WartoscX.Name = "WartoscX";
            this.WartoscX.Size = new System.Drawing.Size(80, 20);
            this.WartoscX.TabIndex = 2;
            this.WartoscX.Text = "50";
            this.WartoscX.Zmiana = 1F;
            this.WartoscX.TextChanged += new System.EventHandler(this.WartoscX_TextChanged);
            this.WartoscX.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WartoscX_KeyDown);
            // 
            // PoleVektor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.WartoscY);
            this.Controls.Add(this.WartoscX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PoleVektor";
            this.Size = new System.Drawing.Size(195, 71);
            this.Load += new System.EventHandler(this.PoleVektor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private PoleWartosci WartoscX;
        private PoleWartosci WartoscY;
        private System.Windows.Forms.Label label3;
    }
}
