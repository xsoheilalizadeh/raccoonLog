using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace raccoonLog.UnitTests
{
    public class FakeForm : IFormFeature
    {
        public static IFormFeature Value = new FakeForm();

        public FakeForm()
        {
            Form = new FormCollection(new Dictionary<string, StringValues>
                    {
                        {"Name", "Soheil"},
                        {"age", "20"}
                    },
                   new FormFileCollection
                   {
                            new FormFile(Stream.Null, 0, 100, "document", "doc.txt")
                            {
                            Headers = new HeaderDictionary
                            {
                                {HeaderNames.ContentType, MediaTypeNames.Text.Plain},
                                {HeaderNames.ContentDisposition, $"attachment; filename=\"{Guid.NewGuid():N}.text\""}
                            }
                        },
                        new FormFile(Stream.Null, 0, 100, "video", "video.mp4")
                        {
                            Headers = new HeaderDictionary
                            {
                                {HeaderNames.ContentType, MediaTypeNames.Text.Plain},
                                {HeaderNames.ContentDisposition, $"attachment; filename=\"{Guid.NewGuid():N}.text\""}
                            }
                        },
                   });
        }

        public bool HasFormContentType => true;

        public IFormCollection Form { get; set; }

        public IFormCollection ReadForm() => Form;

        public Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken) => Task.FromResult(Form);
    }
}
