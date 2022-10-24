using System.Diagnostics;
using C_SwarmWebViewer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace C_SwarmWebViewer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            RestClient restClient = new RestClient("https://localhost:44305/");
            RestRequest restRequest = new RestRequest("api/Clients", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);

            List<Client> clients = JsonConvert.DeserializeObject<List<Client>>(restResponse.Content);
            MyModel model = new MyModel();

            if (clients != null)
            {
                model.Clients = clients;

                foreach(Client client in clients)
                {
                    Work_Stat stat = new Work_Stat();
                    stat.Client = client;
                    stat.NumCompleted = GetCompletedJobCount(client.Id);
                    model.Stats.Add(stat);
                }
            }
            else
            {
                model.Clients = new List<Client>();
            }


            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       private int GetCompletedJobCount(int clientId)
        {
            RestClient restClient = new RestClient("https://localhost:44305/");
            RestRequest restRequest = new RestRequest("api/Jobs", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);

            List<Job> jobs = JsonConvert.DeserializeObject<List<Job>>(restResponse.Content);

            int count = 0;
            if (jobs != null)
            {
                foreach (Job job in jobs)
                {
                    if (job.ClientId == clientId)
                    {
                        count++;
                    }
                }
            }
            else
            {
                count = -1;
            }

            return count;
        }
    }
}