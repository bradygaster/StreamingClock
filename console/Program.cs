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
            Console.WriteLine("Connecting to timer");

            try
            {
                var connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:5001/streamingtime")
                    .Build();

                await connection.StartAsync();
                
                Console.WriteLine("Client connected. Streaming the time.");
                Console.WriteLine("Ctrl-C to exit.");

                var cancellationTokenSource = new CancellationTokenSource();
                var channel = await connection.StreamAsChannelAsync<string>("ServerTimer", 
                    cancellationTokenSource.Token);

                while(await channel.WaitToReadAsync())
                {
                    while (channel.TryRead(out var time))
                    {
                        Console.WriteLine(time);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
