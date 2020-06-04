using System.IO;

namespace raccoonLog.Stores.File
{
    public class FileSystem : IFileSystem
    {
        public FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return System.IO.File.Open(path, mode, access, share);
        }
    }
}