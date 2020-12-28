using System;
using System.Diagnostics.CodeAnalysis;
using AzureFunctionHost.Domain;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Domain
{
    using static UniqueValueHelper;

    [TestClass]
    public class SubmissionApproveBy
    {
        private Submission submission;

        [TestInitialize]
        public void Setup()
        {
            // Arrange
            submission = Submission.For(Name());
        }

        [TestMethod]
        public void ShouldSetResponse()
        {
            // Act
            submission.ApproveBy(Name());

            // Assert
            submission.Approved.Should().Be(true, because: "approving should approve the submission");

            submission.Response.Should().NotBeNull(because: "approving should provide a response");
            submission.Pending.Should().Be(false, because: "approving should provide a response");
            submission.Rejected.Should().Be(false, because: "approving should not reject the submission");
        }

        [TestMethod]
        public void ShouldHaveApproverName()
        {
            var approver = Name();
            submission.ApproveBy(approver);
            submission.Response.UserId.Should().Be(approver);
        }

        [TestMethod]
        public void ShouldHaveBeenCreatedRecently()
        {
            submission.ApproveBy(Name());
            submission.Response.Created.Should().BeCloseTo(DateTimeOffset.Now);
        }

        [TestMethod]
        public void ShouldProduceApprovedDomainEvent()
        {
            submission.ApproveBy(Name());

            var sut = submission as IHaveDomainEvents;
            sut.DomainEvents.Should().HaveCountGreaterThan(0, because: "an event should be produced when a submission is approved");
        }
    }
}
