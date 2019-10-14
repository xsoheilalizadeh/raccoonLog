using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using raccoonLog.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace raccoonLog.Tests.Handlers
{
    public class DefaultHttpLogFormHandlerTests
    {
        [Fact]
        public async Task HandleThrowsNullRefreceExceptionOnNullRequest()
        {
            // arrange
            var handler = new DefaultHttpLogFormHandler();

            // act and assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null, null));
        }

        [Fact]
        public async Task HandleThrowsNullRefreceExceptionOnNullLogMessage()
        {
            // arrange
            var context = new DefaultHttpContext();
            var handler = new DefaultHttpLogFormHandler();

            // act and assert
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(context.Request, null));
        }

        [Theory, MemberData(nameof(FormContentRequest))]
        public async Task HandleInitializeFormLog(HttpRequest request)
        {
            // arrange
            var logMessage = new HttpRequestLog();
            var handler = new DefaultHttpLogFormHandler();

            // act
            await handler.Handle(request, logMessage);

            // assert
            var body = Assert.IsType<FormLog>(logMessage.Body);

            Assert.NotNull(body.Form);
            Assert.NotEmpty(body.Form);

            Assert.NotNull(body.Files);
            Assert.NotEmpty(body.Files);

            Assert.Equal(request.Form.Count, body.Form.Count);
            Assert.Equal(request.Form.Files.Count, body.Files.Count);

            foreach (var item in body.Form)
            {
                Assert.Equal(item.Value, request.Form[item.Key]);
            }

            foreach (var item in body.Files)
            {
                var file = request.Form.Files[item.Name];
                Assert.Equal(item.Name, file.Name);
                Assert.Equal(item.FileName, file.FileName);
                Assert.Equal(item.ContentLength, file.Length);
                Assert.Equal(item.ContentType, file.ContentType);
                Assert.Equal(item.ContentDisposition, file.ContentDisposition);
            }
        }

        public static IEnumerable<object[]> FormContentRequest
        {
            get
            {
                var context = new DefaultHttpContext();
                yield return new object[]
                {
                    new DefaultHttpRequest(context)
                    {
                        ContentType = "application/x-www-form-urlencoded",
                        Form = new FormCollection(new Dictionary<string, StringValues>
                        {
                            {"Name", "Soheil" },
                            {"Age", "20" }
                        },
                        new FormFileCollection
                        {
                            new FormFile(Stream.Null,0,100,"image","photo.png")
                            {
                               Headers = new HeaderDictionary
                               {
                                    {HeaderNames.ContentType, MediaTypeNames.Text.Plain},
                                    {HeaderNames.ContentDisposition, $"attachment; filename=\"{Guid.NewGuid():N}.text\""}
                               }
                            },
                            new FormFile(Stream.Null,0,100,"video","video.mp4")
                            {
                                Headers = new HeaderDictionary
                                {
                                    {HeaderNames.ContentType, MediaTypeNames.Text.Plain},
                                    {HeaderNames.ContentDisposition, $"attachment; filename=\"{Guid.NewGuid():N}.text\""}
                                }
                            },
                        }),
                    }
                };
            }
        }
    }
}
