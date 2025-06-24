using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KxEditor.Forms
{
    public partial class Splash : Form, IMessageFilter
    {
        private HashSet<Control> ControlsToMove { get; set; }
        private readonly System.Windows.Forms.Timer pb_timer;

        public Splash()
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
            ControlsToMove = new HashSet<Control> {
                //panel_Top,
            };

            System.Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            label_Version.Text = "Version: 2025";

            pb_timer = new System.Windows.Forms.Timer
            {
                Interval = 10
            };
            pb_timer.Tick += new EventHandler(Progressbar_Tick);
            pb_timer.Start();

        }

        #region PreFilterMessage
        public bool PreFilterMessage(ref Message message)
        {
            return KxSharpLib.FormHelper.TryDrag(this, ref message, ControlsToMove);
        }
        #endregion

        private void Splash_Load(object sender, System.EventArgs e)
        {

        }

        private void Progressbar_Tick(object sender, EventArgs e)
        {
            kxProgressBar.Value += 1;
            if(kxProgressBar.Value == 100) {
                pb_timer.Stop();
            }
        }

        private void Splash_FormClosing(object sender, FormClosingEventArgs e)
        {
            pb_timer.Stop();
        }

        private void label_Version_Click(object sender, EventArgs e)
        {

        }
    }
}
