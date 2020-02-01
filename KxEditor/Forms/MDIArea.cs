using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KxEditor.Forms
{
    public partial class MDIArea :
        Form, IMessageFilter
    {
        public MDIArea()
        {
            InitializeComponent();
            Instance = this;
            Application.AddMessageFilter(Instance);
            ControlsToMove = new HashSet<Control> {
                Instance,
                Instance.panel_Top,
                };
        }

        public string Filename = "";
        public string Package = "";
        public bool IsPackage = false;
        public bool Changed = false;
        public RichTextBox textBox;
        public int Encoding = 58;
        public int SelectedItem = -1;
        public ListBox listBox;
        public string password;
        public TreeNode node;


        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == (int)KxSharpLib.Util.Win32.WM.LBUTTONDOWN &&
                 ControlsToMove.Contains(FromHandle(m.HWnd)))
            {
                KxSharpLib.Util.Win32.ReleaseCapture();
                KxSharpLib.Util.Win32.SendMessage(Instance.Handle, (int)KxSharpLib.Util.Win32.WM.NCLBUTTONDOWN, (int)KxSharpLib.Util.Win32.HT.CAPTION, 0);
                return true;
            }
            return false;
        }

        public static MDIArea Instance { get; set; }
        private HashSet<Control> ControlsToMove { get; set; }
        private Rectangle SizeGripRectangle { get; set; }
        public int ResizeHandelSize { get; set; } = 16;



        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case (int)KxSharpLib.Util.Win32.WM.NCHITTEST:
                    base.WndProc(ref message);
                    if (SizeGripRectangle.Contains(PointToClient(new Point(message.LParam.ToInt32() & 0xffff, message.LParam.ToInt32() >> 16))))
                        message.Result = new IntPtr((int)KxSharpLib.Util.Win32.HT.BOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref message);
                    break;
            }
        }


        protected override void OnSizeChanged(EventArgs e)
        {

            base.OnSizeChanged(e);
            Region NewRegion = new Region(new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height));
            NewRegion.Exclude(SizeGripRectangle = new Rectangle(ClientRectangle.Width - ResizeHandelSize, ClientRectangle.Height - ResizeHandelSize, ResizeHandelSize, ResizeHandelSize));
            panel_Main.Region = NewRegion;
            Invalidate();
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.FromArgb(45, 45, 48), SizeGripRectangle);
        }










        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
