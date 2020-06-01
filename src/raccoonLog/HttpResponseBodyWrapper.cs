using System;
using System.IO;

namespace raccoonLog
{
    public class HttpResponseBodyWrapper : IDisposable
    {
        private Stream _innerStream;

        public Stream Body
        {
            get { _innerStream.Position = 0; return _innerStream; }
        }

        public HttpResponseBodyWrapper(Stream body)
        {
            _innerStream = body;
        }

        public void Dispose()
        {
            _innerStream?.Dispose();
        }
    }
}
