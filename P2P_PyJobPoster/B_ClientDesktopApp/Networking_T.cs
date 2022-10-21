using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace B_ClientDesktopApp
{
    // class to handle the networking thread. Will be made into a static object
    internal class Networking_T
    {
        // Stub
        int numJobsDone = 5;
        List<>
        public Networking_T()
        {
            Task.Run(() =>
            {
                StartNetworkThread();
            });
        }
        private void StartNetworkThread()
        {
            // Loops over looking for new clients
            // Then check each client for jobs
            while(true)
            {
                LookForNewClients();
                CheckClientsForJobs();
                System.Threading.Thread.Sleep(1000);
            }
        }

        public async Task<int> GetJobsDone() 
        { 
            return await GetNumJobsDone;
        }

        private int GetNumJobsDone()
        {

        }

        private void LookForNewClients()
        {

        }

        private void CheckClientsForJobs()
        {

        }
    }
}
