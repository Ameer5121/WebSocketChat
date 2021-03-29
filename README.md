# Web Socket Chat

This application works using a user-hosted server along with SignlarR's library to allow for many people to chat simultaneously.

Internal             |  External
:-------------------------:|:-------------------------:
![1](https://user-images.githubusercontent.com/71935713/112846357-a44ab200-90ae-11eb-8814-33c1a4e37885.png)  | ![2](https://user-images.githubusercontent.com/71935713/112846477-c6443480-90ae-11eb-963c-fcc85717714f.png)

![fc2a4720c5028ae1b00b6aa7a69c21fb](https://user-images.githubusercontent.com/71935713/112846695-00153b00-90af-11eb-9cb0-5d73f071a967.png)

# Contains usages of:
* MultiThreading through **System.Threading**
* Socket Programming through **Microsoft.AspNetCore.SignalR.Client**
* JSON serialization through **Newtonsoft.Json**
* Web API project to handle HTTP requests and send them over to SignlarR's hub.

# Core functionality:
* List of connected users, automatically updating when one joins/leaves
* Returning to home view automatically when server is down
* Allows for External/Internal connections
* Logging out manually.

# Framework:
* Made in WPF
     * MVVM Design Pattern
 
 # Third-Party Libraries:
 * SignlaR
 * Newtonsoft JSON
 * MaterialDesignThemes
