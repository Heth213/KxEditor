using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KxSharpLib.File
{
    public interface IFile
    {
        void Load();
        void Save();
    }

    public interface IFileManager
    {
        void Load(string filePath);
        void Save(string filePath);
    }
}
