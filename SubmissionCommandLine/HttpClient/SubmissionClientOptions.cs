﻿using System;

namespace SubmissionCommandLine
{
    public class SubmissionClientOptions : HttpClientOptions
    {
        public const string SubmissionClient = "SubmissionClient";

        public string Submit { get; set; }

        public string Status { get; set; }

        public string AccessToken { get; set; }

        public SubmissionClientOptions WithBaseAddress(Uri baseUri)
        {
            BaseAddress = baseUri;
            return this;
        }
    }
}
