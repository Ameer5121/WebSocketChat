# Web Socket Chat

This application works using a user-hosted server along with SignlarR's library to allow for many people to chat simultaneously.

# Contains usages of:
* MultiThreading through **System.Threading**
* Socket Programming through **Microsoft.AspNetCore.SignalR.Client**
* JSON serialization through **Newtonsoft.Json**
* Web API project to handle HTTP requests and send them over to SignlarR's hub.

# Core functionality :
* List of connected users, automatically updating when one joins/leaves
* Returning to home view automatically when server is down
* Allows for External/Internal connections
* Logging out manually.

# Framework
* Made in WPF
     * MVVM Design Pattern
 
 # Third-Party Libraries
 * SignlaR
 * Newtonsoft JSON
 * MaterialDesignThemes
