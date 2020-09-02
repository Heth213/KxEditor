namespace KxEditor
{
    partial class About
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
            this.label_About = new System.Windows.Forms.Label();
            this.panel_BottomCenter = new System.Windows.Forms.Panel();
            this.panel_TopCenter = new System.Windows.Forms.Panel();
            this.panel_Right = new System.Windows.Forms.Panel();
            this.panel_RightTop = new System.Windows.Forms.Panel();
            this.button_RightTopExit = new System.Windows.Forms.Button();
            this.panelRightBottom = new System.Windows.Forms.Panel();
            this.panel_LeftMenu = new System.Windows.Forms.Panel();
            this.panel_LeftCenter = new System.Windows.Forms.Panel();
            this.panel_LeftTop = new System.Windows.Forms.Panel();
            this.label_AboutCaption = new System.Windows.Forms.Label();
            this.pictureBox_TopLeft = new System.Windows.Forms.PictureBox();
            this.panel_LeftBottom = new System.Windows.Forms.Panel();
            this.panel_MainCenter.SuspendLayout();
            this.panel_Right.SuspendLayout();
            this.panel_RightTop.SuspendLayout();
            this.panel_LeftMenu.SuspendLayout();
            this.panel_LeftTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TopLeft)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_MainCenter
            // 
            this.panel_MainCenter.Controls.Add(this.label_About);
            this.panel_MainCenter.Controls.Add(this.panel_BottomCenter);
            this.panel_MainCenter.Controls.Add(this.panel_TopCenter);
            this.panel_MainCenter.Controls.Add(this.panel_Right);
            this.panel_MainCenter.Controls.Add(this.panel_LeftMenu);
            this.panel_MainCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_MainCenter.Location = new System.Drawing.Point(0, 0);
            this.panel_MainCenter.Name = "panel_MainCenter";
            this.panel_MainCenter.Size = new System.Drawing.Size(500, 250);
            this.panel_MainCenter.TabIndex = 0;
            // 
            // label_About
            // 
            this.label_About.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label_About.Font = new System.Drawing.Font("Consolas", 12.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_About.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label_About.Location = new System.Drawing.Point(105, 104);
            this.label_About.Name = "label_About";
            this.label_About.Size = new System.Drawing.Size(314, 126);
            this.label_About.TabIndex = 4;
            this.label_About.Text = "KxEditor, a config editor for KalOnline.\r\nCopyright © Darn 2018 ~ 2019 Modified b" +
    "y Heth 2020";
            this.label_About.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel_BottomCenter
            // 
            this.panel_BottomCenter.BackColor = System.Drawing.Color.Silver;
            this.panel_BottomCenter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_BottomCenter.Location = new System.Drawing.Point(105, 230);
            this.panel_BottomCenter.Name = "panel_BottomCenter";
            this.panel_BottomCenter.Size = new System.Drawing.Size(314, 20);
            this.panel_BottomCenter.TabIndex = 3;
            // 
            // panel_TopCenter
            // 
            this.panel_TopCenter.BackColor = System.Drawing.Color.Silver;
            this.panel_TopCenter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_TopCenter.Location = new System.Drawing.Point(105, 0);
            this.panel_TopCenter.Name = "panel_TopCenter";
            this.panel_TopCenter.Size = new System.Drawing.Size(314, 33);
            this.panel_TopCenter.TabIndex = 2;
            // 
            // panel_Right
            // 
            this.panel_Right.Controls.Add(this.panel_RightTop);
            this.panel_Right.Controls.Add(this.panelRightBottom);
            this.panel_Right.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Right.Location = new System.Drawing.Point(419, 0);
            this.panel_Right.Name = "panel_Right";
            this.panel_Right.Size = new System.Drawing.Size(81, 250);
            this.panel_Right.TabIndex = 1;
            // 
            // panel_RightTop
            // 
            this.panel_RightTop.BackColor = System.Drawing.Color.Silver;
            this.panel_RightTop.Controls.Add(this.button_RightTopExit);
            this.panel_RightTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_RightTop.Location = new System.Drawing.Point(0, 0);
            this.panel_RightTop.Name = "panel_RightTop";
            this.panel_RightTop.Size = new System.Drawing.Size(81, 33);
            this.panel_RightTop.TabIndex = 1;
            // 
            // button_RightTopExit
            // 
            this.button_RightTopExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_RightTopExit.FlatAppearance.BorderSize = 0;
            this.button_RightTopExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(90)))), ((int)(((byte)(41)))));
            this.button_RightTopExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(90)))), ((int)(((byte)(41)))));
            this.button_RightTopExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_RightTopExit.Font = new System.Drawing.Font("Ink Free", 15F, System.Drawing.FontStyle.Bold);
            this.button_RightTopExit.Location = new System.Drawing.Point(52, 0);
            this.button_RightTopExit.Name = "button_RightTopExit";
            this.button_RightTopExit.Size = new System.Drawing.Size(29, 33);
            this.button_RightTopExit.TabIndex = 1;
            this.button_RightTopExit.Text = "X";
            this.button_RightTopExit.UseVisualStyleBackColor = true;
            this.button_RightTopExit.Click += new System.EventHandler(this.Button_RightTopExit_Click);
            this.button_RightTopExit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_RightTopExit_MouseDown);
            this.button_RightTopExit.MouseEnter += new System.EventHandler(this.Button_RightTopExit_MouseEnter);
            this.button_RightTopExit.MouseLeave += new System.EventHandler(this.Button_RightTopExit_MouseLeave);
            this.button_RightTopExit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_RightTopExit_MouseUp);
            // 
            // panelRightBottom
            // 
            this.panelRightBottom.BackColor = System.Drawing.Color.Silver;
            this.panelRightBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelRightBottom.Location = new System.Drawing.Point(0, 230);
            this.panelRightBottom.Name = "panelRightBottom";
            this.panelRightBottom.Size = new System.Drawing.Size(81, 20);
            this.panelRightBottom.TabIndex = 0;
            // 
            // panel_LeftMenu
            // 
            this.panel_LeftMenu.Controls.Add(this.panel_LeftCenter);
            this.panel_LeftMenu.Controls.Add(this.panel_LeftTop);
            this.panel_LeftMenu.Controls.Add(this.panel_LeftBottom);
            this.panel_LeftMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_LeftMenu.Location = new System.Drawing.Point(0, 0);
            this.panel_LeftMenu.Name = "panel_LeftMenu";
            this.panel_LeftMenu.Size = new System.Drawing.Size(105, 250);
            this.panel_LeftMenu.TabIndex = 0;
            // 
            // panel_LeftCenter
            // 
            this.panel_LeftCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_LeftCenter.Location = new System.Drawing.Point(0, 33);
            this.panel_LeftCenter.Name = "panel_LeftCenter";
            this.panel_LeftCenter.Size = new System.Drawing.Size(105, 197);
            this.panel_LeftCenter.TabIndex = 2;
            // 
            // panel_LeftTop
            // 
            this.panel_LeftTop.BackColor = System.Drawing.Color.Silver;
            this.panel_LeftTop.Controls.Add(this.label_AboutCaption);
            this.panel_LeftTop.Controls.Add(this.pictureBox_TopLeft);
            this.panel_LeftTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_LeftTop.Location = new System.Drawing.Point(0, 0);
            this.panel_LeftTop.Name = "panel_LeftTop";
            this.panel_LeftTop.Size = new System.Drawing.Size(105, 33);
            this.panel_LeftTop.TabIndex = 1;
            // 
            // label_AboutCaption
            // 
            this.label_AboutCaption.AutoSize = true;
            this.label_AboutCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label_AboutCaption.Location = new System.Drawing.Point(40, 7);
            this.label_AboutCaption.Name = "label_AboutCaption";
            this.label_AboutCaption.Size = new System.Drawing.Size(57, 20);
            this.label_AboutCaption.TabIndex = 0;
            this.label_AboutCaption.Text = "About";
            this.label_AboutCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label_AboutCaption_MouseDown);
            this.label_AboutCaption.MouseEnter += new System.EventHandler(this.Label_AboutCaption_MouseEnter);
            this.label_AboutCaption.MouseLeave += new System.EventHandler(this.Label_AboutCaption_MouseLeave);
            this.label_AboutCaption.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Label_AboutCaption_MouseUp);
            // 
            // pictureBox_TopLeft
            // 
            this.pictureBox_TopLeft.BackColor = System.Drawing.Color.Silver;
            this.pictureBox_TopLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox_TopLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox_TopLeft.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_TopLeft.Name = "pictureBox_TopLeft";
            this.pictureBox_TopLeft.Size = new System.Drawing.Size(40, 33);
            this.pictureBox_TopLeft.TabIndex = 0;
            this.pictureBox_TopLeft.TabStop = false;
            // 
            // panel_LeftBottom
            // 
            this.panel_LeftBottom.BackColor = System.Drawing.Color.Silver;
            this.panel_LeftBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_LeftBottom.Location = new System.Drawing.Point(0, 230);
            this.panel_LeftBottom.Name = "panel_LeftBottom";
            this.panel_LeftBottom.Size = new System.Drawing.Size(105, 20);
            this.panel_LeftBottom.TabIndex = 0;
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(500, 250);
            this.Controls.Add(this.panel_MainCenter);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.About_Load);
            this.panel_MainCenter.ResumeLayout(false);
            this.panel_Right.ResumeLayout(false);
            this.panel_RightTop.ResumeLayout(false);
            this.panel_LeftMenu.ResumeLayout(false);
            this.panel_LeftTop.ResumeLayout(false);
            this.panel_LeftTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TopLeft)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_MainCenter;
        private System.Windows.Forms.Panel panel_TopCenter;
        private System.Windows.Forms.Panel panel_Right;
        private System.Windows.Forms.Panel panel_LeftMenu;
        private System.Windows.Forms.Panel panel_BottomCenter;
        private System.Windows.Forms.Panel panel_RightTop;
        private System.Windows.Forms.Panel panelRightBottom;
        private System.Windows.Forms.Panel panel_LeftBottom;
        private System.Windows.Forms.Panel panel_LeftTop;
        private System.Windows.Forms.Panel panel_LeftCenter;
        private System.Windows.Forms.PictureBox pictureBox_TopLeft;
        public System.Windows.Forms.Button button_RightTopExit;
        private System.Windows.Forms.Label label_About;
        private System.Windows.Forms.Label label_AboutCaption;
    }
}