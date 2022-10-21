using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using P2P_Library;
using RestSharp;

namespace B_ClientDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();


            testWebAPI();
        }

        private void testWebAPI()
        {
            //IPAddress iPAddress = GetIPAddress();
            //Client client = new Client(ip_address, port, name);

            //RestClient restClient = new RestClient("https://localhost:44305/");
            //RestRequest restRequest = new RestRequest("api/Clients", Method.Post);
            //restRequest.AddBody(JsonConvert.SerializeObject(client));

            //RestResponse restResponse = restClient.Execute(restRequest);

            //Console.WriteLine(restResponse.Content);
        }




    }
}
