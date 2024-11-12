using CycleTLS.Helpers;
using CycleTLS.Interfaces;
using CycleTLS.Models;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
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
            WSResponseTask = new TaskCompletionSource<string>();

            if (options.Timeout.HasValue)
            {
                var cancelSource = new CancellationTokenSource(options.Timeout.Value);
                cancelSource.Token.Register(() => WSResponseTask.TrySetException(new TimeoutException($"No response after {options.Timeout.Value.TotalSeconds} seconds.")));
            }

            CycleRequest request = new CycleRequest();
            request.RequestId = $"{DateTime.Now}:{options.Url}";
            request.Options = options;

            try
            {
                await CycleWS.SendInstantJson(request);
                var response = await WSResponseTask.Task;
                return JsonConvert.DeserializeObject<CycleResponse>(response);
            }
            catch (Exception err)
            {
                return new CycleResponse()
                {
                    Status = System.Net.HttpStatusCode.RequestTimeout
                };
            }
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