using System.Net.NetworkInformation;

namespace C_SwarmWebViewer.Models
{
    public class MyModel
    {
        public List<Client>? Clients { get; set; }
        public List<Work_Stat> Stats= new List<Work_Stat>();

        public void AddStat(Work_Stat value)
        {
            Stats.Add(value);
        }
    }
}
