using Carter;

namespace Uploadarr.API
{
    public abstract class ApiBaseModule : CarterModule
    {
        protected ApiBaseModule(string resource)
            : base("/api/" + resource.Trim('/'))
        {
        }
    }
}