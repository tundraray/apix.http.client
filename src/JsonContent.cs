using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Apix.Http.Client
{
    internal class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        { }
    }

    internal class JsonContent<T> : StringContent
    {
        public JsonContent(T obj) :
            base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        { }
    }
}
