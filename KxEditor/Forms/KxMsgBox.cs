using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KxEditor
{
    public enum KxMsgBoxButton
    {
        OK,
        OKCANCEL,
        YESNO,
        YESNOCANCEL,
        RETRYCANCEL,
        HELP,
    }
    public enum KxMsgBoxIcon
    {
        ERROR,
        QUESTION,
        WARNING,
        INFORMATION,
    }

    public partial class KxMsgBox : Form, IMessageFilter
    {
        private HashSet<Control> ControlsToMove { get; set; }
        static KxMsgBox MsgBox;
        static DialogResult Result;

        public KxMsgBox()
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
        public void SetButton(KxMsgBoxButton btn)
        {
            switch (btn)
            {
                case KxMsgBoxButton.OK:
                    {
                        MsgBox.button_OK.Location = new Point(390, 90);
                        MsgBox.button_OK.Visible = true;
                        break;
                    }
                case KxMsgBoxButton.YESNO:
                    {
                        MsgBox.button_YES.Location = new Point(390, 90);
                        MsgBox.button_YES.Visible = true;
                        MsgBox.button_NO.Location = new Point(310, 90);
                        MsgBox.button_NO.Visible = true;
                        break;
                    }
                case KxMsgBoxButton.OKCANCEL:
                    {
                        MsgBox.button_OK.Location = new Point(390, 90);
                        MsgBox.button_OK.Visible = true;
                        MsgBox.button_CANCEL.Location = new Point(310, 90);
                        MsgBox.button_CANCEL.Visible = true;
                        break;
                    }
            }
        }
        public void SetIcon(KxMsgBoxIcon Icon)
        {
            switch (Icon)
            {
                case KxMsgBoxIcon.ERROR:
                    {
                        MsgBox.pictureBox_TopIcon.BackgroundImage = Properties.Resources.delete_shield_64px;
                        break;
                    }
                case KxMsgBoxIcon.INFORMATION:
                    {
                        MsgBox.pictureBox_TopIcon.BackgroundImage = Properties.Resources.keyhole_shield_64px;
                        break;
                    }
                case KxMsgBoxIcon.QUESTION:
                    {
                        MsgBox.pictureBox_TopIcon.BackgroundImage = Properties.Resources.question_shield_64px;
                        break;
                    }
                case KxMsgBoxIcon.WARNING:
                    {
                        MsgBox.pictureBox_TopIcon.BackgroundImage = Properties.Resources.warning_shield_64px;
                        break;
                    }
            }
        }
        public static DialogResult Show(string text)
        {
            MsgBox = new KxMsgBox();
            MsgBox.pictureBox_TopIcon.Visible = false;
            MsgBox.label_TopCaption.Visible = false;
            MsgBox.label_CenterMessage.Text = text;
            MsgBox.label_CenterMessage.Focus();
            MsgBox.TopMost = true;
            MsgBox.ShowDialog();
            return Result;
        }
        public static DialogResult Show(string caption, string text)
        {
            MsgBox = new KxMsgBox();
            MsgBox.pictureBox_TopIcon.Visible = false;
            MsgBox.label_TopCaption.Visible = true;
            MsgBox.label_TopCaption.Text = caption;
            MsgBox.label_CenterMessage.Text = text;
            MsgBox.label_CenterMessage.Focus();
            MsgBox.TopMost = true;
            MsgBox.ShowDialog();
            return Result;
        }
        public static DialogResult Show(string caption, string text, KxMsgBoxIcon Icon, KxMsgBoxButton Button)
        {
            MsgBox = new KxMsgBox();
            MsgBox.SetIcon(Icon);
            MsgBox.SetButton(Button);
            MsgBox.pictureBox_TopIcon.Visible = true;
            MsgBox.label_TopCaption.Visible = true;
            MsgBox.label_TopCaption.Text = caption;
            MsgBox.label_CenterMessage.Text = text;
            MsgBox.label_CenterMessage.Focus();
            MsgBox.TopMost = true;
            MsgBox.ShowDialog();
            return Result;
        }
        private void Button_TopExit_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Abort;
            MsgBox.Close();
        }
        private void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            MsgBox.Close();
        }
        private void Button_YES_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Yes;
            MsgBox.Close();
        }
        private void Button_NO_Click(object sender, EventArgs e)
        {
            Result = DialogResult.No;
            MsgBox.Close();
        }
        private void Button_OK_Click(object sender, EventArgs e)
        {
            Result = DialogResult.OK;
            MsgBox.Close();
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
