using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using A_WebServer.Models;
using P2P_Library;

namespace A_WebServer.Controllers
{
    public class ClientsController : ApiController
    {
        private clientDBEntities db = new clientDBEntities();

        // GET: api/Clients
        public List<Client> GetClients()
        {
            return db.Clients.ToList();
        }

        /* 
           * Client needs to input their ip address and port to get a list of all the 
           * other clients. If they are not in the list, they cannot access the other
           * clients.
           */
        // GET: api/Clients

        [Route("api/GetClients/{ip}/{port}")]
        [HttpGet]
        [ResponseType(typeof(List<Client>))]
        public IHttpActionResult GetClients(string ip, int port)
        {
            //List<Client> clients = db.Clients.ToList();
            List<Client> clients = new List<Client>();
            bool found = false;
            Debug.WriteLine("api getting clients");
            Debug.WriteLine("api current list count: " + db.Clients.ToList().Count);
            foreach (Client client in db.Clients.ToList())
            {
                Debug.WriteLine("api got client: " + client.ip_address + ":" + client.port);
                if(client.ip_address.Equals(ip) && client.port.Equals(port))
                {
                    Debug.WriteLine("api confirmed client within db: " + client.ip_address + ":" + client.port);
                    // If it found one that matches, then will return the list.
                    // Otherwise wont return anything.
                    // Client needs to be in the swarm in order to retrieve the list of the other clients.
                    found = true;

                }
                else
                {
                    Debug.WriteLine("api added client from db to list: " + client.ip_address + ":" + client.port);
                    //clients.Remove(client);
                    clients.Add(client);
                }
            }
            if (found)
            {
                Debug.WriteLine("api returning list of size: " +clients.Count);
                return Ok(clients);
            }
            else
            {
                return BadRequest();
            }
        }

        // GET: api/Clients/5
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClient(int id)
        {
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        // PUT: api/Clients/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClient(int id, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.id)
            {
                return BadRequest();
            }

            db.Entry(client).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Clients
        [HttpPost]
        [ResponseType(typeof(Client))]
        public IHttpActionResult RegisterClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Clients.Add(client);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = client.id }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete]
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClient(int id)
        {
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Clients.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }
        // DELETE: api/Clients/5
        [HttpDelete]
        [ResponseType(typeof(Client))]
        public IHttpActionResult UnregisterClient(Client client)
        {
            bool found = false;
            List<Client> clients = db.Clients.ToList();
            foreach(Client c in clients)
            { 
                if(c.ip_address.Equals(client.ip_address) && c.port.Equals(client.port))
                {
                    db.Clients.Remove(c);
                    db.SaveChanges();
                    found = true;
                }
            }
            if (found == false)
            {
                return NotFound();
            }

            return Ok(client);
        }

        internal void StartClientCheckTask()
        {
            //Timer timer = new Timer(CheckConnections, null, 5000, 5000);
            Task.Run(() =>
            {
                while(true)
                {
                    CheckConnections();
                    Thread.Sleep(5000);
                }
            });
        }

        internal void CheckConnections()
        {
            List<Client> clients = db.Clients.ToList();
            foreach (Client client in clients)
            {
                try
                {
                    NetTcpBinding tcp = new NetTcpBinding();

                    string URL = "net.tcp://" + client.ip_address + ":" + client.port;
                    ChannelFactory<Client_Net_Interface> factory = new ChannelFactory<Client_Net_Interface>(tcp, URL);
                    Client_Net_Interface client_net = factory.CreateChannel();

                    client_net.GetJob();
                }
                catch(Exception)
                {
                    db.Clients.Remove(client);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Clients.Count(e => e.id == id) > 0;
        }

        internal void ResetDb()
        {
            //static List<Client> clients = db.Clients.ToList();
            foreach (Client entry in db.Clients.ToList())
            {
                db.Clients.Remove(entry);
            }
            db.SaveChanges();
        }
    }
}