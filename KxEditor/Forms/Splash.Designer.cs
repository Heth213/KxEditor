namespace KxEditor.Forms
{
    partial class Splash
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
            this.panel_Main = new System.Windows.Forms.Panel();
            this.panel_Center = new System.Windows.Forms.Panel();
            this.label_Desc = new System.Windows.Forms.Label();
            this.panel_Bottom = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label_LoadingInfo = new System.Windows.Forms.Label();
            this.panel_Top = new System.Windows.Forms.Panel();
            this.label_Version = new System.Windows.Forms.Label();
            this.panel_Main.SuspendLayout();
            this.panel_Center.SuspendLayout();
            this.panel_Bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Main
            // 
            this.panel_Main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.panel_Main.Controls.Add(this.panel_Center);
            this.panel_Main.Controls.Add(this.panel_Bottom);
            this.panel_Main.Controls.Add(this.panel_Top);
            this.panel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Main.Location = new System.Drawing.Point(0, 0);
            this.panel_Main.Name = "panel_Main";
            this.panel_Main.Size = new System.Drawing.Size(500, 128);
            this.panel_Main.TabIndex = 0;
            // 
            // panel_Center
            // 
            this.panel_Center.Controls.Add(this.label_Version);
            this.panel_Center.Controls.Add(this.label_Desc);
            this.panel_Center.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Center.Location = new System.Drawing.Point(0, 33);
            this.panel_Center.Name = "panel_Center";
            this.panel_Center.Size = new System.Drawing.Size(500, 75);
            this.panel_Center.TabIndex = 2;
            // 
            // label_Desc
            // 
            this.label_Desc.AutoSize = true;
            this.label_Desc.Font = new System.Drawing.Font("Ink Free", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label_Desc.Location = new System.Drawing.Point(44, 12);
            this.label_Desc.Name = "label_Desc";
            this.label_Desc.Size = new System.Drawing.Size(392, 34);
            this.label_Desc.TabIndex = 0;
            this.label_Desc.Text = "KxEditor a Kalonline .pk editor.";
            // 
            // panel_Bottom
            // 
            this.panel_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.panel_Bottom.Controls.Add(this.progressBar);
            this.panel_Bottom.Controls.Add(this.label_LoadingInfo);
            this.panel_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Bottom.Location = new System.Drawing.Point(0, 108);
            this.panel_Bottom.Name = "panel_Bottom";
            this.panel_Bottom.Size = new System.Drawing.Size(500, 20);
            this.panel_Bottom.TabIndex = 1;
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.progressBar.Location = new System.Drawing.Point(0, 0);
            this.progressBar.MarqueeAnimationSpeed = 5;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(400, 20);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 2;
            // 
            // label_LoadingInfo
            // 
            this.label_LoadingInfo.AutoSize = true;
            this.label_LoadingInfo.Font = new System.Drawing.Font("Ink Free", 10F, System.Drawing.FontStyle.Bold);
            this.label_LoadingInfo.Location = new System.Drawing.Point(406, 2);
            this.label_LoadingInfo.Name = "label_LoadingInfo";
            this.label_LoadingInfo.Size = new System.Drawing.Size(82, 18);
            this.label_LoadingInfo.TabIndex = 0;
            this.label_LoadingInfo.Text = "Loading. . .";
            this.label_LoadingInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel_Top
            // 
            this.panel_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.panel_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Top.Location = new System.Drawing.Point(0, 0);
            this.panel_Top.Name = "panel_Top";
            this.panel_Top.Size = new System.Drawing.Size(500, 33);
            this.panel_Top.TabIndex = 0;
            // 
            // label_Version
            // 
            this.label_Version.AutoSize = true;
            this.label_Version.Font = new System.Drawing.Font("Ink Free", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label_Version.Location = new System.Drawing.Point(366, 46);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(60, 18);
            this.label_Version.TabIndex = 1;
            this.label_Version.Text = "Version:";
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 128);
            this.Controls.Add(this.panel_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Splash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Splash_Load);
            this.panel_Main.ResumeLayout(false);
            this.panel_Center.ResumeLayout(false);
            this.panel_Center.PerformLayout();
            this.panel_Bottom.ResumeLayout(false);
            this.panel_Bottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_Main;
        private System.Windows.Forms.Panel panel_Bottom;
        private System.Windows.Forms.Panel panel_Top;
        private System.Windows.Forms.Label label_LoadingInfo;
        private System.Windows.Forms.Panel panel_Center;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label_Desc;
        private System.Windows.Forms.Label label_Version;
    }
}