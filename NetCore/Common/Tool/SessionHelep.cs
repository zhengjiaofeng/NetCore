using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Common.Tool
{
    public static class SessionHelep
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            try
            {
                session.SetString(key, JsonConvert.SerializeObject(value));
            }
            catch (Exception ex)
            {
                    
            }
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
