using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ямборко_КП
{
    class TBException : Exception
    {
        public TBException(string message)
            : base(message)
        { }
    }

    class MatrException : Exception
    {
        public MatrException(string message)
            : base(message)
        { }
    }

    class FileException : Exception
    {
        public FileException(string message)
            : base(message)
        { }
    }
}
