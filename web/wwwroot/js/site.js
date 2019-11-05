// the code that creates the connection
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/streamingtime")
    .withAutomaticReconnect()
    .build();

// the method that starts the connection
async function start() 
{
    try 
    {
        connection.start().then(() => {
            startStreaming();
        });
    }
    catch 
    {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
}

// the code that starts the streaming
function startStreaming()
{
    connection
        .stream('ServerTimer')
            .subscribe({
                next: (serverTime) => {
                    document.getElementById('status').innerText = serverTime;
                },
                complete: () => {
                    console.log('complete!');
                },
                error: (err) => {
                    console.log(err);
                }
            });
}

// the code that handles disconnection events
connection.onreconnected((connectionId) =>
{
    startStreaming();
});

start();