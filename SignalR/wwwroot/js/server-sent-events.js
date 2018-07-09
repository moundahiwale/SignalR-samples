document.getElementById("submit_server_sent_events").addEventListener("click", e => {
    e.preventDefault();
    fetch("Pizza/OrderPizza",
        {
            method: "POST"
        })
        .then(response => response.text())
        .then(id => listen(id));
});

listen = (id) => {
    var eventSource = new EventSource(`SSE/${id}`);

    eventSource.onmessage = (event) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = event.data;
    };

    eventSource.onerror = (error) => {
        console.log('Event Source Error occured');
    };
}