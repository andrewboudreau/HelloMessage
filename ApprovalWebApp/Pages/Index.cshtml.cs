using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IActionResult> OnPost()
        {
            await client.PostApprovalAll(Environment.MachineName);
            return RedirectToPage();
        }
    }
}
