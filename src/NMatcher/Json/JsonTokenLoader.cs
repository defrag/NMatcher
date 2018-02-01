using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace NMatcher.Json
{
    public static class JsonTokenLoader
    {
        public static JToken LoadJson(string json)
        {
            JsonReader reader = new JsonTextReader(new StringReader(json));
            reader.DateParseHandling = DateParseHandling.None;

            return JToken.Load(reader);
        }
    }
}
