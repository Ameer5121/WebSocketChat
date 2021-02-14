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
    [ApiController]
    public class ChatController : ControllerBase
    {
        private ILogger<ChatController> _logger;
        private ChatHub _chathub;
        private IHubContext<ChatHub> _hubContext;
        public ChatController(ILogger<ChatController> logger, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
            _chathub = new ChatHub();
        }

        [HttpPost]
        [Route("api/chat/PostUser")]
        public void PostUser(UserModel user)
        {
            if(user.Name != null)
            {
                _chathub.AddUserData(user);
            }
        }

        [HttpPost]
        [Route("api/chat/PostMessage")]
        public void AddMessage(MessageModel message)
        {
            if (message.Message != null && message.User.Name != null)
            {
                _chathub.AddMessageData(message, _hubContext);
            }
        }

        [HttpGet]
        [Route("api/chat/GetHeartBeat")]
        public string GetHeartBeat()
        {
            return "Alive";
        }
    }
}
