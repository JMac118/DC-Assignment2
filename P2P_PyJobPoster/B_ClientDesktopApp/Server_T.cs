using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using P2P_Library;

namespace B_ClientDesktopApp
{
    internal class Server_T
    {
        ServiceHost serviceHost;
        NetTcpBinding tcp;
        int port;
        string ip_address;
        string name;
        public Server_T()
        {
            Task.Run(() =>
            {
                StartServerThread();
            });
        }

        private void StartServerThread()
        {
            InitializeServiceHost();
        }
        private void InitializeServiceHost()
        {
            port = 4000;
            name = "Josh_PC";
            tcp = new NetTcpBinding();

            Client_Net client_Net = new Client_Net();
            serviceHost = new ServiceHost(client_Net);

            ip_address = "net.tcp://localhost/" + port;

            serviceHost.AddServiceEndpoint(typeof(Client_Net_Interface), tcp, ip_address);
            serviceHost.Open();
        }

        private IPAddress GetIPAddress()
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses("");

            IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(Dns.GetHostName());

            return hostEntry.AddressList[0].MapToIPv4();
        }


    }
}
