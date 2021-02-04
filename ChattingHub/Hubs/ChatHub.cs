using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketChat.Models;

namespace ChattingHub.Hubs
{
    public class ChatHub : Hub
    {
        public static DataModel UsersAndMessages;

        public void SendMessages()
        {
            Clients.All.SendAsync("Yes");
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Connected", UsersAndMessages);
            return base.OnConnectedAsync();
        }
    }
}
