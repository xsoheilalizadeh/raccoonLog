using System.IO;

namespace raccoonLog.Stores.File
{
    public interface IFileSystem
    {
        FileStream Open(string path, FileMode mode, FileAccess access, FileShare share);
    }
}