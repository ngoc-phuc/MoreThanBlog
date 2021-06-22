using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Constants
{
    public class JsonSetting
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Newtonsoft.Json.Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK",
            ContractResolver = (IContractResolver)new CamelCasePropertyNamesContractResolver()
        };
    }
}