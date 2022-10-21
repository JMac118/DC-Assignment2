using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace P2P_Library
{
    [ServiceContract]
    public interface Client_Net_Interface
    {
        // Will need to be the Iron Python thing.
        // Jobs are python jobs, this is a test stub method.
        [OperationContract]
        string GetJobs();
    }
}
