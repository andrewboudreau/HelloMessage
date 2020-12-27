using System;
using AzureFunctionHost.Domain;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Domain
{
    using static UniqueValueHelper;

    [TestClass]
    public class SubmissionFor
    {
        [TestMethod]
        public void ShouldCreatePendingSubmission()
        {
            var submission = Submission.For(Name());

            submission.Response.Should().BeNull(because: "the submission does not have a response.");
            submission.Pending.Should().Be(true, because: "the submission does not have a response.");
            submission.Rejected.Should().Be(false, because: "the submission does not have a response.");
            submission.Approved.Should().Be(false, because: "the submission does not have a response.");
        }

        [TestMethod]
        public void ShouldHaveTheGivenUserId()
        {
            var userId = Name();
            Submission.For(userId).UserId.Should().Be(userId);
        }

        [TestMethod]
        public void ShouldHaveBeenCreatedRecently()
        {
            var userId = Name();
            Submission.For(userId).Created.Should().BeCloseTo(DateTimeOffset.Now);
        }

        [TestMethod]
        public void ShouldNotHaveAnyDomainEvents()
        {
            var userId = Name();
            var submission = Submission.For(userId) as IHaveDomainEvents;
            submission.DomainEvents.Should().BeEmpty();
        }
    }
}
