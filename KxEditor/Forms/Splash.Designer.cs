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
            this.panel_Top = new System.Windows.Forms.Panel();
            this.panel_Border = new System.Windows.Forms.Panel();
            this.label_Version = new KxSharpLib.KxLabel();
            this.label_Title = new KxSharpLib.KxLabel();
            this.kxPanel = new KxSharpLib.KxPanel();
            this.kxProgressBar = new KxSharpLib.KxProgressBar();
            this.panel_Main.SuspendLayout();
            this.panel_Center.SuspendLayout();
            this.panel_Border.SuspendLayout();
            this.kxPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Main
            // 
            this.panel_Main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.panel_Main.BackgroundImage = global::KxEditor.Properties.Resources.KxLogo_Background;
            this.panel_Main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel_Main.Controls.Add(this.panel_Center);
            this.panel_Main.Controls.Add(this.kxPanel);
            this.panel_Main.Controls.Add(this.panel_Top);
            this.panel_Main.Location = new System.Drawing.Point(3, 3);
            this.panel_Main.Name = "panel_Main";
            this.panel_Main.Size = new System.Drawing.Size(500, 128);
            this.panel_Main.TabIndex = 0;
            // 
            // panel_Center
            // 
            this.panel_Center.BackColor = System.Drawing.Color.Transparent;
            this.panel_Center.Controls.Add(this.label_Version);
            this.panel_Center.Controls.Add(this.label_Title);
            this.panel_Center.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Center.Location = new System.Drawing.Point(0, 33);
            this.panel_Center.Name = "panel_Center";
            this.panel_Center.Size = new System.Drawing.Size(500, 75);
            this.panel_Center.TabIndex = 2;
            // 
            // panel_Top
            // 
            this.panel_Top.BackColor = System.Drawing.Color.Transparent;
            this.panel_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Top.Location = new System.Drawing.Point(0, 0);
            this.panel_Top.Name = "panel_Top";
            this.panel_Top.Size = new System.Drawing.Size(500, 33);
            this.panel_Top.TabIndex = 0;
            // 
            // panel_Border
            // 
            this.panel_Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.panel_Border.Controls.Add(this.panel_Main);
            this.panel_Border.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Border.Location = new System.Drawing.Point(0, 0);
            this.panel_Border.Name = "panel_Border";
            this.panel_Border.Size = new System.Drawing.Size(508, 136);
            this.panel_Border.TabIndex = 1;
            // 
            // label_Version
            // 
            this.label_Version.AutoSize = true;
            this.label_Version.Font = new System.Drawing.Font("Ink Free", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Version.GradientColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.label_Version.Location = new System.Drawing.Point(371, 46);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(55, 15);
            this.label_Version.TabIndex = 3;
            this.label_Version.Text = "Version:";
            // 
            // label_Title
            // 
            this.label_Title.AutoSize = true;
            this.label_Title.Font = new System.Drawing.Font("Ink Free", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label_Title.GradientColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.label_Title.Location = new System.Drawing.Point(49, 12);
            this.label_Title.Name = "label_Title";
            this.label_Title.Size = new System.Drawing.Size(392, 34);
            this.label_Title.TabIndex = 2;
            this.label_Title.Text = "KxEditor a Kalonline .pk editor.";
            // 
            // kxPanel
            // 
            this.kxPanel.Controls.Add(this.kxProgressBar);
            this.kxPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kxPanel.GradientColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.kxPanel.GradientColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.kxPanel.Location = new System.Drawing.Point(0, 108);
            this.kxPanel.Name = "kxPanel";
            this.kxPanel.Size = new System.Drawing.Size(500, 20);
            this.kxPanel.TabIndex = 2;
            // 
            // kxProgressBar
            // 
            this.kxProgressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.kxProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kxProgressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.kxProgressBar.GradientColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.kxProgressBar.GradientColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.kxProgressBar.Location = new System.Drawing.Point(0, 0);
            this.kxProgressBar.MarqueeAnimationSpeed = 5;
            this.kxProgressBar.Name = "kxProgressBar";
            this.kxProgressBar.Size = new System.Drawing.Size(500, 20);
            this.kxProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.kxProgressBar.TabIndex = 1;
            this.kxProgressBar.TextFont = new System.Drawing.Font("Ink Free", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(20)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(508, 136);
            this.Controls.Add(this.panel_Border);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Splash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Splash_FormClosing);
            this.Load += new System.EventHandler(this.Splash_Load);
            this.panel_Main.ResumeLayout(false);
            this.panel_Center.ResumeLayout(false);
            this.panel_Center.PerformLayout();
            this.panel_Border.ResumeLayout(false);
            this.kxPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_Main;
        private System.Windows.Forms.Panel panel_Top;
        private System.Windows.Forms.Panel panel_Center;
        private KxSharpLib.KxProgressBar kxProgressBar;
        private KxSharpLib.KxPanel kxPanel;
        private System.Windows.Forms.Panel panel_Border;
        private KxSharpLib.KxLabel label_Title;
        private KxSharpLib.KxLabel label_Version;
    }
}