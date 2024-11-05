using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NotificationChannelParserApp
{
    public class NotificationWorkerService : IHostedService
    {
        private readonly Channel<string> _channel;
        private readonly ILogger _logger;
        private string respoonse = "Receive channels:";

        public NotificationWorkerService(ILogger<NotificationWorkerService> logger,
 IHostApplicationLifetime appLifetime, Channel<string> channel)
        {
            _channel = channel;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (await _channel.Reader.WaitToReadAsync())
                {
                    while (_channel.Reader.TryRead(out string? text))
                    {
                        respoonse += text + ",";
                    }
                    Console.WriteLine(respoonse.Substring(0, respoonse.Length - 1));
                }

            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
