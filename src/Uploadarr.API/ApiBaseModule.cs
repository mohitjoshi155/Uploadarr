using System;
using Carter;
using Microsoft.AspNetCore.Http;

namespace Uploadarr.API
{
    public abstract class ApiBaseModule : CarterModule
    {
        protected ApiBaseModule(string resource)
            : base("/api/" + resource.Trim('/'))
        {
        }

        public static T GetQueryValue<T>(HttpRequest req, string key)
        {
            if (req.Query.ContainsKey(key))
            {

                if (typeof(T) == typeof(bool))
                {
                    return (T)Convert.ChangeType(bool.Parse(req.Query[key]), typeof(T));
                }

                if (typeof(T) == typeof(int))
                {
                    return (T)Convert.ChangeType(int.Parse(req.Query[key].ToString()) , typeof(T));
                }     
                
                if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(req.Query[key].ToString(), typeof(T));
                }

            }
            return default(T);
        }
    }
}