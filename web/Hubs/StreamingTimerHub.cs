using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace RealtimeTimer.Hubs
{
    public class StreamingTimerHub : Hub
    {
        public IAsyncEnumerable<string> ServerTimer(CancellationToken token)
        {
            var channel = Channel.CreateUnbounded<string>();
            _ = WriteDateAsync(channel.Writer, token);
            return channel.Reader.ReadAllAsync();
        }

        private async Task WriteDateAsync(ChannelWriter<string> writer, 
            CancellationToken token)
        {
            try
            {
                while(true)
                {
                    token.ThrowIfCancellationRequested();
                    await writer.WriteAsync(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
                    await Task.Delay(10, token);
                }
            }
            catch
            {
                writer.TryComplete();
            }
            
            writer.TryComplete();
        }
    }
}