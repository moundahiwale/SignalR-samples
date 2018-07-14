document.getElementById("submit_websockets").addEventListener("click", e => {
    e.preventDefault();
    fetch("Pizza/OrderPizza",
        {
            method: "POST"
        })
        .then(response => response.text())
        .then(id => listen(id));
});

listen = (id) => {
    // Uses ws as the protocol header and not http
    // There is also a wss similar to https - use wss in production
    const socket = new WebSocket(`wss://localhost:5001/WS/${id}`);
    socket.onmessage = event => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = JSON.parse(event.data);
    }
    socket.onerror = function(evt) {
        onError(evt)
    };

    function onError(evt) {
        console.log(evt.data);
    } 
}