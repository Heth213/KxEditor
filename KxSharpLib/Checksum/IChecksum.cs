using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KxSharpLib.Checksum 
{
    public interface IChecksum 
    {
        long Value { get; }
        void Reset();
        void Update(int bval);
        void Update(byte[] buffer);
        void Update(ArraySegment<byte> segment);
    }
}
