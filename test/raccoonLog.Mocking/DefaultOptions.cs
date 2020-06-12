﻿using Microsoft.Extensions.Options;

namespace raccoonLog.Mocking
{
    public class DefaultOptions : IOptions<RaccoonLogHttpOptions>
    {
        public static IOptions<RaccoonLogHttpOptions> Default => new DefaultOptions();

        public RaccoonLogHttpOptions Value => new RaccoonLogHttpOptions();
    }
}