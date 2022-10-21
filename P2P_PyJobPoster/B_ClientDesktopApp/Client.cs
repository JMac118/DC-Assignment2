using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_ClientDesktopApp
{
    public class Client
    {
        public Client(string ip_address, int port, string name)
        {
            this.ip_address = ip_address;
            this.port = port;
            this.name = name;
        }
        public string ip_address { get; set; }
        public int port { get; set; }
        public string name { get; set; }
    }
}
