using System;
using Nest;

namespace raccoonLog.Stores.ElasticSearch
{
    public class ElasticLogContext : LogContext
    {
        public ElasticLogContext(LogContext context) : base(context.TraceId, context.Request, context.Response, context.Protocol)
        {
            Timestamp = DateTime.Now;
        }

        [Date(Name = "@timestamp")]
        public DateTime Timestamp { get; private set; }
    }
}