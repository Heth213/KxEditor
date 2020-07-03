﻿using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace KxSharpLib.Controls
{
    [ToolboxItem(true)]
    public class KxLabel : Label
    {
        #region GradientColorTop
        private Color gradientcolortop = Color.White;
        [DefaultValue(typeof(Color), "White")]
        [Category("Kx")]
        [DisplayName("Top Gradient Color")]
        [Description("The top gradient color.")]
        public Color GradientColorTop
        {
            get { return gradientcolortop; }
            set { gradientcolortop = value; Invalidate(); }
        }
        #endregion

        #region GradientColorBottom
        private Color gradientcolorbottom = Color.White;

        [DefaultValue(typeof(Color), "Black")]
        [Category("Kx")]
        [DisplayName("Bottom Gradient Color")]
        [Description("The bottom gradient color.")]
        public Color GradientColorBottom
        {
            get { return gradientcolorbottom; }
            set { gradientcolorbottom = value; Invalidate(); }
        }
        #endregion

        #region LinearGradientMode
        private LinearGradientMode gradientmode = LinearGradientMode.Vertical;
        [DefaultValue(typeof(LinearGradientMode), "Vertical")]
        [Category("Kx")]
        [DisplayName("Linear Gradient Mode")]
        [Description("The Linear Gradient Mode.")]
        public LinearGradientMode GradientMode
        {
            get { return gradientmode; }
            set { gradientmode = value; Invalidate(); }
        }
        #endregion

        public KxLabel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            using (LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, Width, Height + 5), GradientColorTop, GradientColorBottom, GradientMode))
            {
                gfx.DrawString(Text, Font, brush, 0, 0);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
