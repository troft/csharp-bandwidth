using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net.Data
{
    public class Media
    {
        public int ContentLength { get; set; }
        public string MediaName { get; set; }
        public Uri Content { get; set; }
    }

    public class MediaContent
    {
        public string MediaType { get; set; }
        public Stream Stream { get; set; }
        public byte[] Buffer { get; set; }
    }
}
