namespace raccoonLog.Http
{
    public class AgentLog
    {
        public AgentLog(OsLog os, UserAgentLog userAgent, string pureUserAgent)
        {
            Os = os;
            UserAgent = userAgent;
            PureUserAgent = pureUserAgent;
        }

        public OsLog Os { get; set; }

        public UserAgentLog UserAgent { get; set; }

        public string PureUserAgent { get; set; }
    }

    public class AgentInfo
    {
        public AgentInfo(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public string Name { get; set; }

        public string Version { get; set; }
    }

    public class OsLog : AgentInfo
    {
        public OsLog(string name, string version) : base(name, version)
        {
        }
    }

    public class UserAgentLog : AgentInfo
    {
        public UserAgentLog(string name, string version) : base(name, version)
        {
        }
    }
}
