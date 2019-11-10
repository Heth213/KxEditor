using System.Windows.Forms;

namespace KxEditor.Forms
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        private void Splash_Load(object sender, System.EventArgs e)
        {
            System.Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            label_Version.Text = string.Format("Version: {0}.{1:00}", version.Major, version.Minor);
        }
    }
}
