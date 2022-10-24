using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace P2P_Library
{
    [DataContract]
    public class Job
    {
        [DataMember]
        string work;
        [DataMember]
        byte[] hash;
        [DataMember]
        int id;
        public Job(int id, string work, byte[] hash)
        {
            this.id = id;
            this.work = work;
            this.hash = hash;
        }


        public string Work { get => work; set => work = value; }
        public int Id { get => id; set => id = value; }

        public byte[] Hash { get => hash; set => hash = value; }
    }
}
