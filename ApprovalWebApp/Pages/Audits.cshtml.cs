using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ApprovalWebApp.Pages
{
    public class AuditsModel : PageModel
    {
        private readonly IApprovalClient client;
        private readonly ILogger<AuditsModel> logger;

        public AuditsModel(IApprovalClient client, ILogger<AuditsModel> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public SubmissionAudit[] Audits { get; private set; }

        public async Task OnGet(int skip = 0, int take = 100)
        {
            Audits = await client.GetAudits(skip, take);
        }
    }
}
