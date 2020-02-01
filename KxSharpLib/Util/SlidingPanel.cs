using System;
using System.Windows.Forms;

namespace KxSharpLib.Util
{

    public class SlidingPanel
    {
        public Panel panel;
        public Timer timer;
        public bool ishidden;
        public int maxwidth;
        public int widthincrement;

        public SlidingPanel(Panel _panel, int _maxwidth, int _widthincrement, int _timerinterval)
        {
            panel = _panel;
            timer = new Timer {
                Interval = _timerinterval
            };
            timer.Tick += new EventHandler(Timer_Tick);

            maxwidth = _maxwidth;
            panel.Width = 0;
            ishidden = true;
            widthincrement = _widthincrement;
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            if (ishidden)
            {
                panel.Width += widthincrement;
                if (panel.Width >= maxwidth)
                {
                    Stop();
                    ishidden = false;
                    panel.Refresh();
                }
            }
            else
            {
                panel.Width -= widthincrement;
                if (panel.Width <= 0)
                {
                    Stop();
                    ishidden = true;
                    panel.Refresh();
                }
            }
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
