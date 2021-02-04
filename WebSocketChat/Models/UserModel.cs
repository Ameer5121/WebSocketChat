using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketChat.Models
{
    public struct UserModel
    {
        private string _name;
        public string Name 
        {
            get => _name;
            set => _name = value;  
        }
    }
}
