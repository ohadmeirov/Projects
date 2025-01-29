namespace CheckersGameUI
{
    partial class FormStartingConditionInfo
    {
        private System.ComponentModel.IContainer components = null;

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
            System.Windows.Forms.Button buttonDone;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStartingConditionInfo));
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton6x6 = new System.Windows.Forms.RadioButton();
            this.radioButton8x8 = new System.Windows.Forms.RadioButton();
            this.radioButton10x10 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPlayer1 = new System.Windows.Forms.TextBox();
            this.checkBoxIsRealPlayer = new System.Windows.Forms.CheckBox();
            this.textBoxPlayer2 = new System.Windows.Forms.TextBox();
            buttonDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonDone
            // 
            buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            buttonDone.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            buttonDone.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            buttonDone.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            buttonDone.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            buttonDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            buttonDone.Location = new System.Drawing.Point(153, 169);
            buttonDone.Name = "buttonDone";
            buttonDone.Size = new System.Drawing.Size(92, 23);
            buttonDone.TabIndex = 70;
            buttonDone.Text = "Done!";
            buttonDone.UseVisualStyleBackColor = true;
            buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Board Size:";
            // 
            // radioButton6x6
            // 
            this.radioButton6x6.AutoSize = true;
            this.radioButton6x6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioButton6x6.Location = new System.Drawing.Point(33, 32);
            this.radioButton6x6.Name = "radioButton6x6";
            this.radioButton6x6.Size = new System.Drawing.Size(47, 17);
            this.radioButton6x6.TabIndex = 10;
            this.radioButton6x6.TabStop = true;
            this.radioButton6x6.Text = "6 x 6";
            this.radioButton6x6.UseVisualStyleBackColor = true;
            // 
            // radioButton8x8
            // 
            this.radioButton8x8.AutoSize = true;
            this.radioButton8x8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioButton8x8.Location = new System.Drawing.Point(93, 32);
            this.radioButton8x8.Name = "radioButton8x8";
            this.radioButton8x8.Size = new System.Drawing.Size(47, 17);
            this.radioButton8x8.TabIndex = 20;
            this.radioButton8x8.TabStop = true;
            this.radioButton8x8.Text = "8 x 8";
            this.radioButton8x8.UseVisualStyleBackColor = true;
            // 
            // radioButton10x10
            // 
            this.radioButton10x10.AutoSize = true;
            this.radioButton10x10.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.radioButton10x10.Location = new System.Drawing.Point(153, 33);
            this.radioButton10x10.Name = "radioButton10x10";
            this.radioButton10x10.Size = new System.Drawing.Size(59, 17);
            this.radioButton10x10.TabIndex = 30;
            this.radioButton10x10.TabStop = true;
            this.radioButton10x10.Text = "10 x 10";
            this.radioButton10x10.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label2.Location = new System.Drawing.Point(12, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Players:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label3.Location = new System.Drawing.Point(39, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Player 1:";
            // 
            // textBoxPlayer1
            // 
            this.textBoxPlayer1.Location = new System.Drawing.Point(115, 91);
            this.textBoxPlayer1.Name = "textBoxPlayer1";
            this.textBoxPlayer1.Size = new System.Drawing.Size(120, 20);
            this.textBoxPlayer1.TabIndex = 40;
            this.textBoxPlayer1.TextChanged += new System.EventHandler(this.textBoxPlayer1_TextChanged);
            // 
            // checkBoxIsRealPlayer
            // 
            this.checkBoxIsRealPlayer.AutoSize = true;
            this.checkBoxIsRealPlayer.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkBoxIsRealPlayer.Location = new System.Drawing.Point(24, 129);
            this.checkBoxIsRealPlayer.Name = "checkBoxIsRealPlayer";
            this.checkBoxIsRealPlayer.Size = new System.Drawing.Size(65, 17);
            this.checkBoxIsRealPlayer.TabIndex = 50;
            this.checkBoxIsRealPlayer.Text = "Player 2:";
            this.checkBoxIsRealPlayer.UseVisualStyleBackColor = true;
            this.checkBoxIsRealPlayer.CheckedChanged += new System.EventHandler(this.checkBoxIsRealPlayer_CheckedChanged);
            // 
            // textBoxPlayer2
            // 
            this.textBoxPlayer2.Enabled = false;
            this.textBoxPlayer2.Location = new System.Drawing.Point(115, 128);
            this.textBoxPlayer2.Name = "textBoxPlayer2";
            this.textBoxPlayer2.Size = new System.Drawing.Size(120, 20);
            this.textBoxPlayer2.TabIndex = 60;
            this.textBoxPlayer2.TabStop = false;
            this.textBoxPlayer2.Text = "Computer";
            this.textBoxPlayer2.TextChanged += new System.EventHandler(this.textBoxPlayer2_TextChanged);
            // 
            // FormStartingConditionInfo
            // 
            this.AcceptButton = buttonDone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(273, 215);
            this.ControlBox = false;
            this.Controls.Add(this.textBoxPlayer2);
            this.Controls.Add(this.checkBoxIsRealPlayer);
            this.Controls.Add(this.textBoxPlayer1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radioButton10x10);
            this.Controls.Add(this.radioButton8x8);
            this.Controls.Add(this.radioButton6x6);
            this.Controls.Add(this.label1);
            this.Controls.Add(buttonDone);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStartingConditionInfo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Settings";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton6x6;
        private System.Windows.Forms.RadioButton radioButton8x8;
        private System.Windows.Forms.RadioButton radioButton10x10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPlayer1;
        private System.Windows.Forms.CheckBox checkBoxIsRealPlayer;
        private System.Windows.Forms.TextBox textBoxPlayer2;
    }
}