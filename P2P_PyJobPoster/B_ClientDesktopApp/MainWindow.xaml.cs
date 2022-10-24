using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
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
using IronPython.Runtime.Exceptions;
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
            string job;

            if(String.IsNullOrEmpty(txtCode.Text))
            {
                job = txtCode.Text;
            }
            else
            {
                byte[] txtBytes = Encoding.UTF8.GetBytes(txtCode.Text);
                job = Convert.ToBase64String(txtBytes);
            }
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] hash = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(job));
                server_T.SubmitJob(job, hash);
                Console.WriteLine("gui fin submit job: " + job);
            }
           
        }

        private void handleGetStatus(object sender, RoutedEventArgs e)
        {
            if (networking_T.CheckIfBusy())
            {
                txtStatus.Text = "Working on job";
            }
            else 
            {
                txtStatus.Text = "Not working on job";
            }

            txtCompleted.Text = networking_T.GetJobsDone().ToString();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            server_T.Shutdown();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void handleCrash(object sender, RoutedEventArgs e)
        {
            throw new ApplicationException();
        }
    }
}
