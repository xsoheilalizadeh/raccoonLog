namespace raccoonLog
{
    public class ExceptionLog
    {
        public ExceptionLog(string message, ExceptionLog innerError, string? stackTrace, string? source,
            string? helpLink)
        {
            Message = message;
            InnerError = innerError;
            StackTrace = stackTrace;
            Source = source;
            HelpLink = helpLink;
        }

        public string Message { get; }

        public ExceptionLog InnerError { get; private set; }

        public string? StackTrace { get; set; }

        public string? Source { get; set; }

        public string? HelpLink { get; set; }

        public void SetInnerError(ExceptionLog error) => InnerError = error;
    }
}