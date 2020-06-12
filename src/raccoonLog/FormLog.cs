using System.Collections.Generic;
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
}