﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Options;
using Moq;
using raccoonLog.Handlers;
using raccoonLog.Mocking;
using Xunit;

namespace raccoonLog.UnitTests
{
    public class HttpLogMessageBodyTests
    {
        private readonly Mock<IOptions<RaccoonLogHttpOptions>> options = new Mock<IOptions<RaccoonLogHttpOptions>>();

        [Fact]
        public async Task HttpLogRequestMapsFormBody()
        {
            options.Setup(o => o.Value).Returns(new RaccoonLogHttpOptions());

            var handler = new DefaultHttpRequestLogFormHandler(options.Object, NullProtector.Value);
            var context = new DefaultHttpContext();

            context.Features.Set(FakeForm.Value);

            var requestLog = new HttpRequestLog(UrlLog.Default, null!, null, null!, null!);

            await handler.Handle(context.Request, requestLog);

            var body = requestLog.Body as FormLog;

            Assert.Equal(body.Form, context.Request.Form);

            foreach (var item in body.Files)
                Assert.Equal(item, (FormFile) context.Request.Form.Files.GetFile(item.Name));
        }

        [Fact]
        public async Task HttpLogRequestProtectsMapsFormBody()
        {
            var option = new RaccoonLogHttpOptions();

            option.Request.SensitiveData.Forms.Add("age");

            options.Setup(o => o.Value).Returns(option);

            var handler = new DefaultHttpRequestLogFormHandler(options.Object, NullProtector.Value);
            var context = new DefaultHttpContext();

            context.Features.Set(FakeForm.Value);

            var requestLog = new HttpRequestLog(UrlLog.Default, null, null, null, null);

            await handler.Handle(context.Request, requestLog);

            var body = requestLog.Body as FormLog;

            Assert.NotEqual(body.Form.First(f => f.Key == "age").Value, context.Request.Form["age"]);
        }
    }
}