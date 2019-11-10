using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KxSharpLib
{
    public static class FormHelper
    {
        public static Form Find(Type type) { return Application.OpenForms.Cast<Form>().FirstOrDefault(form => form.GetType() == type); }
        public static void Open<T>() where T : Form, new()
        {
            T searchResult = (T)Find(typeof(T));
            if (searchResult == null) (new T()).Show();
            else {
                searchResult.WindowState = (searchResult.WindowState == FormWindowState.Minimized) ? FormWindowState.Normal : searchResult.WindowState;
                searchResult.Select();
            }
        }
        public static void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                DisableControls(c);
            }
            con.Enabled = false;
        }
        public static void EnableControls(Control con)
        {
            if (con != null)
            {
                con.Enabled = true;
                EnableControls(con.Parent);
            }
        }
        public static void HideControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                HideControls(c);
            }
            con.Visible = false;
        }
        public static void ShowControls(Control con)
        {
            if (con != null)
            {
                con.Visible = true;
                ShowControls(con.Parent);
            }
        }
        public static void SetLabelText(Label label, string text)
        {
            if (label.InvokeRequired)
                label.Invoke(new Action(() => label.Text = text));
            else { label.Text = text; }
        }
        public static void Minimize(Form form)
        {
            form.WindowState = FormWindowState.Minimized;
        }
        public static void Maximize(Form form)
        {
            if (form.WindowState.ToString() == "Normal")
            {
                form.WindowState = FormWindowState.Maximized;
            }
            else
            {
                form.WindowState = FormWindowState.Normal;
            }
        }
        public static bool TryDrag(Form form, ref Message message, HashSet<Control> controlstomove)
        {
            if(message.Msg == (int)Win32.WM.LBUTTONDOWN && controlstomove.Contains(Control.FromHandle(message.HWnd)))
            {
                Win32.ReleaseCapture();
                Win32.SendMessage(form.Handle, (int)Win32.WM.NCLBUTTONDOWN, (int)Win32.HT.CAPTION, 0);
                return true;
            }
            return false;
        }
    }
}
