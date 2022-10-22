using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace P2P_Library
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single,
        UseSynchronizationContext = true,
        InstanceContextMode = InstanceContextMode.Single)]
    public class Client_Net : Client_Net_Interface
    {
        static List<Job> jobs;
        Action<Result> finishJob;

        public Client_Net(List<Job> inJobs, Action<Result> finishJob)
        {
            jobs = inJobs;
            this.finishJob = finishJob;
        }
        public Job GetJob()
        {
            Console.WriteLine("client_net getting job: ");
            if(jobs.Count == 0)
            {
                return null;
            }
            else
            {
                Console.WriteLine("client_net got job: " + jobs.First());
                return jobs.First();
            }
                
        }

        public bool SubmitAnswer(Job job, string result)
        {
            Console.WriteLine("client net recieved result: " + result + " for job: " + job.Work);
            Console.WriteLine("is it in list? " + jobs.Contains(job));
            foreach (Job j in jobs)
            {
                if (j.Id == job.Id && j.Work.Equals(job.Work))
                {
                    // Do the job
                    jobs.Remove(j);
                    Console.WriteLine("Job done: " + job + " with result: " + result);
                    Result complete = new Result(job, result);
                    // Callback to server to increment jobs done and send info to database
                    finishJob.Invoke(complete);
                    return true;
                }
            }
            return false;
        }
    }
}
