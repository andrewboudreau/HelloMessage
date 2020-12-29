using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ApprovalWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IApprovalClient client;
        private readonly ILogger<IndexModel> logger;

        public IndexModel(IApprovalClient client, ILogger<IndexModel> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public PendingSubmission[] Pending { get; private set; }

        public async Task OnGet()
        {
            Pending = await client.GetPending();
        }

        public async Task<IActionResult> OnPost(Guid[] submissions, bool reject = false)
        {
            using var throttler = new SemaphoreSlim(initialCount: 5);

            var tasks = submissions.Select(async submissionId =>
            {
                try
                {
                    await throttler.WaitAsync();
                    if (reject)
                    {
                        await client.PostRejection(Environment.MachineName, submissionId);
                    }
                    else
                    {
                        await client.PostApproval(Environment.MachineName, submissionId);
                    }
                }
                finally
                {
                    throttler.Release();
                }
            });

            await Task.WhenAll(tasks);
            return RedirectToPage();
        }
    }
}
