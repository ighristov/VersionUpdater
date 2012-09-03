using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VersionUpdaterSrv
{
    public class VersionedFile
    {
        public string ApplicationName { get; set; }
        public string Group { get; set; }
        public string FileName { get; set; }
        public Version Version { get; set; }
        public string CheckSum { get; set; }
        public DateTime DateTime { get; set; }
    }
}