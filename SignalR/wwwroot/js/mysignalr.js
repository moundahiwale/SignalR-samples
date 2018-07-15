// To test SignalR fallback
// WebSocket = undefined;
// EventSource = undefined;

let connection = null;

document.getElementById("submit_signalR").addEventListener("click", e => {
    e.preventDefault();
    const product = "Pizza";
    fetch("Pizza/OrderPizzaSignalR",
        {
            method: "POST",
            body: JSON.stringify({ product }),
            headers: {
               'content-type': 'application/json'
            }
        })
        .then(response => response.text())
        .then(id => connection.invoke("GetUpdateForOrder", id));
});

setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/pizzahub"
        // 2nd parameter to withUrl function to make signalR use specific transport 
        // and switch off automatic selection
        // , signalR.HttpTransportType.LongPolling
    )
        .build();

    connection.on("ReceiveOrderUpdate", (update) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = update;
    });

    connection.on("NewOrder", function(order){
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = "Someone ordered a " + order.product;
    });

    connection.on("Finished", function() {
        connection.stop();
    });

    connection.start()
        .catch(err => console.error(err.toString()));
};

setupConnection();