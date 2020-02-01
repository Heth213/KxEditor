using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KxSharpLib.File
{
    class FileManager : IFileManager
    {
        List<PkFile> m_fileList;
        List<PkFile> FileList
        {
            get { return m_fileList; }
            set { m_fileList = value; }
        }
        FileManager()
        {
            m_fileList = new List<PkFile>();
        }

        public void Load(string filePath)
        {
            throw new NotImplementedException();
        }

        public void Save(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
