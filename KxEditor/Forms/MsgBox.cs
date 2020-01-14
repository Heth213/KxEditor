using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KxEditor
{
    public enum MsgBoxButton
    {
        OK,
        OKCANCEL,
        YESNO,
        YESNOCANCEL,
        RETRYCANCEL,
        HELP,
    }
    public enum MsgBoxIcon
    {
        ERROR,
        QUESTION,
        WARNING,
        INFORMATION,
    }
    public enum MsgBoxResult
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Abort = 3,
        Retry = 4,
        Ignore = 5,
        Yes = 6,
        No = 7
    }

    public partial class MsgBox : Form, IMessageFilter
    {
        private HashSet<Control> ControlsToMove { get; set; }
        static MsgBox msgBox;
        static MsgBoxResult Result;

        public MsgBox()
        {
            InitializeComponent();
            
            Application.AddMessageFilter(this);
            ControlsToMove = new HashSet<Control>
            {
                this,
                panel_Top,
                panel_ContentCenter,
                panel_Bottom,
            };
        }

        public bool PreFilterMessage(ref Message message)
        {
            return KxSharpLib.FormHelper.TryDrag(this, ref message, ControlsToMove);
        }
        public void SetButton(MsgBoxButton btn)
        {
            switch (btn)
            {
                case MsgBoxButton.OK:
                    {
                        msgBox.button_OK.Location = new Point(390, 90);
                        msgBox.button_OK.Visible = true;
                        break;
                    }
                case MsgBoxButton.YESNO:
                    {
                        msgBox.button_YES.Location = new Point(390, 90);
                        msgBox.button_YES.Visible = true;
                        msgBox.button_NO.Location = new Point(310, 90);
                        msgBox.button_NO.Visible = true;
                        break;
                    }
                case MsgBoxButton.OKCANCEL:
                    {
                        msgBox.button_OK.Location = new Point(390, 90);
                        msgBox.button_OK.Visible = true;
                        msgBox.button_CANCEL.Location = new Point(310, 90);
                        msgBox.button_CANCEL.Visible = true;
                        break;
                    }
            }
        }
        public void SetIcon(MsgBoxIcon Icon)
        {
            switch (Icon)
            {
                case MsgBoxIcon.ERROR:
                    {
                        msgBox.pictureBox_TopIcon.BackgroundImage = Properties.Resources.delete_shield_64px;
                        break;
                    }
                case MsgBoxIcon.INFORMATION:
                    {
                        msgBox.pictureBox_TopIcon.BackgroundImage = Properties.Resources.keyhole_shield_64px;
                        break;
                    }
                case MsgBoxIcon.QUESTION:
                    {
                        msgBox.pictureBox_TopIcon.BackgroundImage = Properties.Resources.question_shield_64px;
                        break;
                    }
                case MsgBoxIcon.WARNING:
                    {
                        msgBox.pictureBox_TopIcon.BackgroundImage = Properties.Resources.warning_shield_64px;
                        break;
                    }
            }
        }
        public static MsgBoxResult Show(string text)
        {
            msgBox = new MsgBox();
            msgBox.pictureBox_TopIcon.Visible = false;
            msgBox.label_TopCaption.Visible = false;
            msgBox.label_CenterMessage.Text = text;
            msgBox.label_CenterMessage.Focus();
            msgBox.TopMost = true;
            msgBox.ShowDialog();
            return Result;
        }
        public static MsgBoxResult Show(string caption, string text)
        {
            msgBox = new MsgBox();
            msgBox.pictureBox_TopIcon.Visible = false;
            msgBox.label_TopCaption.Visible = true;
            msgBox.label_TopCaption.Text = caption;
            msgBox.label_CenterMessage.Text = text;
            msgBox.label_CenterMessage.Focus();
            msgBox.TopMost = true;
            msgBox.ShowDialog();
            return Result;
        }
        public static MsgBoxResult Show(string caption, string text, MsgBoxIcon Icon, MsgBoxButton Button)
        {
            msgBox = new MsgBox();
            msgBox.SetIcon(Icon);
            msgBox.SetButton(Button);
            msgBox.pictureBox_TopIcon.Visible = true;
            msgBox.label_TopCaption.Visible = true;
            msgBox.label_TopCaption.Text = caption;
            msgBox.label_CenterMessage.Text = text;
            msgBox.label_CenterMessage.Focus();
            msgBox.TopMost = true;
            msgBox.ShowDialog();
            return Result;
        }
        private void Button_TopExit_Click(object sender, EventArgs e)
        {
            Result = MsgBoxResult.Abort;
            msgBox.Close();
        }
        private void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Result = MsgBoxResult.Cancel;
            msgBox.Close();
        }
        private void Button_YES_Click(object sender, EventArgs e)
        {
            Result = MsgBoxResult.Yes;
            msgBox.Close();
        }
        private void Button_NO_Click(object sender, EventArgs e)
        {
            Result = MsgBoxResult.No;
            msgBox.Close();
        }
        private void Button_OK_Click(object sender, EventArgs e)
        {
            Result = MsgBoxResult.OK;
            msgBox.Close();
        }
        private void Button_TopExit_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_TopExit, Color.Red);
        }
        private void Button_TopExit_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_TopExit, Color.GreenYellow);
        }
        private void Button_TopExit_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_TopExit, Color.Black);
        }
        private void Button_TopExit_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_TopExit, Color.GreenYellow);
        }
    }
}
