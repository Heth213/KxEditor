using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using KxExtension;
using FastColoredTextBoxNS;
using System.Text.RegularExpressions;

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
        public int treenViewLastSearchedIndex = -1;
        public ContextMenu treeviewContextMenu;
        public MenuItem treeview_RightClickMenueItem_SaveAs;
        public MenuItem treeview_RightClickMenueItem_SaveAll;
        public MenuItem treeview_RightClickMenueItem_Add;
        public MenuItem treeview_RightClickMenueItem_Delete;
        public int treeview_RightClickMenu_ClickedNodeIndex = -1;

        public static Rectangle ScreenResolution => Screen.PrimaryScreen.Bounds;
        private int ResizeHandelSize => 16;
        private Rectangle SizeGripRectangle;
        Style GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);

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
            treeview_RightClickMenueItem_Add = new MenuItem("Add");
            treeview_RightClickMenueItem_Delete = new MenuItem("Delete");
            treeviewContextMenu.MenuItems.Add(treeview_RightClickMenueItem_SaveAll);
            treeviewContextMenu.MenuItems.Add(treeview_RightClickMenueItem_SaveAs);
            treeviewContextMenu.MenuItems.Add(treeview_RightClickMenueItem_Add);
            treeviewContextMenu.MenuItems.Add(treeview_RightClickMenueItem_Delete);

            treeview_RightClickMenueItem_SaveAs.Click += new EventHandler(Treeviw_RightClick_SaveAs);
            treeview_RightClickMenueItem_SaveAll.Click += new EventHandler(Treeviw_RightClick_SaveAll);
            treeview_RightClickMenueItem_Add.Click += new EventHandler(Treeviw_RightClick_Add);
            treeview_RightClickMenueItem_Delete.Click += new EventHandler(Treeviw_RightClick_Delete);

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
                try {
                    if (tabControl1.SelectedTab.Text != "Default")
                        DATList[treenViewLastSelectedIndex].content = Center_EditorTextBox.Text;
                    treeView_PKiew.Nodes[0].Nodes[treenViewLastSelectedIndex].ForeColor = Color.White;
                }
                catch
                { //the node has been deleted
                    }
                }

            KxSharpLib.Kal.DAT item = DATList[treeView_PKiew.SelectedNode.Index];

            Center_EditorTextBox.BeginUpdate();
            Center_EditorTextBox.Text = item.content;
            Center_EditorTextBox.EndUpdate();

            treeView_PKiew.SelectedNode.ForeColor = Color.FromArgb(11, 110, 79); //green
            treeView_PKiew.SelectedNode.BackColor = Color.FromArgb(28, 28, 28); //dark gray

            treenViewLastSelectedIndex = treeView_PKiew.SelectedNode.Index;

            
            logger.Write(string.Format("[File:({0})] >> [SelectedItem:({1})]", item.name, item.index));
            KxSharpLib.FormHelper.SetLabelText(label_CurrentFileTopCenter, string.Format("Current File:   [{0}]", item.name));

            //tabs code
            string title = item.name;
            foreach (TabPage tb in tabControl1.TabPages)
            {
                if (tb.Text == item.name)
                {
                    tabControl1.SelectedTab = tb;
                    return;
                }

            }
            TabPage myTabPage = new TabPage(title);

            //the stupidest thing i've done..
            //changed the tab name to the treeview selected node index so i can convert it to Int later and use it as index LOLOL
            myTabPage.Name = treeView_PKiew.SelectedNode.Index.ToString();

            tabControl1.TabPages.Add(myTabPage);
            tabControl1.SelectedTab = myTabPage;
            if (tabControl1.TabPages[0].Text == "Default" )
            {
                tabControl1.TabPages.Remove(tabControl1.TabPages[0]);
            }


            //tabs end



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
        public void Treeviw_RightClick_Add(object sender, EventArgs e)
        {
            if (treeview_RightClickMenu_ClickedNodeIndex == -1) return;

            if (treeView_PKiew.SelectedNode != null)
            {

                string myNewDatName;
                datName fileNameDialog = new datName();
                if (fileNameDialog.ShowDialog(this) == DialogResult.OK)
                {
                    myNewDatName = datName.datFileName.Trim();
                } else
                {
                    fileNameDialog.Dispose();
                    return;
                }

                fileNameDialog.Dispose();

                if (myNewDatName.Contains(".dat"))
                {
                    myNewDatName = myNewDatName.Replace(".dat", "");
                }


                myNewDatName = myNewDatName + ".dat";
                treeView_PKiew.Nodes[0].Nodes.Add(myNewDatName);
                List<byte> input = new List<byte>();
                DATList.Add(new KxSharpLib.Kal.DAT(DATList.Count + 1, myNewDatName, input.ToArray()));
                logger.Write(string.Format("Added: {0}", myNewDatName));
            }

            treeview_RightClickMenu_ClickedNodeIndex = -1;
        }
        public void Treeviw_RightClick_Delete(object sender, EventArgs e)
        {
            if (treeview_RightClickMenu_ClickedNodeIndex == -1)
                return;

            foreach (TabPage tb in tabControl1.TabPages)
            {
                if (tb.Name == treeview_RightClickMenu_ClickedNodeIndex.ToString())
                {
                    tabControl1.TabPages.Remove(tb);
                }
            }
            logger.Write(string.Format("Removed: {0}", treeView_PKiew.SelectedNode.Text));
            treeView_PKiew.Nodes[0].Nodes.RemoveAt(treeview_RightClickMenu_ClickedNodeIndex);
            DATList.RemoveAt(treeview_RightClickMenu_ClickedNodeIndex);

            
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
            else if (selectedItem.Contains("2018/2019 Config.pk"))
            {
                KxSharpLib.Security.Kal.Crypto.GUseCrypt = KxSharpLib.Security.Kal.Crypto.EUseCrypt.c2018;
                KxSharpLib.Security.Kal.Crypto.GKey = (int)KxSharpLib.Security.Kal.Crypto.EKeys.CFG + (int)KxSharpLib.Security.Kal.Crypto.EKeys.CFGADD;
                logger.Write(string.Format("Using CryptTable from 2018/2019 and Config.pk key({0})", KxSharpLib.Security.Kal.Crypto.GKey));
            }
            else if (selectedItem.Contains("2018/2019 E.pk"))
            {
                KxSharpLib.Security.Kal.Crypto.GUseCrypt = KxSharpLib.Security.Kal.Crypto.EUseCrypt.c2018;
                KxSharpLib.Security.Kal.Crypto.GKey = (int)KxSharpLib.Security.Kal.Crypto.EKeys.E;
                logger.Write(string.Format("Using CryptTable from 2018/2019 and E.pk key({0})", KxSharpLib.Security.Kal.Crypto.GKey));
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
            //KxSharpLib.Win32.SetControlForeColor(button_LeftTopAppName, Color.Red);
            //button_LeftTopAppName.Font = new Font("Ink Free", 16.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_LeftTopAppName_MouseEnter(object sender, EventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_LeftTopAppName, Color.GreenYellow);
        }
        private void Button_LeftTopAppName_MouseLeave(object sender, EventArgs e)
        {
           // KxSharpLib.Win32.SetControlForeColor(button_LeftTopAppName, SystemColors.ControlText);
        }
        private void Button_LeftTopAppName_MouseUp(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_LeftTopAppName, Color.GreenYellow);
           // button_LeftTopAppName.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }

        private void Button_RightBottomCopyright_MouseEnter(object sender, EventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_RightBottomCopyright, Color.DimGray);
          
        }
        private void Button_RightBottomCopyright_MouseLeave(object sender, EventArgs e)
        {
          
            //KxSharpLib.Win32.SetControlForeColor(button_RightBottomCopyright, Color.Silver);
        }
        private void Button_RightBottomCopyright_MouseDown(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_RightBottomCopyright, Color.White);
        }
        private void Button_RightBottomCopyright_MouseUp(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_RightBottomCopyright, Color.SteelBlue);
        }

        private void Button_MenuLeftFileOpen_MouseDown(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_MenuLeftFileOpen, Color.GreenYellow);
            //button_MenuLeftFileOpen.Font = new Font("Ink Free", 13.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_MenuLeftFileOpen_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_MenuLeftFileOpen, Color.DimGray);
            //button_MenuLeftFileOpen.Font = new Font("Ink Free", 12.0F, FontStyle.Bold);
        }
        private void Button_MenuLeftFileOpen_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_MenuLeftFileOpen, Color.Silver);
            //button_MenuLeftFileOpen.Font = new Font("Ink Free", 12.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_MenuLeftFileOpen_MouseUp(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_MenuLeftFileOpen, Color.SteelBlue);
            // button_MenuLeftFileOpen.Font = new Font("Ink Free", 12.0F, FontStyle.Bold);
        }

        private void Button_MenuLeftSaveFile_MouseDown(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_MenuLeftSaveFile, Color.GreenYellow);
            // button_MenuLeftSaveFile.Font = new Font("Ink Free", 13.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_MenuLeftSaveFile_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_MenuLeftSaveFile, Color.DimGray);
            //button_MenuLeftSaveFile.Font = new Font("Ink Free", 12.0F, FontStyle.Bold);
        }
        private void Button_MenuLeftSaveFile_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Win32.SetControlForeColor(button_MenuLeftSaveFile, Color.Silver);
            //button_MenuLeftSaveFile.Font = new Font("Ink Free", 12.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_MenuLeftSaveFile_MouseUp(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_MenuLeftSaveFile, Color.SteelBlue);
            //button_MenuLeftSaveFile.Font = new Font("Ink Free", 12.0F, FontStyle.Bold);
        }

        private void Button_RightTopExit_MouseDown(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_RightTopExit, Color.Red);
            //button_RightTopExit.Font = new Font("Ink Free", 17.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopExit_MouseEnter(object sender, EventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_RightTopExit, Color.GreenYellow);
            //button_RightTopExit.Font = new Font("Ink Free", 16.0F, FontStyle.Bold);
        }
        private void Button_RightTopExit_MouseLeave(object sender, EventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_RightTopExit, Color.Black);
            // button_RightTopExit.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopExit_MouseUp(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_RightTopExit, Color.GreenYellow);
            // button_RightTopExit.Font = new Font("Ink Free", 16.0F, FontStyle.Bold);
        }

        private void Button_RightTopMaximize_MouseDown(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_RightTopMaximize, Color.Red);
            //button_RightTopMaximize.Font = new Font("Ink Free", 17.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopMaximize_MouseEnter(object sender, EventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_RightTopMaximize, Color.GreenYellow);
            //button_RightTopMaximize.Font = new Font("Ink Free", 16.0F, FontStyle.Bold);
        }
        private void Button_RightTopMaximize_MouseLeave(object sender, EventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_RightTopMaximize, Color.Black);
            // button_RightTopMaximize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopMaximize_MouseUp(object sender, MouseEventArgs e)
        {
           // KxSharpLib.Win32.SetControlForeColor(button_RightTopMaximize, Color.GreenYellow);
            // button_RightTopMaximize.Font = new Font("Ink Free", 16.0F, FontStyle.Bold);
        }

        private void Button_RightTopMinimize_MouseDown(object sender, MouseEventArgs e)
        {
           // KxSharpLib.Win32.SetControlForeColor(button_RightTopMinimize, Color.Red);
            // button_RightTopMinimize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopMinimize_MouseEnter(object sender, EventArgs e)
        {
           // KxSharpLib.Win32.SetControlForeColor(button_RightTopMinimize, Color.GreenYellow);
            //button_RightTopMinimize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold);
        }
        private void Button_RightTopMinimize_MouseLeave(object sender, EventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_RightTopMinimize, Color.Black);
            //button_RightTopMinimize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopMinimize_MouseUp(object sender, MouseEventArgs e)
        {
            //KxSharpLib.Win32.SetControlForeColor(button_RightTopMinimize, Color.GreenYellow);
            // button_RightTopMinimize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold);
        }
        #endregion

        #region Menu Buttons
        private void Button_RightTopExit_Click(object sender, EventArgs e)
        {
            DialogResult result = KxMsgBox.Show("Exit?", "Do you want to close?", KxMsgBoxIcon.QUESTION, KxMsgBoxButton.YESNO);
            if (result == DialogResult.Yes)
            {
                Properties.Settings.Default.selectedEBG = EditorBG_comboBox.SelectedIndex;
                Properties.Settings.Default.selectedSyntax = comboBox1.SelectedIndex;
                Properties.Settings.Default.Save();
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
                            Setting_CryptTable_comboBox.Enabled = true;
                            Center_EditorTextBox.Clear();
                            LoadedPK = new KxSharpLib.Kal.PK();
                            DATList.Clear();
                            DATList = null;
                            treeView_PKiew.Nodes.Clear();

                            //tabs code
                            tabControl1.TabPages.Clear();
                            TabPage defaultTab = new TabPage("Default");
                            defaultTab.Name = "Default";
                            tabControl1.TabPages.Add(defaultTab);
                            tabControl1.SelectedTab = defaultTab;
                            tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(Center_EditorTextBox);
                            //tabs end

                            
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            EditorBG_comboBox.SelectedIndex = Properties.Settings.Default.selectedEBG;
            comboBox1.SelectedIndex = Properties.Settings.Default.selectedSyntax;

            slidingPanel.Start();
        }

        private void panel_RightTopExit_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Center_EditorTextBox_Load(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (treeView_PKiew.Nodes.Count == 0) return;

            //im using the stupidest thing 
            int selectedTabIndex = Int32.Parse(tabControl1.SelectedTab.Name);

            TreeNode selectedNode = treeView_PKiew.Nodes[0].Nodes[selectedTabIndex];
      
            if (selectedNode == null)
                return;

            if (selectedNode.Index == -1 || selectedNode == treeView_PKiew.TopNode)
                return;

            if (treenViewLastSelectedIndex != -1)
            {
                try
                {
                    if (tabControl1.SelectedTab.Text != "Default")
                        DATList[treenViewLastSelectedIndex].content = Center_EditorTextBox.Text;
                    treeView_PKiew.Nodes[0].Nodes[treenViewLastSelectedIndex].ForeColor = Color.White;
                }
                catch { //the node has been deleted
                }
            }

            treeView_PKiew.SelectedNode = selectedNode;
            KxSharpLib.Kal.DAT item = DATList[selectedNode.Index];

                Center_EditorTextBox.BeginUpdate();
                Center_EditorTextBox.Text = item.content;
                Center_EditorTextBox.EndUpdate();
            
            treenViewLastSelectedIndex = selectedTabIndex;

            tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(Center_EditorTextBox);

            selectedNode.ForeColor = Color.FromArgb(11, 110, 79); //green
            selectedNode.BackColor = Color.FromArgb(28, 28, 28); //dark gray
            logger.Write(string.Format("[Tab Selected:({0})", item.name));
            KxSharpLib.FormHelper.SetLabelText(label_CurrentFileTopCenter, string.Format("Current File:   [{0}]", item.name));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text != "Default") {
                
                if (tabControl1.TabPages.Count != 1) {

                    //the last time im using it.
                    int selectedTabIndex = Int32.Parse(tabControl1.SelectedTab.Name);

                    tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                    treeView_PKiew.Nodes[0].Nodes[selectedTabIndex].BackColor = Color.FromArgb(45, 45, 48); // back to normal background
                    logger.Write(string.Format("[Tab Closed:({0})", tabControl1.SelectedTab.Text));
                }

            }
        }

        private void Center_EditorTextBox_Load_1(object sender, EventArgs e)
        {

        }

        private void Center_EditorTextBox_TextChangedDelayed(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {

        }
        
        private void Center_EditorTextBox_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            //comment green highlighting
            e.ChangedRange.ClearStyle(GreenStyle);
            e.ChangedRange.SetStyle(GreenStyle, @";.*$", RegexOptions.Multiline);
        }

        private void Setting_CryptTable_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                return;

            var selectedSyntax = comboBox1.SelectedItem.ToString();
            if (selectedSyntax.Contains("None"))
            {
                Center_EditorTextBox.Language = FastColoredTextBoxNS.Language.Custom;
            }
            else if (selectedSyntax.Contains("Highlight"))
            {
                Center_EditorTextBox.Language = FastColoredTextBoxNS.Language.JS;
            }
            


        }

        private void EditorBG_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EditorBG_comboBox.SelectedIndex == -1)
                return;

            var selectedSyntax = EditorBG_comboBox.SelectedItem.ToString();
            if (selectedSyntax.Contains("Light"))
            {
                Center_EditorTextBox.BackColor = Color.Gainsboro;
                Center_EditorTextBox.ForeColor = Color.Black;
                Center_EditorTextBox.CaretColor = Color.Black;
            }
            else if (selectedSyntax.Contains("Dark"))
            {
                Center_EditorTextBox.BackColor = Color.FromArgb(28, 28, 28);
                Center_EditorTextBox.ForeColor = Color.Silver;
                Center_EditorTextBox.CaretColor = Color.White;
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "Search")
                textBox1.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.IndianRed;
            if (textBox1.Text.Trim() != "")
            {
                if (treeView_PKiew.Nodes.Count > 0)
                {
                    if(treenViewLastSearchedIndex != -1) {
                        try { 
                    treeView_PKiew.Nodes[0].Nodes[treenViewLastSearchedIndex].BackColor = Color.FromArgb(45, 45, 48); // normal bg
                        }
                        catch { //the node has been deleted
                        }
                    }

                    foreach (TreeNode tn in treeView_PKiew.Nodes[0].Nodes)
                    {
                        if (tn.Text.ToUpper().Contains(textBox1.Text.Trim().ToUpper().ToString()))
                        {
 
                            treeView_PKiew.SelectedNode = tn;
                            tn.BackColor = Color.FromArgb(255, 124, 84); // yellow
                            textBox1.ForeColor = Color.LightGreen;
                            treenViewLastSearchedIndex = tn.Index;
                            break;
                        }
   
                    }

                }
            }
                

  



            

        }
    }
}