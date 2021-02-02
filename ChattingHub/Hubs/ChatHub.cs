using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChattingHub.Hubs
{
    public class ChatHub : Hub
    {
        public void SendMessages()
        {
            Clients.All.SendAsync("Yes");
        }
    }
}
