document.getElementById("submit_long_polling").addEventListener("click", e => {
    e.preventDefault();
    fetch("Pizza/OrderPizza",
        {
            method: "POST"
        })
        .then(response => response.text())
        .then(id => longPoll(id));
});

longPoll = (orderId) => {
    pollWithTimeOut(`LongPolling/${orderId}`)
        .then(response => {
            if(response.status === 200) {
                const statusDiv = document.getElementById("status");
                response.json().then(j=> {
                    statusDiv.innerHTML = j.update;
                    if(!j.finished)
                        longPoll(orderId);
                });
            }
        }
    )
    .catch(response => longPoll(orderId));
}

pollWithTimeOut = (url, options, timeout = 9000) => {
    // Setup a race between two promises: 
    // 1. fetch promise that represents the request
    // 2. Promise that will reject if a timer of 9 secs has elapsed
    // The 1st promise that is done either in the result or rejected state will be returned
    return Promise.race([
        fetch(url, options),
        new Promise((_,reject) =>
            setTimeout(() => reject(new Error('timeout')), timeout))
    ]);
}