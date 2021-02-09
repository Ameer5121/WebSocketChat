using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace ChattingHub.Hubs
{
    public class ChatHub : Hub
    {
        private DataModel _usersAndMessages;
        public void SendMessages()
        {
            // TODO;
           // Clients.All.SendAsync("Yes");
        }

        public ChatHub()
        {
            _usersAndMessages = new DataModel();
        }

        public void AddData(UserModel data)
        {
            _usersAndMessages.Users.Add(data);
        }
        public void AddData(MessageModel data)
        {
            _usersAndMessages.Messages.Add(data);
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Connected", _usersAndMessages);
            return base.OnConnectedAsync();
        }

    }
}
