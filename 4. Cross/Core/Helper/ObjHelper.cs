using Core.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Core.Helper
{
    public static class ObjHelper
    {
        public static string ToJsonString(this object obj)
        {
            JObject jobject = obj as JObject;
            if (jobject != null)
                return jobject.ToString();
            return JsonConvert.SerializeObject(obj, JsonSetting.JsonSerializerSettings);
        }
    }
}