//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Features;
//using raccoonLog.Http;
//using System;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace raccoonLog.Tests.Handlers
//{
//    public partial class DefaultHttpLogFormHandlerTests
//    {
//        [Fact]
//        public async Task HandleThrowsNullRefreceExceptionOnNullRequest()
//        {
//            // arrange
//            var handler = new DefaultHttpRequestLogFormHandler();

//            // act and assert
//            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null, null));
//        }

//        [Fact]
//        public async Task HandleThrowsNullRefreceExceptionOnNullLogMessage()
//        {
//            // arrange
//            var context = new DefaultHttpContext();
//            var handler = new DefaultHttpRequestLogFormHandler();

//            // act and assert
//            await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(context.Request, null));
//        }

//        [Fact]
//        public async Task HandleInitializeFormLog()
//        {
//            // arrange
//            var context = new DefaultHttpContext();
//            var request = context.Request;
//            var logMessage = new HttpRequestLog();
//            var handler = new DefaultHttpRequestLogFormHandler();

//            context.Features.Set<IFormFeature>(new FormContentRequestFeatureStub());

//            // act
//            await handler.Handle(request, logMessage);

//            // assert
//            var body = Assert.IsType<FormLog>(logMessage.Body);

//            Assert.NotNull(body.Form);
//            Assert.NotEmpty(body.Form);

//            Assert.NotNull(body.Files);
//            Assert.NotEmpty(body.Files);

//            Assert.Equal(request.Form.Count, body.Form.Count);
//            Assert.Equal(request.Form.Files.Count, body.Files.Count);

//            foreach (var item in body.Form)
//            {
//                Assert.Equal(item.Value, request.Form[item.Key]);
//            }

//            foreach (var item in body.Files)
//            {
//                var file = request.Form.Files[item.Name];
//                Assert.Equal(item.Name, file.Name);
//                Assert.Equal(item.FileName, file.FileName);
//                Assert.Equal(item.ContentLength, file.Length);
//                Assert.Equal(item.ContentType, file.ContentType);
//                Assert.Equal(item.ContentDisposition, file.ContentDisposition);
//            }
//        }
//    }
//}