using Newtonsoft.Json;

namespace Uploadarr.Common
{
    public static class ObjectExtensions
    {
        public static T JsonClone<T>(this T source) where T : new()
        {
            var json = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}