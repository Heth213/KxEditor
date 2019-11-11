using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using KxExtension;

namespace KxEditor
{
    public partial class MainForm : Form, IMessageFilter
    {
        #region vars
        public static MainForm Instance { get; set; }
        private HashSet<Control> ControlsToMove { get; set; }

        public KxSharpLib.Kal.PK LoadedPK;
        public List<KxSharpLib.Kal.DAT> DATList;

        public Configuration configuration;
        public KxSharpLib.Utility.Logger logger;

        public int treenViewLastSelectedIndex = -1;
        public ContextMenu treeviewContextMenu;
        public MenuItem treeview_RightClickMenueItem_SaveAs;
        public MenuItem treeview_RightClickMenueItem_SaveAll;
        public int treeview_RightClickMenu_ClickedNodeIndex = -1;

        public static Rectangle ScreenResolution => Screen.PrimaryScreen.Bounds;
        private int ResizeHandelSize => 16;
        private Rectangle SizeGripRectangle;

        public class Sliding_Panel 
        {
            public Panel panel;
            public System.Windows.Forms.Timer timer;
            public bool ishidden;
            public int maxwidth;
            public int widthincrement;
            public Sliding_Panel(Panel _panel, int _maxwidth, int _widthincrement, int _timerinterval)
            {
                panel = _panel;
                timer = new System.Windows.Forms.Timer
                {
                    Interval = _timerinterval
                };
                timer.Tick += new EventHandler(Timer_Tick);

                maxwidth = _maxwidth;
                panel.Width = 0;
                ishidden = true;
                widthincrement = _widthincrement;
            }

            public void Timer_Tick(object sender, EventArgs e)
            {
                if (ishidden)
                {
                    panel.Width += widthincrement;
                    if (panel.Width >= maxwidth)
                    {
                        Stop();
                        ishidden = false;
                        panel.Refresh();
                    }
                }
                else
                {
                    panel.Width -= widthincrement;
                    if (panel.Width <= 0)
                    {
                        Stop();
                        ishidden = true;
                        panel.Refresh();
                    }
                }
            }
            public void Start()
            {
                timer.Start();
            }
            public void Stop()
            {
                timer.Stop();
            }
        }
        public Sliding_Panel slidingPanel;
        #endregion

        #region Splash
        readonly Thread splashThread;
        public void ShowSplash()
        {
            Application.Run(new Forms.Splash());
        }
        #endregion

        #region Constructor
        public MainForm()
        {
            splashThread = new Thread(new ThreadStart(ShowSplash));
            splashThread.Start();
            Thread.Sleep(2000);

            InitializeComponent();
            Instance = this;
            Application.AddMessageFilter(Instance);
            ControlsToMove = new HashSet<Control> {
                Instance.panel_CenterTop,
                Instance.panel_RightTopExit,
            };

            Instance.FormBorderStyle = FormBorderStyle.None;
            Instance.DoubleBuffered = true;
            Instance.SetStyle(ControlStyles.ResizeRedraw, true);
            Instance.Size = new Size(950, 500);
            Instance.MaximumSize = new Size(ScreenResolution.Width + 100, ScreenResolution.Height + 100);
            Instance.MinimumSize = new Size(600, 400);
            Instance.Center_EditorTextBox.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            treeviewContextMenu = new ContextMenu();
            treeview_RightClickMenueItem_SaveAll = new MenuItem("Save All");
            treeview_RightClickMenueItem_SaveAs = new MenuItem("SaveAs...");
            treeviewContextMenu.MenuItems.Add(treeview_RightClickMenueItem_SaveAll);
            treeviewContextMenu.MenuItems.Add(treeview_RightClickMenueItem_SaveAs);
            treeview_RightClickMenueItem_SaveAs.Click += new EventHandler(Treeviw_RightClick_SaveAs);
            treeview_RightClickMenueItem_SaveAll.Click += new EventHandler(Treeviw_RightClick_SaveAll);

            slidingPanel = new Sliding_Panel(panel_MenuLeft, 130, 15, 10);

            logger = new KxSharpLib.Utility.Logger(Center_LoggingTextBox);
            configuration = new Configuration();
            LoadedPK = new KxSharpLib.Kal.PK();

            if (configuration.UseConfigurationFile)
            {
                KxSharpLib.FormHelper.DisableControls(Settings_groupBox);
                KxSharpLib.FormHelper.HideControls(Settings_groupBox);
            }
            logger.Write("Initialized and ready!");

            splashThread.Abort();
            KxSharpLib.Win32.SwitchToThisWindow(this.Handle, true);
        }
        #endregion

        #region PreFilterMessage
        public bool PreFilterMessage(ref Message message)
        {
            return KxSharpLib.FormHelper.TryDrag(this, ref message, ControlsToMove);
        }
        #endregion

        #region Overwritten Functions
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components?.Dispose();
                treeviewContextMenu?.Dispose();
                treeview_RightClickMenueItem_SaveAs?.Dispose();
                treeview_RightClickMenueItem_SaveAll?.Dispose();
            }
            base.Dispose(disposing);
        }
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case (int)KxSharpLib.Win32.WM.NCHITTEST:
                    base.WndProc(ref message);
                    if (SizeGripRectangle.Contains(PointToClient(new Point(message.LParam.ToInt32() & 0xffff, message.LParam.ToInt32() >> 16)))) message.Result = new IntPtr((int)KxSharpLib.Win32.HT.BOTTOMRIGHT);
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
            SizeGripRectangle = new Rectangle(ClientRectangle.Width - ResizeHandelSize, ClientRectangle.Height - ResizeHandelSize, ResizeHandelSize, ResizeHandelSize);
            NewRegion.Exclude(SizeGripRectangle);
            panel_Main.Region = NewRegion;
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, BackColor, SizeGripRectangle);
        }
        #endregion

        #region TreeView
        private void TreeView_PKiew_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var clickPoint = new Point(e.X, e.Y);
                TreeNode selectedNode = ((TreeView)sender).GetNodeAt(clickPoint);
                if (selectedNode == null)
                    return;

                treeview_RightClickMenu_ClickedNodeIndex = selectedNode.Index;

                var screenPoint = treeView_PKiew.PointToScreen(clickPoint);
                var formPoint = PointToClient(screenPoint);
                treeviewContextMenu.Show(this, formPoint);
            }
        }
        private void TreeView_PKiew_DoubleClick(object sender, EventArgs e)
        {
            if (treeView_PKiew.SelectedNode == null)
                return;

            if (treeView_PKiew.SelectedNode.Index == -1 || treeView_PKiew.SelectedNode == treeView_PKiew.TopNode)
                return;

            if (treenViewLastSelectedIndex != -1)
            {
                DATList[treenViewLastSelectedIndex].content = Center_EditorTextBox.Text;
            }

            KxSharpLib.Kal.DAT item = DATList[treeView_PKiew.SelectedNode.Index];
            Center_EditorTextBox.Text = item.content;
            treenViewLastSelectedIndex = treeView_PKiew.SelectedNode.Index;

            logger.Write(string.Format("[File:({0})] >> [SelectedItem:({1})]", item.name, item.index));
            KxSharpLib.FormHelper.SetLabelText(label_CurrentFileTopCenter, string.Format("Current File:   [{0}]", item.name));

        }
        public void Treeviw_RightClick_SaveAs(object sender, EventArgs e)
        {
            if (treeview_RightClickMenu_ClickedNodeIndex == -1)
                return;

            using(var saveFileDia = new SaveFileDialog())
            {
                var dat = DATList[treeview_RightClickMenu_ClickedNodeIndex];
                saveFileDia.FileName = dat.ToString().Replace(".dat", ".txt");
                var datwithoutExt = dat.ToString().Replace(".dat", "");
                if (saveFileDia.ShowDialog(this) == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDia.FileName, dat.content);
                    logger.Write(string.Format("Saved: {0} to {1}", datwithoutExt, saveFileDia.FileName));
                }

            }
            treeview_RightClickMenu_ClickedNodeIndex = -1;
        }
        public void Treeviw_RightClick_SaveAll(object sender, EventArgs e)
        {
            if (treeview_RightClickMenu_ClickedNodeIndex == -1)
                return;

            using (var folderBrowserDia = new FolderBrowserDialog())
            {
                if (folderBrowserDia.ShowDialog(this) == DialogResult.OK)
                {
                    var folderPath = folderBrowserDia.SelectedPath;

                    logger.Write(string.Format("FolderPath: {0}", folderPath));
                    foreach (KxSharpLib.Kal.DAT dat in DATList)
                    {
                        var datAstxt = dat.ToString().Replace(".dat", ".txt");
                        var finalPath = Path.Combine(folderPath, datAstxt);
                        dat.content.WriteToFile(finalPath);
                        logger.Write(string.Format("Saved: {0} to {1}", datAstxt, finalPath));
                    }
                }
            }
            treeview_RightClickMenu_ClickedNodeIndex = -1;
        }
        #endregion

        #region CryptTable Combobox
        private void Setting_CryptTable_comboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (Setting_CryptTable_comboBox.SelectedIndex == -1)
                return;

            var selectedItem = Setting_CryptTable_comboBox.SelectedItem.ToString();
            if (selectedItem.Contains("2006 Config.pk"))
            {
                KxSharpLib.Security.Kal.Crypto.GUseCrypt = KxSharpLib.Security.Kal.Crypto.EUseCrypt.c2006;
                KxSharpLib.Security.Kal.Crypto.GKey = (int)KxSharpLib.Security.Kal.Crypto.EKeys.CFG;
                logger.Write(string.Format("Using CryptTable from 2006 and Config.pk key({0})", KxSharpLib.Security.Kal.Crypto.GKey));
            }
            else if (selectedItem.Contains("2006 E.pk"))
            {
                KxSharpLib.Security.Kal.Crypto.GUseCrypt = KxSharpLib.Security.Kal.Crypto.EUseCrypt.c2006;
                KxSharpLib.Security.Kal.Crypto.GKey = (int)KxSharpLib.Security.Kal.Crypto.EKeys.E;
                logger.Write(string.Format("Using CryptTable from 2006 and E.pk key({0})", KxSharpLib.Security.Kal.Crypto.GKey));
            }
            else if (selectedItem.Contains("2015 Config.pk"))
            {
                KxSharpLib.Security.Kal.Crypto.GUseCrypt = KxSharpLib.Security.Kal.Crypto.EUseCrypt.c2015;
                KxSharpLib.Security.Kal.Crypto.GKey = (int)KxSharpLib.Security.Kal.Crypto.EKeys.CFG;
                logger.Write(string.Format("Using CryptTable from 2015 and Config.pk key({0})", KxSharpLib.Security.Kal.Crypto.GKey));
            }
            else if (selectedItem.Contains("2015 E.pk"))
            {
                KxSharpLib.Security.Kal.Crypto.GUseCrypt = KxSharpLib.Security.Kal.Crypto.EUseCrypt.c2015;
                KxSharpLib.Security.Kal.Crypto.GKey = (int)KxSharpLib.Security.Kal.Crypto.EKeys.E;
                logger.Write(string.Format("Using CryptTable from 2015 and E.pk key({0})", KxSharpLib.Security.Kal.Crypto.GKey));
            }
            else if (selectedItem.Contains("2018 Config.pk"))
            {
                KxSharpLib.Security.Kal.Crypto.GUseCrypt = KxSharpLib.Security.Kal.Crypto.EUseCrypt.c2018;
                KxSharpLib.Security.Kal.Crypto.GKey = (int)KxSharpLib.Security.Kal.Crypto.EKeys.CFG + (int)KxSharpLib.Security.Kal.Crypto.EKeys.CFGADD;
                logger.Write(string.Format("Using CryptTable from 2018 and Config.pk key({0})", KxSharpLib.Security.Kal.Crypto.GKey));
            }
            else if (selectedItem.Contains("2018 E.pk"))
            {
                KxSharpLib.Security.Kal.Crypto.GUseCrypt = KxSharpLib.Security.Kal.Crypto.EUseCrypt.c2018;
                KxSharpLib.Security.Kal.Crypto.GKey = (int)KxSharpLib.Security.Kal.Crypto.EKeys.E;
                logger.Write(string.Format("Using CryptTable from 2018 and E.pk key({0})", KxSharpLib.Security.Kal.Crypto.GKey));
            }
            else
            {
                KxSharpLib.Security.Kal.Crypto.GUseCrypt = KxSharpLib.Security.Kal.Crypto.EUseCrypt.Unknown;
                logger.Write("Unknown CryptTable selected, select a valid option.");
            }
        }
        #endregion

        #region Form Resize
        private void Panel_CenterTop_DoubleClick(object sender, EventArgs e)
        {
            KxSharpLib.FormHelper.Maximize(this);
        }
        private void Button_RightTopMinimize_Click(object sender, EventArgs e)
        {
            KxSharpLib.FormHelper.Minimize(this);
        }
        private void Button_RightTopMaximize_Click(object sender, EventArgs e)
        {
            KxSharpLib.FormHelper.Maximize(this);
        }
        #endregion

        #region Button Events MouseDown/Up/Enter/Leave
        private void Button_RightBottomCopyright_Click(object sender, EventArgs e)
        {
            KxSharpLib.FormHelper.Open<About>();
        }

        private void Button_LeftTopAppName_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_LeftTopAppName, Color.Red);
            button_LeftTopAppName.Font = new Font("Ink Free", 16.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_LeftTopAppName_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_LeftTopAppName, Color.GreenYellow);
        }
        private void Button_LeftTopAppName_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_LeftTopAppName, SystemColors.ControlText);
        }
        private void Button_LeftTopAppName_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_LeftTopAppName, Color.GreenYellow);
            button_LeftTopAppName.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }

        private void Button_RightBottomCopyright_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightBottomCopyright, Color.SteelBlue);
        }
        private void Button_RightBottomCopyright_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightBottomCopyright, Color.Black);
        }
        private void Button_RightBottomCopyright_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightBottomCopyright, Color.White);
        }
        private void Button_RightBottomCopyright_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightBottomCopyright, Color.SteelBlue);
        }

        private void Button_MenuLeftFileOpen_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_MenuLeftFileOpen, Color.GreenYellow);
            //button_MenuLeftFileOpen.Font = new Font("Ink Free", 13.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_MenuLeftFileOpen_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_MenuLeftFileOpen, Color.SteelBlue);
            //button_MenuLeftFileOpen.Font = new Font("Ink Free", 12.0F, FontStyle.Bold);
        }
        private void Button_MenuLeftFileOpen_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_MenuLeftFileOpen, Color.Black);
            //button_MenuLeftFileOpen.Font = new Font("Ink Free", 12.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_MenuLeftFileOpen_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_MenuLeftFileOpen, Color.SteelBlue);
            // button_MenuLeftFileOpen.Font = new Font("Ink Free", 12.0F, FontStyle.Bold);
        }

        private void Button_MenuLeftSaveFile_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_MenuLeftSaveFile, Color.GreenYellow);
            // button_MenuLeftSaveFile.Font = new Font("Ink Free", 13.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_MenuLeftSaveFile_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_MenuLeftSaveFile, Color.SteelBlue);
            //button_MenuLeftSaveFile.Font = new Font("Ink Free", 12.0F, FontStyle.Bold);
        }
        private void Button_MenuLeftSaveFile_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_MenuLeftSaveFile, Color.Black);
            //button_MenuLeftSaveFile.Font = new Font("Ink Free", 12.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_MenuLeftSaveFile_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_MenuLeftSaveFile, Color.SteelBlue);
            //button_MenuLeftSaveFile.Font = new Font("Ink Free", 12.0F, FontStyle.Bold);
        }

        private void Button_RightTopExit_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopExit, Color.Red);
            //button_RightTopExit.Font = new Font("Ink Free", 17.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopExit_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopExit, Color.GreenYellow);
            //button_RightTopExit.Font = new Font("Ink Free", 16.0F, FontStyle.Bold);
        }
        private void Button_RightTopExit_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopExit, Color.Black);
            // button_RightTopExit.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopExit_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopExit, Color.GreenYellow);
            // button_RightTopExit.Font = new Font("Ink Free", 16.0F, FontStyle.Bold);
        }

        private void Button_RightTopMaximize_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopMaximize, Color.Red);
            //button_RightTopMaximize.Font = new Font("Ink Free", 17.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopMaximize_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopMaximize, Color.GreenYellow);
            //button_RightTopMaximize.Font = new Font("Ink Free", 16.0F, FontStyle.Bold);
        }
        private void Button_RightTopMaximize_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopMaximize, Color.Black);
            // button_RightTopMaximize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopMaximize_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopMaximize, Color.GreenYellow);
            // button_RightTopMaximize.Font = new Font("Ink Free", 16.0F, FontStyle.Bold);
        }

        private void Button_RightTopMinimize_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopMinimize, Color.Red);
            // button_RightTopMinimize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopMinimize_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopMinimize, Color.GreenYellow);
            //button_RightTopMinimize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold);
        }
        private void Button_RightTopMinimize_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopMinimize, Color.Black);
            //button_RightTopMinimize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopMinimize_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_RightTopMinimize, Color.GreenYellow);
            // button_RightTopMinimize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold);
        }
        #endregion

        #region Menu Buttons
        private void Button_RightTopExit_Click(object sender, EventArgs e)
        {
            DialogResult result = KxMsgBox.Show("Exit?", "Do you want to close?", KxMsgBoxIcon.QUESTION, KxMsgBoxButton.YESNO);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void Button_MenuLeftFileOpen_Click(object sender, EventArgs e)
        {
            if (Setting_CryptTable_comboBox.SelectedIndex == -1 || KxSharpLib.Security.Kal.Crypto.GUseCrypt == KxSharpLib.Security.Kal.Crypto.EUseCrypt.Unknown)
            {
                logger.Write("[Unknown CryptTable, Please set a valid table!]");
                return;
            }

            if (DATList != null)
            {
                logger.Write("[There is already a file loaded!]");

                DialogResult diaResult = KxMsgBox.Show("Already open!", "There is already a file loaded!\nClose without saving ?", KxMsgBoxIcon.WARNING, KxMsgBoxButton.YESNO);
                switch (diaResult)
                {
                    case DialogResult.Yes:
                        {
                            Center_EditorTextBox.Clear();
                            LoadedPK = new KxSharpLib.Kal.PK();
                            DATList.Clear();
                            DATList = null;
                            treeView_PKiew.Nodes.Clear();
                            Setting_CryptTable_comboBox.Enabled = true;
                            KxSharpLib.FormHelper.SetLabelText(label_CurrentFileTopCenter, "Current File:   [None]");
                            break;
                        }

                    case DialogResult.No:
                        return;
                    default:
                        return;
                }
            }

            using (OpenFileDialog filedia = new OpenFileDialog())
            {
                filedia.Title = "Open PK File";
                filedia.Filter = "PK files|*.pk";
                filedia.CheckFileExists = true;
                filedia.CheckPathExists = true;

                switch (filedia.ShowDialog(this))
                {
                    case DialogResult.OK:
                        {
                            _ = new PackageHandler(filedia.FileName);
                            Setting_CryptTable_comboBox.Enabled = false;
                            textBox_FileInfo_Name.Text = Path.GetFileName(filedia.FileName);
                            textBox_FileInfo_Path.Text = filedia.FileName;
                            using (var md5 = MD5.Create())
                            {
                                textBox_FileInfo_MD5.Text = BitConverter.ToString(md5.ComputeHash(File.ReadAllBytes(filedia.FileName))).Replace("-", "").ToLower();
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

        }

        private void Button_MenuLeftSaveFile_Click(object sender, EventArgs e)
        {
            if (DATList != null)
                PackageHandler.Save();
            else
                logger.Write(string.Format("Load a File before trying to save!"));
        }

        private void Button_TopLeftLogo_Click(object sender, EventArgs e)
        {
            slidingPanel.Start();
        }
        #endregion

    }
}