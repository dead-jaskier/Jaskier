using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Agent.Models
{
    public class HttpCommModule : CommModule
    {
        public string ConnectAddress { get; set; }
        public int ConnectPort { get; set; }

        private CancellationTokenSource _tokenSource;
        private HttpClient _client;

        public HttpCommModule(string connectAddress, int connectPort)
        {
            ConnectAddress = connectAddress;
            ConnectPort = connectPort;
        }

        public override void Init(AgentMetaData metadata)
        {
            base.Init(metadata);

            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{ConnectAddress}:{ConnectPort}");
            _client.DefaultRequestHeaders.Clear();

        }

        public override Task Start()
        {
            _tokenSource = new CancellationTokenSource();

            while (!_tokenSource.IsCancellationRequested)
            {
                // checkin
                // get tasks
                // sleep
            }
        }

        private void Checkin()
        {

        }

        public override void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}
