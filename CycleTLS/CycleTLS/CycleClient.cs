using CycleTLS.Helpers;
using CycleTLS.Interfaces;
using CycleTLS.Models;
using Newtonsoft.Json;
using Websocket.Client;

namespace CycleTLS
{
    public class CycleClient : ICycleClient
    {
        private IWebsocketClient CycleWS;
        private TaskCompletionSource<string> WSResponseTask;

        public CycleClient(Uri cycleWS)
        {
            CycleWS = new WebsocketClient(cycleWS);
            CycleWS.MessageReceived.Subscribe(msg =>
            {
                WSResponseTask?.TrySetResult(msg.Text);
            });

            CycleWS.StartOrFail().Wait();
        }

        public async Task<CycleResponse> SendAsync(CycleRequestOptions options)
        {
            WSResponseTask = new();

            CycleRequest request = new();
            request.RequestId = $"{DateTime.Now}:{options.Url}";
            request.Options = options;

            await CycleWS.SendInstantJson(request);

            var response = await WSResponseTask.Task;
            return JsonConvert.DeserializeObject<CycleResponse>(response);
        }

        public void Dispose()
        {
            if (CycleWS != null)
            {
                CycleWS.Dispose();
                CycleWS = null;
            }
        }
    }
}