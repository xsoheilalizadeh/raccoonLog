using System.IO;

namespace raccoonLog.Http.Stores
{
    public interface IFileSystem
    {
        FileStream Open(string path, FileMode mode, FileAccess access, FileShare share);
    }

    public class FileSystem : IFileSystem
    {
        public FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return File.Open(path, mode, access, share);
        }
    }
}
