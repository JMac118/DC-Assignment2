using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace P2P_Library
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
        UseSynchronizationContext = false,
        InstanceContextMode = InstanceContextMode.Single)]
    public class Client_Net : Client_Net_Interface
    {
        public string GetJobs()
        {
            return "Here's a job";
        }
    }
}
