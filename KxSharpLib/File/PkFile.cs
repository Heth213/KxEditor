using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;

namespace KxSharpLib.File
{
    public class PkFile : IFile
    {
        private string m_name;
        public string Name 
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private string m_path;
        public string Path 
        {
            get { return m_path; }
            set { m_path = value; }
        }

        private string m_password;
        public string Password 
        {
            get { return m_password; }
            set { m_password = value; }
        }

        private byte m_key;
        public byte Key 
        {
            get { return m_key; }
            set { m_key = value; }
        }

        private List<DAT> m_fileList;
        public List<DAT> FileList 
        {
            get { return m_fileList; }
            set { m_fileList = value; }
        }

        private readonly TreeView _tview;
        private readonly Utility.Logger _logger;

        public PkFile() => new PkFile("", "", 0, null, null);
        public PkFile(string path, string pw, byte key, TreeView tview, Utility.Logger logger) 
        {
            m_fileList = new List<DAT>();
            m_name = System.IO.Path.GetFileName(path);
            m_password = pw;
            m_path = path;
            m_key = key;
            _tview = tview;
            _logger = logger;
            Load();
        }

        public void Load() 
        {
            using (ZipInputStream zipStream = new ZipInputStream(System.IO.File.OpenRead(m_path))) 
            {
                zipStream.Password = m_password;
                try 
                {
                    m_fileList.Clear();
                    _tview.BeginUpdate();
                    var node = _tview.Nodes.Add(m_name);
                    var entries = GetEntries(zipStream, node);
                    node.ExpandAll();
                    _tview.EndUpdate();

                    using (BackgroundWorker bWorker = new BackgroundWorker()) 
                    {
                        bWorker.DoWork += new DoWorkEventHandler(delegate (object sender, DoWorkEventArgs e) 
                        {
                            m_fileList.AddRange(entries.ToArray());
                        });
                        bWorker.RunWorkerAsync();
                    }

                } 
                catch (Exception ex) 
                {
                    _logger.Write(string.Format("{0}", ex.Message));
                    _tview.Nodes.Clear();
                    return;
                }
            }
        }

        public void Save() 
        {
            if (m_fileList == null) 
            {
                _logger.Write(string.Format("[FileList not initialized! Load a file to be able to save!]"));
                return;
            }

            if (m_fileList.Count <= 0) 
            {
                _logger.Write(string.Format("[FileList is empty! Load a file to be able to save!]"));
                return;
            }

            System.IO.FileMode filemode = System.IO.FileMode.CreateNew;
            if (System.IO.File.Exists(m_path)) 
            {
                filemode = System.IO.FileMode.Truncate;
            }

            using (ZipOutputStream zipStream = new ZipOutputStream(System.IO.File.Open(m_path, filemode))) 
            {
                zipStream.SetLevel(5);
                zipStream.Password = m_password;

                foreach(var item in m_fileList) 
                {
                    ZipEntry entry = new ZipEntry(item.Name) {
                        DateTime = DateTime.Now
                    };

                    var itemData = item.Encrypt();

                    zipStream.PutNextEntry(entry);
                    zipStream.Write(itemData, 0, itemData.Length);
                }
                zipStream.Finish();
                zipStream.Close();
            }
            _logger.Write(string.Format("[({0}) has been saved!\n\tPath: ({1})\n\tPassword: ({2})]", m_name, m_path, m_password));
        }

        private List<DAT> GetEntries(ZipInputStream zipStream, TreeNode node) {
            var entryIndex = 0;
            var output = new List<DAT>();

            for (var zipEntry = zipStream.GetNextEntry(); zipEntry != null; zipEntry = zipStream.GetNextEntry())
            {
                var buffer = new byte[4096];
                var entryData = new List<byte>();

                while (true)
                {
                    var len = zipStream.Read(buffer, 0, buffer.Length);
                    if (len <= 0)
                        break;

                    for (int i = 0; i < len; i++)
                        entryData.Add(buffer[i]);
                }

                output.Add(new DAT(entryIndex++, zipEntry.Name, entryData.ToArray()));
                node.Nodes.Add(zipEntry.Name);
            }
            return output;
        }
    }
}