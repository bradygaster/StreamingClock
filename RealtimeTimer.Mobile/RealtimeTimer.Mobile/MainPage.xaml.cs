using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace RealtimeTimer.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async Task ConnectToStreamingTimer()
        {
            try
            {
                var connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/streamingtime")
                    .Build();

                await connection.StartAsync();

                var cancellationTokenSource = new CancellationTokenSource();
                var channel = await connection.StreamAsChannelAsync<string>("ServerTimer",
                    cancellationTokenSource.Token);

                while (await channel.WaitToReadAsync())
                {
                    while (channel.TryRead(out var time))
                    {
                        _label.Text = time;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ConnectToStreamingTimer();
        }
    }
}
