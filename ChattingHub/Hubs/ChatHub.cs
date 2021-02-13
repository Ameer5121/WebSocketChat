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
        private static DataModel _usersAndMessages = new DataModel();
        private static List<string> _connections = new List<string>();
        public void SendMessages()
        {
            // TODO;
           // Clients.All.SendAsync("Yes");
        }
        public void AddUserData(UserModel data)
        {
            _usersAndMessages.Users.Add(data);
        }
        public void AddMessageData(MessageModel data)
        {
            _usersAndMessages.Messages.Add(data);
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Connected", _usersAndMessages);
            _connections.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }
    }
}
