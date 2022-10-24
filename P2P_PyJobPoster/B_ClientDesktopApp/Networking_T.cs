using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using P2P_Library;
using RestSharp;
using RestSharp.Authenticators;
using static IronPython.Modules._ast;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using static Community.CsharpSqlite.Sqlite3;
using System.Threading;
using System.Security.Cryptography;

namespace B_ClientDesktopApp
{
    // class to handle the networking thread. Will be made into a static object
    internal class Networking_T
    {
        // Stub
        int numJobsDone = 0;
        static List<Client> clients;
        Client serverT;
        bool isBusy = false;
        private static Random rnd = new Random();

        ChannelFactory<Client_Net_Interface> factory;

        public Networking_T(Client serverT)
        {
            this.serverT = serverT;
            clients = new List<Client>();
            Task.Run(() =>
            {
                StartNetworkThreadAsync();
            });
        }
        private async Task StartNetworkThreadAsync()
        {
            // Loops over looking for new clients
            // Then check each client for jobs
            while(true)
            {
                await LookForNewClientsAsync();
                CheckClientsForJobs();
                System.Threading.Thread.Sleep(20000);
            }
        }

        public int GetJobsDone() 
        { 
            return numJobsDone;
        }

        public bool CheckIfBusy()
        {
            return isBusy;
        }

        private async Task LookForNewClientsAsync()
        {
            //Client client = new Client(ip_address, port, name);

            RestClient restClient = new RestClient("https://localhost:44305/");
            RestRequest restRequest = new RestRequest("api/GetClients/" + serverT.ip_address + "/" + serverT.port, Method.Get);
            //restRequest.AddBody(JsonConvert.SerializeObject(client));

            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            try
            {
                List<Client> clientsTemp = JsonConvert.DeserializeObject<List<Client>>(restResponse.Content);
                if(clientsTemp != null)
                {
                    clients = clientsTemp;
                }
                Console.WriteLine("networkT retrieved list of clients");
                Console.WriteLine(clients.Count);
                foreach (Client client in clients)
                {
                    Console.WriteLine("NetT Got client: " + client.ip_address + ":" + client.port);
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine("Null clients list");
            }
            //Console.WriteLine(restResponse.Content);
        }

        private void ShuffleList(List<Client> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                Client value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        private void CheckClientsForJobs()
        {
            ShuffleList(clients);

            foreach(Client client in clients)
            {
                try
                {
                    NetTcpBinding tcp = new NetTcpBinding();

                    string URL = "net.tcp://" + client.ip_address + ":" + client.port;
                    factory = new ChannelFactory<Client_Net_Interface>(tcp, URL);
                    Client_Net_Interface client_net = factory.CreateChannel();
                    /*List<string> jobs = client_net.GetJobs();
                    if(jobs != null)
                    {
                        //Do the jobs
                        Console.WriteLine(jobs);
                    }*/
                    Console.WriteLine("netT attempting to get job from: " + client.ip_address + ":" + client.port);
                    Job job = client_net.GetJob();
                    if (job != null)
                    {
                        SHA256 sha256Hash = SHA256.Create();
                        byte[] hash = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(job.Work));

                        if (job.Hash.ToString().Equals(hash.ToString()))
                        {
                            // Do the job
                            string resultString = PerformTask(job);
                            Console.WriteLine("netT doing job: " + client.ip_address + ":" + client.port);
                            client_net.SubmitAnswer(job, resultString);
                            numJobsDone++;
                        }
                        else
                        {
                            Console.WriteLine("Hash not matching");
                        }
                    }
                }
                catch(Exception exc)
                {
                    Console.WriteLine("Checking for jobs and got an exception: " + exc.ToString());
                }
            }
        }

        private string PerformTask(Job job)
        {
            try
            {
                //string pyDef = "def run_func(): \n";

                //pyDef = pyDef + job.Work;
                isBusy = true;

                string pyDef = job.Work;

                if (!String.IsNullOrEmpty(job.Work))
                {
                    byte[] encodedStr = Convert.FromBase64String(job.Work);
                    pyDef = Encoding.UTF8.GetString(encodedStr);
                }
                

                ScriptEngine engine = Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();
                engine.Execute(pyDef, scope);
                dynamic runFunction = scope.GetVariable("run_func");
                var result = runFunction();
                Console.WriteLine(result);

                Thread.Sleep(10000);
                isBusy = false;

                return result.ToString();
            }
            catch(Exception exc)
            {
                isBusy = false;
                return "failed";
            }


            //string pyDef = "def run_func(var1, var2): return var1+var2";
            //int var1, var2;
            //var1 = 2;
            //var2 = 5;
            //string pyDef = "def run_func(): return ";

            //ScriptEngine engine = Python.CreateEngine();
            //ScriptSource source = engine.CreateScriptSourceFromString(job.Work);
            //ScriptScope scope = engine.CreateScope();
            //dynamic outcome = source.Execute(scope);
            //Console.WriteLine(outcome.ToString());
            //Console.WriteLine(outcome);
            //return outcome.ToString();
        }
    }
}
