using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace P2P_Library
{
    [DataContract]
    public class Result
    {
        [DataMember]
        Job job;
        [DataMember]
        string completedWork;

        public Result(Job job, string completedWork)
        {
            this.job = job;
            this.completedWork = completedWork;
        }

        public Job Job { get => job; set => job = value; }
        public string CompletedWork { get => completedWork; set => completedWork = value; }
    }
}
