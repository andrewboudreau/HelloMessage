using AzureFunctionHost.Domain;

namespace AzureFunctionHost.Infrastructure
{
    public class SubmissionRepository : ConcurrentDictionaryRepository<Submission>
    {
    }
}
