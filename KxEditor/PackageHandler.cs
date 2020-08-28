using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;

namespace KxEditor
{
    public class PackageHandler
    {
        private static KxSharpLib.Utility.Logger MainLogger => MainForm.Instance.logger;
        private readonly TreeNode node;
        public PackageHandler(string filePath) {
            MainForm.Instance.LoadedPK.Name = Path.GetFileName(filePath);
            MainForm.Instance.LoadedPK.Path = filePath;

            using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(MainForm.Instance.LoadedPK.Path)))
            {
                using (PasswordPromt Pw_Promt = new PasswordPromt())
                {
                    if (Pw_Promt.ShowDialog() == DialogResult.OK)
                    {
                        zipStream.Password = MainForm.Instance.LoadedPK.Password = Pw_Promt.Password;

                        List<KxSharpLib.Kal.DAT> entries;
                        try
                        {
                            node = MainForm.Instance.treeView_PKiew.Nodes.Add(Path.GetFileName(MainForm.Instance.LoadedPK.Path));
                            entries = GetEntries(zipStream);
                        }
                        catch (Exception)
                        {
                            MainLogger.Write("Wrong password! Please reopen the file with the correct password");
                            zipStream.Close();
                            MainForm.Instance.treeView_PKiew.Nodes.Clear();
                            return;
                        }
                        zipStream.Close();

                        MainForm.Instance.DATList = new List<KxSharpLib.Kal.DAT>();

                        using (BackgroundWorker bw = new BackgroundWorker())
                        {
                            bw.DoWork += new DoWorkEventHandler(delegate (object sender, DoWorkEventArgs e) 
                            {
                                MainForm.Instance.DATList.AddRange(entries.ToArray());
                            });
                            bw.RunWorkerAsync();
                        }

                    }
                }
            }
        }




        private List<KxSharpLib.Kal.DAT> GetEntries(ZipInputStream zipStream) {
            List<KxSharpLib.Kal.DAT> files = new List<KxSharpLib.Kal.DAT>();
            ZipEntry zipEntry;
            int idx = 0;
            while ((zipEntry = zipStream.GetNextEntry()) != null) {
                byte[] buffer = new byte[2048];
                List<byte> input = new List<byte>();
                while (true) {
                    int size = zipStream.Read(buffer, 0, buffer.Length);
                    if (size <= 0)
                        break;

                    for (int i = 0; i < size; i++)
                        input.Add(buffer[i]);
                }
                node.Nodes.Add(zipEntry.Name);
                files.Add(new KxSharpLib.Kal.DAT(idx, zipEntry.Name, input.ToArray()));
                idx++;
            }
            node.ExpandAll();
            return files;
        }
        public static void Save() {
            if(MainForm.Instance.DATList == null) {
                MainLogger.Write(string.Format("[DatList not initialized! Load a file to be able to save!]"));
                return;
            }
            if(MainForm.Instance.DATList.Count <= 0) {
                MainLogger.Write(string.Format("[DatList is empty! Load a file to be able to save!]"));
                return;
            }
            FileMode filemode = FileMode.CreateNew;
            if (File.Exists(MainForm.Instance.LoadedPK.Path)) {
                filemode = FileMode.Truncate;
            }
            ZipOutputStream zipOutStream = new ZipOutputStream(File.Open(MainForm.Instance.LoadedPK.Path, filemode));

            zipOutStream.SetLevel(5);
            zipOutStream.Password = MainForm.Instance.LoadedPK.Password;

            MainForm.Instance.DATList[MainForm.Instance.treeView_PKiew.SelectedNode.Index].content = MainForm.Instance.Center_EditorTextBox.Text;

            ZipEntry entry;
            foreach (KxSharpLib.Kal.DAT dat in MainForm.Instance.DATList) {
                entry = new ZipEntry(dat.ToString()) {
                    DateTime = DateTime.Now
                };

                byte[] buffer = KxSharpLib.Security.Kal.Crypto.EncryptDat(dat.content);

                zipOutStream.PutNextEntry(entry);
                zipOutStream.Write(buffer, 0, buffer.Length);
            }
            zipOutStream.Finish();
            zipOutStream.Close();

            MainLogger.Write(string.Format("[({0}) has been saved!\n\tPath: ({1})\n\tPassword: ({2})]", MainForm.Instance.LoadedPK.Name, MainForm.Instance.LoadedPK.Path, MainForm.Instance.LoadedPK.Password));

        }
    }
}
