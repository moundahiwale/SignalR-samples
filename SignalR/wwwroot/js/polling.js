let intervalId;

document.getElementById("submit_polling").addEventListener("click", e => {
    e.preventDefault();
    fetch("Pizza/OrderPizza",
        {
            method: "POST"
        })
        .then(response => response.text())
        .then(id => intervalId = setInterval(poll, 1000, id));
})

poll = (orderId) => {
    fetch(`/Pizza/GetUpdateForOrder/${orderId}`)
    .then(response => {
        if(response.status === 200) {
            response.json().then(j => {
                const status = document.getElementById("status");   
                status.innerHTML = j.update;
                if(j.finished)
                    clearInterval(intervalId);
            })
        }
    })
}