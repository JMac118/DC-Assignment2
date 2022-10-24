using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2P_Library
{
    public class FinishedJob
    {

        public FinishedJob(string answer, int id)
        {
            this.answer = answer;
            this.clientId = id;
        }


        public int clientId { get; set; }
        public string answer { get; set; }
    }
}
