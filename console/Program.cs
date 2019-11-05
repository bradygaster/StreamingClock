﻿using System;
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
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
