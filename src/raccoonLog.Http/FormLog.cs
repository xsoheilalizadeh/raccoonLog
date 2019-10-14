using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace raccoonLog.Http
{
    public class FormLog
    {
        public FormLog()
        {
            Form = new Dictionary<string, string>();
            Files = new List<FileLog>();
        }

        public Dictionary<string, string> Form { get; set; }

        public List<FileLog> Files { get; set; }
    }

    public class FileLog
    {
        public FileLog(IFormFile file)
        {
            Name = file.Name;
            FileName = file.FileName;
            ContentLength = file.Length;
            ContentType = file.ContentType;
            ContentDisposition = file.ContentDisposition;
        }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long ContentLength { get; set; }

        public string ContentDisposition { get; set; }
    }
}
