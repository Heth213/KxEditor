using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace KxSharpLib
{
    public class KxPanel : Panel
    {
        public Color TopColor { get; set; }
        public Color BottomColor { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, TopColor, BottomColor, LinearGradientMode.Vertical);
            Graphics g = e.Graphics;
            g.FillRectangle(brush, ClientRectangle);
            base.OnPaint(e);
        }
    }
}
