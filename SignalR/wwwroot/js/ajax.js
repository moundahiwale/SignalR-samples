document.getElementById("submit_ajax").addEventListener("click", e => {

    e.preventDefault();

    fetch("Pizza/OrderPizza",
        {
            method: "POST"
        })
        .then(response => response.text())
        .then(response => {
            const status = document.getElementById("status");
            status.innerHTML = "Order Id: " + response;
        });
});
