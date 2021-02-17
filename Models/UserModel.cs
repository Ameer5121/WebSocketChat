using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public struct UserModel
    {
        private string _name;
        private string _endPoint;
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string EndPoint
        {
            get => _endPoint;
            set => _endPoint = value;
        }
    }
}
