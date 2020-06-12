using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;

namespace raccoonLog
{
    public class FormLog
    {
        public FormLog(List<KeyValuePair<string, StringValues>> form, List<FileLog> files)
        {
            Form = form;
            Files = files;
        }

        public List<KeyValuePair<string, StringValues>> Form { get; }

        public IReadOnlyList<FileLog> Files { get; }
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

        public string Name { get; }

        public string FileName { get; }

        public string ContentType { get; }

        public long ContentLength { get; }

        public string ContentDisposition { get; }

        public static implicit operator FileLog(FormFile file)
        {
            return new FileLog(file);
        }

        public override bool Equals(object? obj)
        {
            var fileLog = obj as FileLog;

            if (ReferenceEquals(fileLog, null))
                return false;

            if (GetType() != obj?.GetType())
                return false;

            return Name == fileLog.Name &&
                   FileName == fileLog.FileName &&
                   ContentType == fileLog.ContentType &&
                   ContentLength == fileLog.ContentLength &&
                   ContentDisposition == fileLog.ContentDisposition;
        }


        public override int GetHashCode()
        {
            return (Name, FileName, ContentType, ContentLength, ContentDisposition).GetHashCode();
        }

        public static bool operator ==(FileLog a, FileLog b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(FileLog a, FileLog b)
        {
            return !(a == b);
        }
    }
}