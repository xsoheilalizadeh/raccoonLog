namespace raccoonLog.Http
{
    public class AgentLog
    {
        public AgentLog(string os, string device, string userAgent)
        {
            Os = os;
            Device = device;
            UserAgent = userAgent;
        }

        public string Os { get; set; }

        public string Device { get; set; }

        public string UserAgent { get; set; }
    }
}
