namespace Lana_Renee_Lashes
{
    partial class Main
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelSubTitle = new System.Windows.Forms.Label();
            this.pictureBoxWillow = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWillow)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Tai Le", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(585, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(188, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Lana Renee Lashes";
            // 
            // labelSubTitle
            // 
            this.labelSubTitle.AutoSize = true;
            this.labelSubTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelSubTitle.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubTitle.Location = new System.Drawing.Point(655, 35);
            this.labelSubTitle.Name = "labelSubTitle";
            this.labelSubTitle.Size = new System.Drawing.Size(118, 21);
            this.labelSubTitle.TabIndex = 1;
            this.labelSubTitle.Text = "Cost Calculator:";
            // 
            // pictureBoxWillow
            // 
            this.pictureBoxWillow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBoxWillow.Image = global::Lana_Renee_Lashes.Properties.Resources.Lana;
            this.pictureBoxWillow.Location = new System.Drawing.Point(0, -46);
            this.pictureBoxWillow.Name = "pictureBoxWillow";
            this.pictureBoxWillow.Size = new System.Drawing.Size(773, 157);
            this.pictureBoxWillow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxWillow.TabIndex = 2;
            this.pictureBoxWillow.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Thistle;
            this.ClientSize = new System.Drawing.Size(770, 581);
            this.Controls.Add(this.labelSubTitle);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.pictureBoxWillow);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Name = "Main";
            this.Text = "Lana Renee Lashes";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWillow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelSubTitle;
        private System.Windows.Forms.PictureBox pictureBoxWillow;
    }
}

