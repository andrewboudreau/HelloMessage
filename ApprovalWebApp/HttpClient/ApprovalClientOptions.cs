using System;

namespace ApprovalWebApp
{
    public class ApprovalClientOptions : HttpClientOptions
    {
        public const string ApprovalWebApp = "ApprovalWebApp";

        public string PostApproval { get; set; }

        public string GetPending { get; set; }

        public string AccessToken { get; set; }

        public ApprovalClientOptions WithBaseAddress(Uri baseUri)
        {
            BaseAddress = baseUri;
            return this;
        }
    }
}
