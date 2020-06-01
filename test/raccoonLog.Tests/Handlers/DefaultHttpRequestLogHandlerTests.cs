// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Http.Features;
// using Microsoft.Extensions.DependencyInjection;
// using Moq;
// using raccoonLog.Http;
// using raccoonLog.Http.Handlers;
// using Xunit;

// namespace raccoonLog.Tests.Handlers
// {
//     public class DefaultHttpRequestLogHandlerTests
//     {
//         private Mock<IHttpLogMessageFactory> _logMessageFactory;

//         private Mock<IHttpRequestLogFormHandler> _formContentHandler;

//         public DefaultHttpRequestLogHandlerTests()
//         {
//             _formContentHandler = new Mock<IHttpRequestLogFormHandler>();
//         }


//         [Fact]
//         public async Task HanleThrowsNullReferenceExceptionOnNullRequest()
//         {
//             // arrange
//             var handler = CreateHandler();

//             // act and assert
//             await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null).AsTask());
//         }

//         [Fact]
//         public async Task HandleThrowsNullReferenceExceptionOnNullLogMessage()
//         {
//             // arrange
//             var context = new DefaultHttpContext();
//             var handler = CreateHandler();

//             // act and assert
//             await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(context.Request).AsTask());
//         }

//         [Fact]
//         public async Task HandleInvokeFormContentHandlerOnFormContentRequest()
//         {
//             // arrange
//             var handler = CreateHandler();
//             var logMessage = new HttpRequestLog();
//             var context = new DefaultHttpContext
//             {
//                 Request =
//                 {
//                     Scheme = "http",
//                     Host = new HostString("ex.com"),
//                     ContentType = "application/x-www-form-urlencoded"
//                 }
//             };

//             _logMessageFactory.Setup(s => s.Create<HttpRequestLog>(CancellationToken.None))
//             .ReturnsAsync(logMessage);

//             // act 
//             await handler.Handle(context.Request);

//             // assert (verify) 
//             _formContentHandler.Verify(s => s.Handle(context.Request, logMessage,CancellationToken.None), Times.Once);

//             _bodyHandler.Verify(s => s.Handle(context.Request.Body, logMessage,CancellationToken.None), Times.Never);
//         }


//         [Fact]
//         public async Task HandleInvokeBodyHandlerOnNormalRequest()
//         {
//             // arrange
//             var handler = CreateHandler();
//             var logMessage = new HttpRequestLog();
//             var context = new DefaultHttpContext
//             {
//                 Request =
//                 {
//                     Scheme = "http",
//                     Host = new HostString("http://ex.com"),
//                     ContentType = "application/json"
//                 }
//             };

//             _logMessageFactory.Setup(s => s.Create<HttpRequestLog>(CancellationToken.None))
//             .ReturnsAsync(logMessage);

//             // act 
//             await handler.Handle(context.Request);

//             // assert (verify) 
//             _bodyHandler.Verify(s => s.Handle(context.Request.Body, logMessage,CancellationToken.None), Times.Once);

//             _formContentHandler.Verify(s => s.Handle(context.Request, logMessage,CancellationToken.None), Times.Never);
//         }


//         [Fact]
//         public async Task HandleSetsRequestInformationToLogMessage()
//         {
//             // arrange
//             var context = new DefaultHttpContext();
//             var request = context.Request;
//             var handler = new ServiceCollection()
//                 .SetHttpContext(context)
//                 .AddHttpLogging()
//                 .BuildServiceProvider()
//                 .GetService<IHttpRequestLogHandler>();

//             context.Features.Set<IHttpRequestFeature>(new RequestFeatureStub());

//             context.Features.Set<IRequestCookiesFeature>(new RequestCookiesFeatureStub());

//             // act 
//             var logMessage = await handler.Handle(request);

//             Assert.Equal(logMessage.Cookies.Count, request.Cookies.Count);
//             Assert.Equal(logMessage.Parameters.Count, request.Query.Count);
//             Assert.Equal(logMessage.Url.Path, request.Path);
//             Assert.Equal(logMessage.Url.Scheme, request.Scheme);
//             Assert.Equal(logMessage.Url.Protocol, request.Protocol);
//             Assert.Equal(logMessage.Url.Host, request.Host.ToString());
//         }


//         private DefaultHttpRequestLogHandler CreateHandler()
//         {
//             return new DefaultHttpRequestLogHandler(
//                 _logMessageFactory.Object,
//                 _formContentHandler.Object,
//                 _bodyHandler.Object
//             );
//         }
//     }
// }
