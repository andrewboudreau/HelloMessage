using System;

namespace SubmissionCommandLine
{
    public class HttpClientOptions
    {
        public Uri BaseAddress { get; set; }

        public TimeSpan Timeout { get; set; }
    }
}
