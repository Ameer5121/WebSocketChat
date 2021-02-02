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
        private ChatHub _chathub;
        public ChatController()
        {
            _chathub = new ChatHub();
        }

        [HttpGet]
        [Route("api/[controller]/messages")]
        public void GetMessages()
        {
            _chathub.SendMessages();
        }
    }
}
