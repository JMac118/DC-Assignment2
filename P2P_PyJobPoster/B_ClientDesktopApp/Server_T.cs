using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using P2P_Library;
using RestSharp;

namespace B_ClientDesktopApp
{

    internal class Server_T
    {
        Client_Net client_Net;
        ServiceHost serviceHost;
        NetTcpBinding tcp;
        int port;
        string ip_address;
        string name;
        static List<Job> jobs;
        Client client;
        int jobsCurrent;
        int jobsDone;


        public Server_T()
        {
            jobsCurrent = 0;
            jobsDone = 0;
            jobs = new List<Job>();
            ip_address = "127.0.0.1";
            //Task.Run(() =>
            //{
            //StartServerThread();
            //});
        }

        public void Shutdown()
        {
            RestClient restClient = new RestClient("https://localhost:44305/");
            RestRequest restRequest = new RestRequest("api/Clients", Method.Delete);
            restRequest.AddBody(JsonConvert.SerializeObject(client));

            RestResponse restResponse = restClient.Execute(restRequest);

            Console.WriteLine(restResponse.Content);
        }

        public Client GetCurrentDetails()
        {
            return client;
        }
        public async Task<Client> StartServerThread()
        {
            await InitializeServiceHost();
            return GetCurrentDetails();
        }

        public void FinishJob(Result result)
        {
            Console.WriteLine("server finish job invoked");
            jobsDone++;
            jobsCurrent--;
            //jobs.Remove(result.Job);
            Console.WriteLine("Server finished a job: " + result.Job + ", result given was: " + result.CompletedWork);
        }

        private async Task InitializeServiceHost()
        {
            port = 8000;
            name = "Josh_PC";

            for(int i = 0; i < 100; i++) // Try to increment port number 100 times
            {
                try
                {
                    tcp = new NetTcpBinding();

                    client_Net = new Client_Net(jobs, FinishJob);
                    serviceHost = new ServiceHost(client_Net);
                    serviceHost.AddServiceEndpoint(typeof(Client_Net_Interface), tcp, ("net.tcp://localhost:" + port) );
                    serviceHost.Open();
                    RegisterToServer();
                    return;
                }
                catch (Exception exc)
                {
                    port++;
                }
            }


        }
        public void SubmitJob(string work)
        {
            Console.WriteLine("client server got work: " + work);
            Job job = new Job(jobsCurrent, work);
            jobsCurrent++;
            jobs.Add(job);
            Console.WriteLine("client server currently has jobs: " + jobs);
        }

        private IPAddress GetIPAddress()
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses("");

            IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(Dns.GetHostName());

            return hostEntry.AddressList[0].MapToIPv4();
        }

        private void RegisterToServer()
        {
            //IPAddress iPAddress = GetIPAddress();
            client = new Client(ip_address, port, name);

            RestClient restClient = new RestClient("https://localhost:44305/");
            RestRequest restRequest = new RestRequest("api/Clients", Method.Post);
            restRequest.AddBody(JsonConvert.SerializeObject(client));

            RestResponse restResponse = restClient.Execute(restRequest);

            Console.WriteLine(restResponse.Content);
        }


    }
}
