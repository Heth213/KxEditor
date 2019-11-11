using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace KxSharpLib
{
    public class KxProgressBar : ProgressBar
    {
        public Color TopColor { get; set; }
        public Color BottomColor { get; set; }

        public KxProgressBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphicsObject = e.Graphics;
            Rectangle rec = new Rectangle(0, 0, Width, Height);
            double scaleFactor = (((double)Value - (double)Minimum) / ((double)Maximum - (double)Minimum));
            int currentWidth = (int)((rec.Width * scaleFactor));

            using (SolidBrush brush = new SolidBrush(TopColor))
            {
                graphicsObject.FillRectangle(brush, rec);
            }

            using (LinearGradientBrush brush = new LinearGradientBrush(rec, TopColor, BottomColor, LinearGradientMode.Vertical))
            {
                rec.Width = currentWidth;
                if (rec.Width == 0) rec.Width = 1;
                graphicsObject.FillRectangle(brush, rec);
            }

            using (SolidBrush sb = new SolidBrush(Color.GreenYellow))
            {
                using (Font font = new Font("Ink Free", 10, FontStyle.Bold))
                {
                    SizeF sz = graphicsObject.MeasureString("Loading . . .", font);
                    graphicsObject.DrawString("Loading . . .", font, sb, new PointF((Width - sz.Width) / 2F, (Height - sz.Height) / 2F));
                }
            }
        }
    }
}