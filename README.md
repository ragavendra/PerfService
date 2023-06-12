# PerfService
Service to enable load testing on different types of network calls.

Welcome to the PerfService wiki!

PerfService is a whole suite of `dotnet` projects to run performance test(s)

a. PerfRunner
b. PerfLoader
c. PerfRunnerTests

under one repository.

### PerfRunner
The actual hoster and runner of service which has a folder called tests which contain the tests. Simply copy the `TestBase.cs` in it to your new test and update the `RunTest` method in it to suit your performance test needs. The http client has been typed DIed into `_httpClient` which can be used in the test(s) to make the http calls along with an end point.

### PerfLoader
Front end or the web interface to initiate performance test run(s) and control or to stop them.

### PerfRunnerTests
Unit tests for the `PerfRunner` to be kept updated with any new features added to the `PerfRunner` project.

### Planned features
1. Migrate to use Semaphore to use limited no. of threads per second instead of manual control?
2. Try using interface inherit implementation.

### Existing features
1. Abitlity to create and run http test(s).
2. UI to run, view and stop test(s).
3. Ability to update rate per sec during test run.
4. Abitlity to create and run gRPC test(s).
5. Account data support with `User` object having state has well. Each user have their own state and are in the queue per state and updated by the test(s) accordingly.
6. User state management - as user queue per each state.
7. Ability to transfer data across test(s) - can be achieved by adding new data properties to the `User` object itself, say like the phone number, library card number, health number and so on. This is updated and be used by each test(s) accordingly by dequeing that state queue with the user with that updated data. Say the user just registeted at the front desk with payment, library no, his state is now authenticated and library no. is the new data and he is in the authenticated state queue. Whenever its his turn, that user gets popped from that queue. 
8. Support for flag for equal or uneven load distribution per second.
8a. If each action has a rate that is used for rate otherwise default to test rate.

### Planned features - Loader
1. Update/ Edit test params during run like rate, distribution.
2. Clean interface for interaction.

### Sample Web app
For this use case, I am using the Bowling alley web app, assuming when a user goes to the ally, he has to say, get autheneticated, next wait for the lane and if lane is available, play can be initiated. For each stage, state can be represented, which is defined in `UserState`.

### Sample commands
```
$ grpcurl -plaintext localhost:5277 describe
perf.Perf is a service:
service Perf {
  rpc Ping ( .perf.PingRequest ) returns ( .perf.PingReply );
  rpc RunTest ( .perf.TestRequest ) returns ( .perf.TestReply );
  rpc StopAllTests ( .perf.StopAllTestsRequest ) returns ( .perf.StopAllTestsReply );
  rpc StopTest ( .perf.StopTestRequest ) returns ( .perf.StopTestReply );
  rpc UpdateRate ( .perf.UpdateRateRequest ) returns ( .perf.UpdateRateReply );
}
grpc.reflection.v1alpha.ServerReflection is a service:
service ServerReflection {
  rpc ServerReflectionInfo ( stream .grpc.reflection.v1alpha.ServerReflectionRequest ) returns ( stream .grpc.reflection.v1alpha.ServerReflectionResponse );
}
```

```
$ grpcurl -plaintext localhost:5277 describe perf.TestRequest
perf.TestRequest is a message:
message TestRequest {
  string guid = 1;
  string name = 2;
  int32 rate = 3;
  repeated .perf.ActionOption actions = 4;
}
```