using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketChat.Models
{
    readonly struct UserModel
    {
        public readonly string Name;

        public UserModel(string name)
        {
            this.Name = name;
        }
    }
}
