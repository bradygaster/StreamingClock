using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/streamingtime")
                .Build();

            await connection.StartAsync();

            await StartStreaming(connection);
        }

        static async Task StartStreaming(HubConnection connection)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var stream = connection.StreamAsync<string>("ServerTimer", cancellationTokenSource.Token);

            await foreach (var time in stream)
            {
                Console.Clear();
                Console.WriteLine("Client connected. Streaming the time.");
                Console.WriteLine("Ctrl-C to exit.");
                Console.WriteLine(time);
            }
        }
    }
}
