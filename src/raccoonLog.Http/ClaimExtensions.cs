using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace raccoonLog.Http
{
    public static class ClaimExtensions
    {
        public static Dictionary<string, string> ToDictionary(this IEnumerable<Claim> claims)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var item in claims)
            {
                if (!dictionary.ContainsKey(item.Type))
                {
                    string value;

                    if (claims.Count(x => x.Type == item.Type) > 1)
                    {
                        var values = claims.Where(x => x.Type == item.Type).Select(x => x.Value).ToArray();

                        value = new StringValues(values);
                    }
                    else
                    {
                        value = item.Value;
                    }

                    dictionary.Add(item.Type, value);
                }
            }

            return dictionary;
        }
    }
}
