using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using Websocket.Client;

namespace CycleTLS.Helpers
{
    public static class WebsocketExtensions
    {
        private static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static void SendJson<T>(this IWebsocketClient WS, T obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            WS.Send(json);
        }
        public static async Task SendInstantJson<T>(this IWebsocketClient WS, T obj)
        {
            string json = JsonConvert.SerializeObject(obj, SerializerSettings);
            await WS.SendInstant(json);
        }
    }
}
