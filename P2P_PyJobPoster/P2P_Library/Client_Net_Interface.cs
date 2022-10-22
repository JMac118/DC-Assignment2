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
        [OperationContract]
        Job GetJob();

        [OperationContract]
        bool SubmitAnswer(Job job, string result);
    }
}
