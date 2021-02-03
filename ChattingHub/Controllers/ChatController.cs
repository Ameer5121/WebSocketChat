using ChattingHub.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChattingHub.Controllers
{
    [ApiController]
    public class ChatController : ControllerBase
    {
        private ILogger<ChatController> _logger;
        private ChatHub _chathub;
        public ChatController(ILogger<ChatController> logger, ChatHub chathub)
        {
            _chathub = chathub;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/chat/messages")]
        public void GetMessages()
        {
            _chathub.SendMessages();
        }
    }
}
