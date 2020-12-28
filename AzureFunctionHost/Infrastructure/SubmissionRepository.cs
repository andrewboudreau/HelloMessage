using System.Threading;
using AzureFunctionHost.Domain;

namespace AzureFunctionHost.Infrastructure
{
    public class SubmissionRepository : ConcurrentDictionaryRepository<Submission>
    {
        public SubmissionRepository()
        {
            Add(Submission.For("seed-data@example.com"));
        }
    }
}
