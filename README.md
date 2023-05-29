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
1. Account data support.
2. Ability to transfer data across test(s).

### Existing features
1. Abitlity to create and run http test(s).
2. UI to run, view and stop test(s).
3. Ability to update rate per sec during test run.