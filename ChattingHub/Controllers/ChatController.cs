using ChattingHub.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace ChattingHub.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private ChatHub _chathub;
        private IHubContext<ChatHub> _hubContext;
        public ChatController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            _chathub = new ChatHub();
        }
        [HttpPost]
        [Route("PostUser")]
        public void PostUser(UserModel user)
        {
           _chathub.AddUserData(user);  
        }

        [HttpPost]
        [Route("PostMessage")]
        public void AddMessage(MessageModel message)
        {
           _chathub.AddMessageData(message, _hubContext);
        }

        [HttpGet]
        [Route("GetHeartBeat")]
        public string GetHeartBeat()
        {
            return "Alive";
        }
    }
}
