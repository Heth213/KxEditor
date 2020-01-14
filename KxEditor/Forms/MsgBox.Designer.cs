namespace KxEditor
{
    partial class MsgBox
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
            this.panel_MainCenter = new System.Windows.Forms.Panel();
            this.panel_ContentCenter = new System.Windows.Forms.Panel();
            this.label_CenterMessage = new System.Windows.Forms.Label();
            this.button_CANCEL = new System.Windows.Forms.Button();
            this.button_YES = new System.Windows.Forms.Button();
            this.button_NO = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.panel_Bottom = new System.Windows.Forms.Panel();
            this.panel_Top = new System.Windows.Forms.Panel();
            this.button_TopExit = new System.Windows.Forms.Button();
            this.label_TopCaption = new System.Windows.Forms.Label();
            this.pictureBox_TopIcon = new System.Windows.Forms.PictureBox();
            this.panel_MainCenter.SuspendLayout();
            this.panel_ContentCenter.SuspendLayout();
            this.panel_Top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TopIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_MainCenter
            // 
            this.panel_MainCenter.Controls.Add(this.panel_ContentCenter);
            this.panel_MainCenter.Controls.Add(this.panel_Bottom);
            this.panel_MainCenter.Controls.Add(this.panel_Top);
            this.panel_MainCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_MainCenter.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel_MainCenter.Location = new System.Drawing.Point(0, 0);
            this.panel_MainCenter.Name = "panel_MainCenter";
            this.panel_MainCenter.Size = new System.Drawing.Size(485, 173);
            this.panel_MainCenter.TabIndex = 0;
            // 
            // panel_ContentCenter
            // 
            this.panel_ContentCenter.Controls.Add(this.label_CenterMessage);
            this.panel_ContentCenter.Controls.Add(this.button_CANCEL);
            this.panel_ContentCenter.Controls.Add(this.button_YES);
            this.panel_ContentCenter.Controls.Add(this.button_NO);
            this.panel_ContentCenter.Controls.Add(this.button_OK);
            this.panel_ContentCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_ContentCenter.Location = new System.Drawing.Point(0, 33);
            this.panel_ContentCenter.Name = "panel_ContentCenter";
            this.panel_ContentCenter.Size = new System.Drawing.Size(485, 120);
            this.panel_ContentCenter.TabIndex = 2;
            // 
            // label_CenterMessage
            // 
            this.label_CenterMessage.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_CenterMessage.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label_CenterMessage.Location = new System.Drawing.Point(12, 16);
            this.label_CenterMessage.Name = "label_CenterMessage";
            this.label_CenterMessage.Size = new System.Drawing.Size(453, 72);
            this.label_CenterMessage.TabIndex = 4;
            this.label_CenterMessage.Text = "Message Text";
            this.label_CenterMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_CANCEL
            // 
            this.button_CANCEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CANCEL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_CANCEL.Font = new System.Drawing.Font("Consolas", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_CANCEL.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.button_CANCEL.Location = new System.Drawing.Point(310, 90);
            this.button_CANCEL.Name = "button_CANCEL";
            this.button_CANCEL.Size = new System.Drawing.Size(75, 23);
            this.button_CANCEL.TabIndex = 3;
            this.button_CANCEL.Text = "CANCEL";
            this.button_CANCEL.UseVisualStyleBackColor = true;
            this.button_CANCEL.Visible = false;
            this.button_CANCEL.Click += new System.EventHandler(this.Button_CANCEL_Click);
            // 
            // button_YES
            // 
            this.button_YES.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_YES.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_YES.Font = new System.Drawing.Font("Consolas", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.button_YES.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.button_YES.Location = new System.Drawing.Point(390, 90);
            this.button_YES.Name = "button_YES";
            this.button_YES.Size = new System.Drawing.Size(75, 23);
            this.button_YES.TabIndex = 2;
            this.button_YES.Text = "YES";
            this.button_YES.UseVisualStyleBackColor = true;
            this.button_YES.Visible = false;
            this.button_YES.Click += new System.EventHandler(this.Button_YES_Click);
            // 
            // button_NO
            // 
            this.button_NO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_NO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_NO.Font = new System.Drawing.Font("Consolas", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_NO.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.button_NO.Location = new System.Drawing.Point(390, 90);
            this.button_NO.Name = "button_NO";
            this.button_NO.Size = new System.Drawing.Size(75, 23);
            this.button_NO.TabIndex = 1;
            this.button_NO.Text = "NO";
            this.button_NO.UseVisualStyleBackColor = true;
            this.button_NO.Visible = false;
            this.button_NO.Click += new System.EventHandler(this.Button_NO_Click);
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_OK.Font = new System.Drawing.Font("Consolas", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_OK.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.button_OK.Location = new System.Drawing.Point(390, 90);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 0;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Visible = false;
            this.button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            // 
            // panel_Bottom
            // 
            this.panel_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.panel_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Bottom.Location = new System.Drawing.Point(0, 153);
            this.panel_Bottom.Name = "panel_Bottom";
            this.panel_Bottom.Size = new System.Drawing.Size(485, 20);
            this.panel_Bottom.TabIndex = 1;
            // 
            // panel_Top
            // 
            this.panel_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(90)))), ((int)(((byte)(41)))));
            this.panel_Top.Controls.Add(this.button_TopExit);
            this.panel_Top.Controls.Add(this.label_TopCaption);
            this.panel_Top.Controls.Add(this.pictureBox_TopIcon);
            this.panel_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Top.Location = new System.Drawing.Point(0, 0);
            this.panel_Top.Name = "panel_Top";
            this.panel_Top.Size = new System.Drawing.Size(485, 33);
            this.panel_Top.TabIndex = 0;
            // 
            // button_TopExit
            // 
            this.button_TopExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_TopExit.FlatAppearance.BorderSize = 0;
            this.button_TopExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(90)))), ((int)(((byte)(41)))));
            this.button_TopExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(90)))), ((int)(((byte)(41)))));
            this.button_TopExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_TopExit.Font = new System.Drawing.Font("Ink Free", 15F, System.Drawing.FontStyle.Bold);
            this.button_TopExit.Location = new System.Drawing.Point(456, 0);
            this.button_TopExit.Name = "button_TopExit";
            this.button_TopExit.Size = new System.Drawing.Size(29, 33);
            this.button_TopExit.TabIndex = 2;
            this.button_TopExit.Text = "X";
            this.button_TopExit.UseVisualStyleBackColor = false;
            this.button_TopExit.Click += new System.EventHandler(this.Button_TopExit_Click);
            this.button_TopExit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_TopExit_MouseDown);
            this.button_TopExit.MouseEnter += new System.EventHandler(this.Button_TopExit_MouseEnter);
            this.button_TopExit.MouseLeave += new System.EventHandler(this.Button_TopExit_MouseLeave);
            this.button_TopExit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_TopExit_MouseUp);
            // 
            // label_TopCaption
            // 
            this.label_TopCaption.AutoSize = true;
            this.label_TopCaption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_TopCaption.Font = new System.Drawing.Font("Ink Free", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TopCaption.Location = new System.Drawing.Point(42, 5);
            this.label_TopCaption.Name = "label_TopCaption";
            this.label_TopCaption.Size = new System.Drawing.Size(77, 23);
            this.label_TopCaption.TabIndex = 1;
            this.label_TopCaption.Text = "Caption";
            this.label_TopCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox_TopIcon
            // 
            this.pictureBox_TopIcon.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_TopIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox_TopIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox_TopIcon.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_TopIcon.Name = "pictureBox_TopIcon";
            this.pictureBox_TopIcon.Size = new System.Drawing.Size(36, 33);
            this.pictureBox_TopIcon.TabIndex = 0;
            this.pictureBox_TopIcon.TabStop = false;
            // 
            // KxMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(485, 173);
            this.Controls.Add(this.panel_MainCenter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "KxMsgBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KxMsgBox";
            this.panel_MainCenter.ResumeLayout(false);
            this.panel_ContentCenter.ResumeLayout(false);
            this.panel_Top.ResumeLayout(false);
            this.panel_Top.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TopIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_MainCenter;
        private System.Windows.Forms.Panel panel_Bottom;
        private System.Windows.Forms.Panel panel_Top;
        private System.Windows.Forms.Panel panel_ContentCenter;
        private System.Windows.Forms.Button button_CANCEL;
        private System.Windows.Forms.Button button_YES;
        private System.Windows.Forms.Button button_NO;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.PictureBox pictureBox_TopIcon;
        private System.Windows.Forms.Label label_CenterMessage;
        private System.Windows.Forms.Button button_TopExit;
        private System.Windows.Forms.Label label_TopCaption;
    }
}