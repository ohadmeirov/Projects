namespace FourInARow
{
    partial class Levels
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Levels));
            this.Exit = new System.Windows.Forms.Button();
            this.Easy = new System.Windows.Forms.Button();
            this.Hard = new System.Windows.Forms.Button();
            this.Noramal = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // Exit
            // 
            this.Exit.BackColor = System.Drawing.SystemColors.Highlight;
            this.Exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Exit.Location = new System.Drawing.Point(1734, 863);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(149, 70);
            this.Exit.TabIndex = 9;
            this.Exit.Text = "Return";
            this.Exit.UseVisualStyleBackColor = false;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Easy
            // 
            this.Easy.BackColor = System.Drawing.Color.RoyalBlue;
            this.Easy.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Easy.Location = new System.Drawing.Point(1133, 249);
            this.Easy.Name = "Easy";
            this.Easy.Size = new System.Drawing.Size(223, 106);
            this.Easy.TabIndex = 8;
            this.Easy.Text = "Easy";
            this.Easy.UseVisualStyleBackColor = false;
            this.Easy.Click += new System.EventHandler(this.Easy_Click);
            // 
            // Hard
            // 
            this.Hard.BackColor = System.Drawing.Color.Cyan;
            this.Hard.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Hard.Location = new System.Drawing.Point(1133, 500);
            this.Hard.Name = "Hard";
            this.Hard.Size = new System.Drawing.Size(224, 106);
            this.Hard.TabIndex = 7;
            this.Hard.Text = "Hard";
            this.Hard.UseVisualStyleBackColor = false;
            this.Hard.Click += new System.EventHandler(this.Hard_Click);
            // 
            // Noramal
            // 
            this.Noramal.BackColor = System.Drawing.Color.Yellow;
            this.Noramal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Noramal.Location = new System.Drawing.Point(525, 380);
            this.Noramal.Name = "Noramal";
            this.Noramal.Size = new System.Drawing.Size(223, 106);
            this.Noramal.TabIndex = 6;
            this.Noramal.Text = "Normal";
            this.Noramal.UseVisualStyleBackColor = false;
            this.Noramal.Click += new System.EventHandler(this.Noramal_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(525, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(223, 101);
            this.button1.TabIndex = 5;
            this.button1.Text = "Choose level:";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Levels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1326, 650);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.Easy);
            this.Controls.Add(this.Hard);
            this.Controls.Add(this.Noramal);
            this.Controls.Add(this.button1);
            this.Name = "Levels";
            this.Text = "Levels";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Levels_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.Button Easy;
        private System.Windows.Forms.Button Hard;
        private System.Windows.Forms.Button Noramal;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ImageList imageList1;
    }
}