# HelloMessage 
An experiment in modern software design ideas.
Explores DDD, CQRS, EventStore, and various approaches to message exchanges. HelloMessage is a simple example that hopefully explores many of the main principles of 
distributed microservices and messaging using C# in .NET and Microsoft Azure.

# AzureFunctionHosts
Azure functions act as a host for the domain to send, receive, and store the data. AzureFunctionHost is a microservice.
## Domain
### Submission
A `Submission` is created for a `User` and submitted to the domain. 
A `Submission` can be `Accepted` or `Rejected` and will be in `Pending` state until the submission receives a `Response`.
An `Submission` will no not be `Pending` once an approval or rejection has been applied.
An `Approver` can accept or reject a `Submission`.

## Application
Application is the business logic and the first class consumer of the Domain. It provides infrastructure and logic atop of the Domain. Loose coupling and interfaces of the application 
take dependencies on the infrastructure but do not implement the details directly.

## Api
Two endpoints provide the interfaces need to submit and response to submissions.

### Submission
Submission HTTP endpoints provides a way to send and check the status of Submissions on the host

#### Approval
Approval HTTP endpoitns provide a way to query the pending `Submissions` as well as accept or reject the requests.

## Infrastructure
Provides the metal and hardware need to receive, send, and store data.

#SubmissionCommandLine
A console terminal that send submissions to the microservice.

#ApprovalWebApp
A website to query and approve active submissions.
