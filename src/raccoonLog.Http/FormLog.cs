using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace raccoonLog.Http
{
    public class FormLog
    {
        public FormLog(List<KeyValuePair<string, string>> form, List<FileLog> files)
        {
            Form = form;
            Files = files;
        }

        public List<KeyValuePair<string, string>> Form { get; private set; }

        public IReadOnlyList<FileLog> Files { get; private set; }
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

        public string Name { get; private set; }

        public string FileName { get; private set; }

        public string ContentType { get; private set; }

        public long ContentLength { get; private set; }

        public string ContentDisposition { get; private set; }
    }
}
