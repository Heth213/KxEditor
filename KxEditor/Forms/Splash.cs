using System;
using System.Threading;
using System.Windows.Forms;

namespace KxEditor.Forms
{
    public partial class Splash : Form
    {
        private System.Windows.Forms.Timer pb_timer;
        public Splash()
        {
            InitializeComponent();
            System.Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            label_Version.Text = string.Format("Version: {0}.{1:00}", version.Major, version.Minor);

            pb_timer = new System.Windows.Forms.Timer
            {
                Interval = 10
            };
            pb_timer.Tick += new EventHandler(Progressbar_Tick);
            pb_timer.Start();

        }

        private void Splash_Load(object sender, System.EventArgs e)
        {

        }

        private void Progressbar_Tick(object sender, EventArgs e)
        {
            kxProgressBar.Value += 1;
            if(kxProgressBar.Value == 100) {
                pb_timer.Stop();
                //kxProgressBar.Value = 0;
            }
        }

        private void Splash_FormClosing(object sender, FormClosingEventArgs e)
        {
            pb_timer.Stop();
        }
    }
}
