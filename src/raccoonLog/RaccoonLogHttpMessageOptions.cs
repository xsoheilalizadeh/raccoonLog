using System.Collections.Generic;

namespace raccoonLog
{
    public abstract class RaccoonLogHttpMessageOptions
    {
        public HashSet<string> IgnoreHeaders { get; set; } = new HashSet<string>();

        public HashSet<string> IgnoreContentTypes { get; set; } = new HashSet<string>();
    }
}