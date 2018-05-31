namespace SlajdyZdziec.GUI.ImageInImage
{
    partial class ColageImageForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.projection = new SlajdyZdziec.GUI.Comon.PoleVektor();
            this.SizePartImage = new SlajdyZdziec.GUI.Comon.PoleVektor();
            this.button2 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(212, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Wczytaj zdjęcie odwzorowywane";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 43);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(212, 182);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // projection
            // 
            this.projection.AutoSize = true;
            this.projection.Lab = 0;
            this.projection.Location = new System.Drawing.Point(13, 232);
            this.projection.Name = "projection";
            this.projection.Size = new System.Drawing.Size(212, 69);
            this.projection.TabIndex = 2;
            this.projection.Text = "Dokładność odwzrowoania";
            this.projection.Wartość = new System.Drawing.Point(20, 20);
            this.projection.Load += new System.EventHandler(this.poleVektor1_Load);
            // 
            // SizePartImage
            // 
            this.SizePartImage.AutoSize = true;
            this.SizePartImage.Lab = 0;
            this.SizePartImage.Location = new System.Drawing.Point(12, 307);
            this.SizePartImage.Name = "SizePartImage";
            this.SizePartImage.Size = new System.Drawing.Size(213, 69);
            this.SizePartImage.TabIndex = 3;
            this.SizePartImage.Text = "Wielkość pod obrazu";
            this.SizePartImage.Wartość = new System.Drawing.Point(20, 20);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 394);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(226, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Działaj";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ColageImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 469);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.SizePartImage);
            this.Controls.Add(this.projection);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Name = "ColageImageForm";
            this.Text = "ColageImageForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Comon.PoleVektor projection;
        private Comon.PoleVektor SizePartImage;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}