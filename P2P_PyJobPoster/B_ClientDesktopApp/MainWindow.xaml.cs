using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        Server_T server_T;
        Networking_T networking_T;
        Client client;
        public MainWindow()
        {
            InitializeComponent();


            testWebAPI();
        }

        private async void testWebAPI()
        {
            server_T = new Server_T();
            Task<Client> startingServer = server_T.StartServerThread();
            client = await startingServer;
            networking_T = new Networking_T(client);
        }

        private void handleSubmit(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("gui submitting job");
            server_T.SubmitJob(txtCode.Text);
            Console.WriteLine("gui fin submit job: " + txtCode.Text);
        }

        private void handleGetStatus(object sender, RoutedEventArgs e)
        {

        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            server_T.Shutdown();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }
    }
}
