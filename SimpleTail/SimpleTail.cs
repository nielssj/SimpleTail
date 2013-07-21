using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTail
{
    class SimpleTail
    {
        private FileArguments[] fileArgs;

        public SimpleTail(FileArguments[] fileList)
        {
            this.fileArgs = fileList;
        }
    }
}
