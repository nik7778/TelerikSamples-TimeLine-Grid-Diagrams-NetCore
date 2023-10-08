using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.ForensicFileTimeline.Models
{
    public class FileModel
    {
        public string Item { get; set; }
        public string Path { get; set; }
        public string Category { get; set; }
        public string PSize { get; set; }
        public string LSize { get; set; }
        public string MD5 { get; set; }
        public string Folder { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }

        public DateTime Created { get; set; }
        public DateTime Accessed { get; set; }
        public DateTime Modified { get; set; }
    }

    public class FileCsv
    {
        public string Item { get; set; }
        public string Path { get; set; }
        public string Category { get; set; }
        public string PSize { get; set; }
        public string LSize { get; set; }
        public string MD5 { get; set; }
        public string Created { get; set; }
        public string Accessed { get; set; }
        public string Modified { get; set; }
    }
}
