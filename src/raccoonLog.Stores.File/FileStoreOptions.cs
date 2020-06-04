namespace raccoonLog.Stores.File
{
    public class FileStoreOptions
    {
        public string SavePath { get; set; } = "logs/http";

        public string FileName { get; set; } = "log.json";
    }
}
