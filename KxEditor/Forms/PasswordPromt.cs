using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KxEditor
{
    public partial class PasswordPromt : Form, IMessageFilter {

        public static PasswordPromt Instance { get; set; }
        private HashSet<Control> ControlsToMove { get; set; }
        private Rectangle SizeGripRectangle { get; set; }
        public int ResizeHandelSize { get; set; } = 16;
        public string Password = "";

        public PasswordPromt() {
            InitializeComponent();
            Instance = this;
            Instance.DoubleBuffered = true;
            Instance.SetStyle(ControlStyles.ResizeRedraw, true);
            Application.AddMessageFilter(Instance);
            ControlsToMove = new HashSet<Control> { Instance, Instance.panel_Top, };
            this.MinimumSize = new Size(500, 150);
            this.MaximumSize = new Size(500, 150);
            button4.Text = Properties.Settings.Default.prevPW;
        }

        public bool PreFilterMessage(ref Message message)
        {
            return KxSharpLib.FormHelper.TryDrag(this, ref message, ControlsToMove);
        }

        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case (int)KxSharpLib.Win32.WM.NCHITTEST:
                    base.WndProc(ref message);
                    if (SizeGripRectangle.Contains(PointToClient(new Point(message.LParam.ToInt32() & 0xffff, message.LParam.ToInt32() >> 16))))
                        message.Result = new IntPtr((int)KxSharpLib.Win32.HT.BOTTOMRIGHT);
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
            ControlPaint.DrawSizeGrip(e.Graphics, BackColor, SizeGripRectangle);
        }


        private void Button1_Click(object sender, EventArgs e) {
            Properties.Settings.Default.prevPW = textBox1.Text;
            Properties.Settings.Default.Save();
            Password = textBox1.Text;
            this.DialogResult = DialogResult.OK;
            Close();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "5HwyFCjVIbahPH47hMFfkip7SvJj6s";
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "EV)O8@BL$3O2E";
        }
        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Panel_Main_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Close_MouseDown(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(Close, Color.Red);
        }
        private void Close_MouseEnter(object sender, EventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(Close, Color.GreenYellow);
        }
        private void Close_MouseLeave(object sender, EventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(Close, Color.Black);
        }
        private void Close_MouseUp(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(Close, Color.GreenYellow);
        }
        private void Close_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void panel_Main_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.prevPW;
        }
    }
}