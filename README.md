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
<a href="PerfLoader.png" target="_blank">Screen</a>

### PerfRunnerTests
Unit tests for the `PerfRunner` to be kept updated with any new features added to the `PerfRunner` project.

### Architecture
![Architecture](Architecture.svg)

### Getting Started
#### Docker
Once in this directory. Should be as straight as 

`sudo docker build -t perfrunner .`

and

`docker compose up --no-build`

The `mcr.microsoft.com/dotnet/sdk:6.0` and `redis:alpine` images are used.

### Redic Cache manager
Redis is used as a common user store to store users and are pulled and pushed into a queue there say when one or multiple `PerfRunner`s are running.

`PerfRunner` can alone be run without the `redis` as well, in which case the user store should default to the in-memory queue and running multiple `Perfrunner`s can cause the same user to call the api's from each of the `PerfRunner`s.

### Planned features
1. Assunming runner(s) running a single test, test users may have to be shared across them. Need a cache or ESB queue to may be write to db or not on cloud. The UserMgr may have to fetch user(s) from that cache or queue.
2. Migrate to use Semaphore to use limited no. of threads per second instead of manual control?

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
9. Try using interface inherit implementation.
10. Monitor traffic in prometheus or Grafana cloud. Check the Grafana dashboard [json](PerfRunner/grafanaDashboard.json) to import it to the Grafana dashboard.
11. Update rate for individual action.
12. Pause/ Unpause individual action.

### Planned features - Loader
1. Update/ Edit test params during run like rate, distribution.
2. Clean interface for interaction.

### Sample Web app
For this use case, I am using the Bowling alley web app, assuming when a user goes to the ally, he has to say, get autheneticated, next wait for the lane and if lane is available, play can be initiated. For each stage, state can be represented, which is defined in `UserState`.

### Prometheus
Prometheus is used to more or less display or relay the instrumentation metrics to apps like Grafana or similar. To install you may have to download and run it on oyur distribution. The config file for it is [here](PerfRunner/grafanaDashboard.json) or append the scrape config like below.

```
  - job_name: 'PerfRunner'
    scrape_interval: 1s # poll very quickly for a more responsive demo
    static_configs:
      - targets: ['localhost:9186']
```

### Grafana
To run Grafana as a docker image, run.

``` 
docker run -d --name=grafana -p 3000:3000 grafana/grafana-enterprise
admin
```

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
### Contributing
Please read ![CONTRIBUTING](CONTRIBUTING.md) to help make any contributions to this project or repository.

### License
Free for non-commercial use, but please read ![LICENSE](LICENSE) for commercial use, other(s) and support.

### Customizations/ Support
<a href="https://sites.google.com/view/garden-systems" target="_blank"><img src="Garden-Systems-logos_transparent.svg" style="width:100px;height:100px"></a>

### Donations
If you like using this repository and like to donate, please visit the below link. This work is made possible with donations like yours. PM for customizations and implementations .

<a href="https://www.buymeacoffee.com/ragavendra"><img src="https://img.buymeacoffee.com/button-api/?text=Buy me a pop&emoji=🥃&slug=ragavendra&button_colour=FFDD00&font_colour=000000&font_family=Cookie&outline_colour=000000&coffee_colour=ffffff" /></a>

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ZKRHDCLG22EJA)
