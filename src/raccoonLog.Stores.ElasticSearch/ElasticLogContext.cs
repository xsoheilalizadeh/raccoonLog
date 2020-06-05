namespace raccoonLog.Stores.ElasticSearch
{
    public class ElasticLogContext : LogContext
    {
        public ElasticLogContext(LogContext context) : base(context.TraceId, context.Request, context.Response,
            context.Protocol, context.Timestamp, context.Error)
        {
            // Wrapping the bodies to make the stored object consistent.
            context.Request.Body = new {Data = context.Request.Body};
            context.Response.Body = new {Data = context.Response.Body};
        }
    }
}