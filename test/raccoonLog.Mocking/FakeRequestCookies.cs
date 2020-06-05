using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;

namespace raccoonLog.Mocking
{
    public class FakeRequestCookies : IRequestCookiesFeature
    {
        public FakeRequestCookies()
        {
            Cookies = new RequestCookieCollection(new Dictionary<string, string>
            {
                {"auth_token", Guid.NewGuid().ToString() }
            });
        }

        public IRequestCookieCollection Cookies { get; set; }
    }
}
