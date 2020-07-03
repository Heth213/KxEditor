using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace KxSharpLib.Controls
{
    [ToolboxItem(true)]
    public class KxProgressBar : ProgressBar
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

        #region TextFont
        private Font font = new Font("Ink Free", 10f, FontStyle.Bold);
        [DefaultValue(typeof(Font), "Ink Free")]
        [Category("Kx")]
        [DisplayName("TextFont")]
        [Description("TextFont")]
        public Font TextFont
        {
            get { return font; }
            set { font = value; Invalidate(); }
        }
        #endregion

        public KxProgressBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaintBackground(PaintEventArgs p)
        {
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.OnPaintBackground(p);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            Rectangle rec = new Rectangle(0, 0, Width, Height);
            double scaleFactor = (((double)Value - (double)Minimum) / ((double)Maximum - (double)Minimum));
            int currentWidth = (int)((rec.Width * scaleFactor));

            using (SolidBrush brush = new SolidBrush(GradientColorTop))
            {
                gfx.FillRectangle(brush, rec);
            }

            using (LinearGradientBrush brush = new LinearGradientBrush(rec, GradientColorTop, GradientColorBottom, GradientMode))
            {
                rec.Width = currentWidth;
                if (rec.Width == 0) rec.Width = 1;
                gfx.FillRectangle(brush, rec);
            }

            using (SolidBrush brush = new SolidBrush(Color.GreenYellow))
            {
                SizeF textsize = gfx.MeasureString("Loading . . .", TextFont);
                PointF drawpoint = new PointF((Width - textsize.Width) / 2F, (Height - textsize.Height) / 2F);
                gfx.DrawString("Loading . . .", TextFont, brush, drawpoint);
            }
        }
    }
}