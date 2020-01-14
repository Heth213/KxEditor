using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KxEditor
{
    public partial class About : Form, IMessageFilter
    {
        private HashSet<Control> ControlsToMove { get; set; }

        public About()
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
            ControlsToMove = new HashSet<Control> {
                this,
                panel_TopCenter,
                panel_RightTop,
                panel_LeftTop,
                panel_MainCenter,
            };

        }
        public bool PreFilterMessage(ref Message message)
        {
            return KxSharpLib.FormHelper.TryDrag(this, ref message, ControlsToMove);
        }

        private void About_Load(object sender, EventArgs e)
        {
            this.pictureBox_TopLeft.BackgroundImage = new Bitmap(Properties.Resources.financial_changes_64px);
        }
        private void Button_RightTopExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Button_RightTopExit_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopExit, Color.Red);
        }
        private void Button_RightTopExit_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopExit, Color.GreenYellow);
        }
        private void Button_RightTopExit_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopExit, Color.Black);
        }
        private void Button_RightTopExit_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopExit, Color.GreenYellow);
        }
        private void Label_AboutCaption_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(label_AboutCaption, Color.Red);
        }
        private void Label_AboutCaption_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(label_AboutCaption, Color.GreenYellow);
        }
        private void Label_AboutCaption_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(label_AboutCaption, Color.Black);
        }
        private void Label_AboutCaption_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(label_AboutCaption, Color.GreenYellow);
        }
    }
}
