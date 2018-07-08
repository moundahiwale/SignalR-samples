## Technologies

| Dependency | Version*
| :--- | ---:
| .NET Core Framework | 2.1
| ASP.NET Core MVC | 2.1

## Getting Started

Real-time web applications are everywhere right from email clients (e.g. gmail), web documents or stock quotes 
and many more.

This project contains sample code for various real-time web techniques such as AJAX, Polling, Long Polling, 
Server Sent Events (SSE), WebSockets and finally SignalR.

SignalR offers many advantages such as choosing the best available transport automatically.

It also provides a fallback mechanism. WebSockets is the most efficient of the transports, if it is not supported by 
either the server or the client/browser, SignalR falls back on SSE and if that isn't supported then Long Polling.

![screen shot 2018-07-08 at 1 56 01 pm](https://user-images.githubusercontent.com/38259683/42420025-6e71c180-82b7-11e8-93b4-8bc8682fcd00.png)
