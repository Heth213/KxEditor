using System;
using System.Windows.Forms;

namespace KxEditor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using (new KxSharpLib.Utility.SingleInstanceMutex(1000))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
