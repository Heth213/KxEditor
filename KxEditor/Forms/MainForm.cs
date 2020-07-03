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
        public static Rectangle ScreenResolution => Screen.PrimaryScreen.Bounds;

        public KxSharpLib.File.PkFile gCurrentFile;
        public Configuration configuration;
        public KxSharpLib.Utility.Logger logger;

        public int treenViewLastSelectedIndex = -1;
        public ContextMenu treeviewContextMenu;
        public MenuItem treeview_RightClickMenueItem_SaveAs;
        public MenuItem treeview_RightClickMenueItem_SaveAll;
        public MenuItem treeview_RightClickMenueItem_Add;
        public MenuItem treeview_RightClickMenueItem_Delete;
        public int treeview_RightClickMenu_ClickedNodeIndex = -1;

        public KxSharpLib.Util.SlidingPanel slidingPanel;

        private int ResizeHandelSize => 16;
        private Rectangle SizeGripRectangle;
        private HashSet<Control> ControlsToMove { get; set; }
        #endregion

        #region Splash
        readonly Thread splashThread;
        public void ShowSplash() { Application.Run(new Forms.Splash()); }
        #endregion

        #region Constructor
        public MainForm(string[] args)
        {
            InitializeComponent();
            Instance = this;

            splashThread = new Thread(new ThreadStart(ShowSplash));
            splashThread.Start();
            splashThread.Join(2500);

            Application.AddMessageFilter(this);
            ControlsToMove = new HashSet<Control> {
                panel_CenterTop,
                panel_RightTopExit,
            };

            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(950, 500);
            MaximumSize = new Size(ScreenResolution.Width + 100, ScreenResolution.Height + 100);
            MinimumSize = new Size(600, 400);
            Center_EditorTextBox.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

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

            slidingPanel = new KxSharpLib.Util.SlidingPanel(panel_MenuLeft, 130, 10, 10);

            logger = new KxSharpLib.Utility.Logger(Center_LoggingTextBox);
            configuration = new Configuration();

            if (configuration.UseConfigurationFile)
            {
                KxSharpLib.FormHelper.DisableControls(Settings_groupBox);
                KxSharpLib.FormHelper.HideControls(Settings_groupBox);
            }
            logger.Write("Initialized and ready!");

            KxSharpLib.Util.Win32.SwitchToThisWindow(this.Handle, true);

            // TODO: Fix this ugly temporary $hiz! 
            if (args.Length > 0) {
                if (Path.GetExtension(args[0]) == ".dat") {
                    var filepath = args[0];
                    var filename = System.IO.Path.GetFileName(filepath);
                    var filenamenoext = System.IO.Path.GetFileNameWithoutExtension(filepath);
                    logger.Write(filepath);
                    logger.Write(filename);

                    KxSharpLib.Security.Kal.Crypto.GUseCrypt = KxSharpLib.Security.Kal.Crypto.EUseCrypt.c2018;
                    KxSharpLib.Security.Kal.Crypto.GKey = (int)KxSharpLib.Security.Kal.Crypto.EKeys.E;
                    KxSharpLib.File.DAT dat = new KxSharpLib.File.DAT(0, filename, File.ReadAllBytes(filepath));
                    Center_EditorTextBox.BeginUpdate();
                    Center_EditorTextBox.Text = dat.Content;
                    Center_EditorTextBox.EndUpdate();
                    dat.Content.WriteToFile(string.Format("{0}.txt", filenamenoext));
                }
            }


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
                case (int)KxSharpLib.Util.Win32.WM.NCHITTEST:
                    base.WndProc(ref message);
                    if (SizeGripRectangle.Contains(PointToClient(new Point(message.LParam.ToInt32() & 0xffff, message.LParam.ToInt32() >> 16)))) message.Result = new IntPtr((int)KxSharpLib.Util.Win32.HT.BOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref message);
                    break;
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var NewRegion = new Region(new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height));
            SizeGripRectangle = new Rectangle(ClientRectangle.Width - ResizeHandelSize, ClientRectangle.Height - ResizeHandelSize, ResizeHandelSize, ResizeHandelSize);
            NewRegion.Exclude(SizeGripRectangle);
            panel_Main.Region = NewRegion;
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(new SolidBrush(button_RightBottomCopyright.BackColor), SizeGripRectangle);
            ControlPaint.DrawSizeGrip(e.Graphics, panel_CenterTop.BackColor, SizeGripRectangle);
        }
        // Enable Double buffering to reduce flickering.
        // TODO: Some controls get transparent on maximizing the window.. Need to be fixed!
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= KxSharpLib.Util.Win32.WS_EX_COMPOSITED;
                return cp;
            }
        }
        #endregion

        #region TreeView
        private void TreeView_PKiew_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var clickPoint = new Point(e.X, e.Y);
                var selectedNode = ((TreeView)sender).GetNodeAt(clickPoint);
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
            if (treeView_PKiew.SelectedNode == null || treeView_PKiew.SelectedNode == treeView_PKiew.TopNode)
                return;

            var SelectedIndex = treeView_PKiew.SelectedNode.Index;

            if (SelectedIndex == -1)
                return;

            if (treenViewLastSelectedIndex != -1)
                gCurrentFile.FileList[treenViewLastSelectedIndex].Content = Center_EditorTextBox.Text;

            var item = gCurrentFile.FileList[SelectedIndex];

            Center_EditorTextBox.BeginUpdate();
            Center_EditorTextBox.Text = item.Content;
            Center_EditorTextBox.EndUpdate();

            treenViewLastSelectedIndex = SelectedIndex;

            logger.Write(string.Format("[File:({0})] >> [SelectedItem:({1})]", item.Name, item.Index));
            KxSharpLib.FormHelper.SetLabelText(label_CurrentFileTopCenter, string.Format("Current File:   [{0}]", item.Name));

        }
        public void Treeviw_RightClick_SaveAs(object sender, EventArgs e)
        {
            if (treeview_RightClickMenu_ClickedNodeIndex == -1)
                return;

            using(var saveFileDia = new SaveFileDialog())
            {
                var item = gCurrentFile.FileList[treeview_RightClickMenu_ClickedNodeIndex];
                saveFileDia.FileName = item.Name.Replace(".dat", ".txt");
                var datwithoutExt = item.Name.Replace(".dat", "");
                if (saveFileDia.ShowDialog(this) == DialogResult.OK)
                {
                    item.Content.WriteToFile(saveFileDia.FileName);
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
                    foreach (var item in gCurrentFile.FileList)
                    {
                        var datAstxt = item.Name.Replace(".dat", ".txt");
                        var finalPath = Path.Combine(folderPath, datAstxt);
                        item.Content.WriteToFile(finalPath);
                        logger.Write(string.Format("Saved: {0} to {1}", datAstxt, finalPath));
                    }
                }
            }
            treeview_RightClickMenu_ClickedNodeIndex = -1;
        }

        public void Treeviw_RightClick_Add(object sender, EventArgs e)
        {
            if (treeview_RightClickMenu_ClickedNodeIndex == -1)
                return;


            treeview_RightClickMenu_ClickedNodeIndex = -1;
        }

        public void Treeviw_RightClick_Delete(object sender, EventArgs e)
        {
            if (treeview_RightClickMenu_ClickedNodeIndex == -1)
                return;


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
            KxSharpLib.Util.Win32.SetControlForeColor(button_LeftTopAppName, Color.Red);
            button_LeftTopAppName.Font = new Font("Ink Free", 16.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_LeftTopAppName_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_LeftTopAppName, Color.GreenYellow);
        }
        private void Button_LeftTopAppName_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_LeftTopAppName, SystemColors.ControlText);
        }
        private void Button_LeftTopAppName_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_LeftTopAppName, Color.GreenYellow);
            button_LeftTopAppName.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }

        private void Button_RightBottomCopyright_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightBottomCopyright, Color.SteelBlue);
        }
        private void Button_RightBottomCopyright_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightBottomCopyright, Color.Black);
        }
        private void Button_RightBottomCopyright_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightBottomCopyright, Color.White);
        }
        private void Button_RightBottomCopyright_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightBottomCopyright, Color.SteelBlue);
        }

        private void Button_MenuLeftFileOpen_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_MenuLeftFileOpen, Color.GreenYellow);
            //button_MenuLeftFileOpen.Font = new Font("Ink Free", 13.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_MenuLeftFileOpen_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_MenuLeftFileOpen, Color.SteelBlue);
            //button_MenuLeftFileOpen.Font = new Font("Ink Free", 12.0F, FontStyle.Bold);
        }
        private void Button_MenuLeftFileOpen_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_MenuLeftFileOpen, Color.Black);
            //button_MenuLeftFileOpen.Font = new Font("Ink Free", 12.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_MenuLeftFileOpen_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_MenuLeftFileOpen, Color.SteelBlue);
            // button_MenuLeftFileOpen.Font = new Font("Ink Free", 12.0F, FontStyle.Bold);
        }

        private void Button_MenuLeftSaveFile_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_MenuLeftSaveFile, Color.GreenYellow);
            // button_MenuLeftSaveFile.Font = new Font("Ink Free", 13.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_MenuLeftSaveFile_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_MenuLeftSaveFile, Color.SteelBlue);
            //button_MenuLeftSaveFile.Font = new Font("Ink Free", 12.0F, FontStyle.Bold);
        }
        private void Button_MenuLeftSaveFile_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_MenuLeftSaveFile, Color.Black);
            //button_MenuLeftSaveFile.Font = new Font("Ink Free", 12.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_MenuLeftSaveFile_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_MenuLeftSaveFile, Color.SteelBlue);
            //button_MenuLeftSaveFile.Font = new Font("Ink Free", 12.0F, FontStyle.Bold);
        }

        private void Button_RightTopExit_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightTopExit, Color.Red);
            //button_RightTopExit.Font = new Font("Ink Free", 17.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopExit_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightTopExit, Color.GreenYellow);
            //button_RightTopExit.Font = new Font("Ink Free", 16.0F, FontStyle.Bold);
        }
        private void Button_RightTopExit_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightTopExit, Color.Black);
            // button_RightTopExit.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopExit_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightTopExit, Color.GreenYellow);
            // button_RightTopExit.Font = new Font("Ink Free", 16.0F, FontStyle.Bold);
        }

        private void Button_RightTopMaximize_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightTopMaximize, Color.Red);
            //button_RightTopMaximize.Font = new Font("Ink Free", 17.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopMaximize_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightTopMaximize, Color.GreenYellow);
            //button_RightTopMaximize.Font = new Font("Ink Free", 16.0F, FontStyle.Bold);
        }
        private void Button_RightTopMaximize_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightTopMaximize, Color.Black);
            // button_RightTopMaximize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopMaximize_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightTopMaximize, Color.GreenYellow);
            // button_RightTopMaximize.Font = new Font("Ink Free", 16.0F, FontStyle.Bold);
        }

        private void Button_RightTopMinimize_MouseDown(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightTopMinimize, Color.Red);
            // button_RightTopMinimize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopMinimize_MouseEnter(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightTopMinimize, Color.GreenYellow);
            //button_RightTopMinimize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold);
        }
        private void Button_RightTopMinimize_MouseLeave(object sender, EventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightTopMinimize, Color.Black);
            //button_RightTopMinimize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold | FontStyle.Italic);
        }
        private void Button_RightTopMinimize_MouseUp(object sender, MouseEventArgs e)
        {
            KxSharpLib.Util.Win32.SetControlForeColor(button_RightTopMinimize, Color.GreenYellow);
            // button_RightTopMinimize.Font = new Font("Ink Free", 15.0F, FontStyle.Bold);
        }
        #endregion

        #region Menu Buttons
        private void Button_RightTopExit_Click(object sender, EventArgs e)
        {
            if (MsgBox.Show("Exit?", "Do you want to close?", MsgBoxIcon.QUESTION, MsgBoxButton.YESNO) == MsgBoxResult.Yes)
                Application.Exit();
        }

        private void Button_MenuLeftFileOpen_Click(object sender, EventArgs e)
        {
            if (Setting_CryptTable_comboBox.SelectedIndex == -1 || KxSharpLib.Security.Kal.Crypto.GUseCrypt == KxSharpLib.Security.Kal.Crypto.EUseCrypt.Unknown)
            {
                logger.Write("[Unknown CryptTable, Please set a valid table!]");
                return;
            }

            if (gCurrentFile != null)
            {
                logger.Write("[There is already a file loaded!]");

                switch (MsgBox.Show("Already open!", "There is already a file loaded!\nClose without saving ?", MsgBoxIcon.WARNING, MsgBoxButton.YESNO))
                {
                    case MsgBoxResult.Yes:
                        {
                            Center_EditorTextBox.BeginUpdate();
                            Center_EditorTextBox.Clear();
                            Center_EditorTextBox.EndUpdate();

                            treeView_PKiew.BeginUpdate();
                            treeView_PKiew.Nodes.Clear();
                            treeView_PKiew.EndUpdate();

                            Setting_CryptTable_comboBox.Enabled = true;
                            KxSharpLib.FormHelper.SetLabelText(label_CurrentFileTopCenter, "Current File:   [None]");
                            gCurrentFile = null;
                            GC.Collect();
                            return;
                        }

                    case MsgBoxResult.No:
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
                            using (PasswordPromt Pw_Promt = new PasswordPromt())
                            {
                                if (Pw_Promt.ShowDialog() == DialogResult.OK)
                                {
                                    gCurrentFile = new KxSharpLib.File.PkFile(filedia.FileName, Pw_Promt.Password, (byte)KxSharpLib.Security.Kal.Crypto.GKey, treeView_PKiew, logger);
                                }
                            }

                            Setting_CryptTable_comboBox.Enabled = false;
                            textBox_FileInfo_Name.Text = gCurrentFile.Name;
                            textBox_FileInfo_Path.Text = gCurrentFile.Path;
                            using (var md5 = MD5.Create())
                            {
                                textBox_FileInfo_MD5.Text = BitConverter.ToString(md5.ComputeHash(File.ReadAllBytes(gCurrentFile.Path))).Replace("-", "").ToLower();
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
            if(treeView_PKiew.SelectedNode != null)
                gCurrentFile.FileList[treeView_PKiew.SelectedNode.Index].Content = Center_EditorTextBox.Text;
            if(gCurrentFile != null)
                gCurrentFile.Save();
        }

        private void Button_TopLeftLogo_Click(object sender, EventArgs e)
        {
            slidingPanel.Start();
        }
        #endregion


    }
}