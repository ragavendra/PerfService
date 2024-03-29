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

Each instance PerfRunner instance can be configured to run from a certain email index to a count so that no instance uses the same accounts. `AccountStartIndex` and `TotalUsers` need to be updated accordingly in the appsettings json, like

Instance1 `AccountStartIndex = 1_000 ` and `TotalUsers = 300`
Instance2 `AccountStartIndex = 1_301 ` and `TotalUsers = 500`
and so on.

To run on a specific port, when running multiple instances.

```
dotnet run --no-build Debug urls=http://0.0.0.0:5278
```

PS - Need to update `docker-compose.yml` for more than one `PerfRunner`

### PerfLoader
Front end or the web interface to initiate performance test run(s) and control or to stop them.
<a href="PerfLoader.png" target="_blank">Screen</a>

In development, you can use watch like
```
dotnet watch
```
or you will have to build and run
```
dotnet run build
dotnet run --no-build Debug
```
or in one command
```
dotnet run build Debug
```

### PerfRunnerTests
Unit tests for the `PerfRunner` to be kept updated with any new features added to the `PerfRunner` project.

### Architecture
![Architecture](Architecture.svg)

### Getting Started
#### Docker
Creating container using one `PerfRunner` image and one `Redis` image like microservice architecture. You can add many `PerfRunner`'s running in different ports like 5278, 5279 and so on ( Please update `PerfLoader`'s `appSettings.json` accordingly to reflect this ) to emulate distributed load generation. Please check [docker-compose.yml](docker-compose.yml) for more information.

Once in this directory, building images should be as straight as

`sudo docker build -t perfrunner .`

and running them as containers like

`docker compose up --no-build`

The `mcr.microsoft.com/dotnet/sdk:6.0` and `redis:alpine` images are used.

### Redis Cache manager
Redis is used as a common user store to store users and are pulled and pushed into a queue there say when one or multiple `PerfRunner`s are running.

`PerfRunner` can alone be run without the `redis` as well, in which case the user store should default to the in-memory queue and running multiple `Perfrunner`s can cause the same user to call the api's from each of the `PerfRunner`s.

### Kafka as User manager
Kafka can be used as User manager instead of the in-memory User manager. One topic for each user state need to be created which holds the users. Kfks service is loaded as a DI service suring the app startup as singleton.

To use Kafka as user manager

a. update the Kafka settings in [PerfRunner/appsettings.json](PerfRunner/appsettings.json)
b. Comment and uncomment in [PrefRunner/AppStart.cs](PrefRunner/AppStart.cs) file.

```
// services.AddSingleton<IUserManager, UserManager>();
services.AddSingleton<KafkaClientHandle>();
services.AddSingleton<KafkaDependentProducer<string, string>>();
services.AddSingleton<IUserManager, KafkaUserStore>();
```
c. Download an run Zookeeper and Kafka ( Only for local setup )

d. Create topics for each state i.e. for `perf_user_authenticated` , `perf_user_playing`, `perf_user_waiting`, `perf_user_ready` for the sample implementation.
```
bin/kafka-topics.sh --create --topic perf_user_ready --bootstrap-server loca
lhost:9092
```

### Planned features
 ....

### Existing features
1. Abitlity to create and run http test(s).
2. UI to run, view and stop test(s).
3. Ability to update rate per sec during test run.
4. Abitlity to create and run gRPC test(s).
5. Account data support with `User` object having state has well. Each user have their own state and are in the queue per state and updated by the test(s) accordingly.
6. User state management - as user queue per each state.
7. Ability to transfer data across test(s) - can be achieved by adding new data properties to the `User` object itself, say like the phone number, library card number, health number and so on. This is updated and be used by each test(s) accordingly by dequeing that state queue with the user with that updated data. Say the user just registeted at the front desk with payment, library no, his state is now authenticated and library no. is the new data and he is in the authenticated state queue. Whenever its his turn, that user gets popped from that queue.
8. Support for flag for even or uneven load distribution per second.
8a. If each action has a rate that is used for rate otherwise default to test rate.
9. Try using interface inherit implementation.
10. Monitor traffic in prometheus or Grafana cloud. Check the Grafana dashboard [json](PerfRunner/grafanaDashboard.json) to import it to the Grafana dashboard.
11. Update rate for individual action.
12. Pause/ Unpause individual action.
13. Set duration for action and test. Whichever comes first will stop first.
14. Updating duration for action will run the action for the next n update seconds since update.
15. Update distribution for each action.
16. Run, update and monitor test(s) on selected runner(s). If no runner is selected, all are probed*.
17. PerfLoader dynamically maintains the PerfRunner(s) list using [Polly](https://github.com/App-vNext/Polly) polling every minute.
19. UserManager as in memory user manager or Redis user manager or Kafka user manager.

### Planned features - Loader
1. Clean front end interface for interaction. The app needs a lot of fron end improvement yet. Basic operations such as rate, duration and distribution updates are instantanious.

### Sample Web app
For this use case, I am using the Bowling alley web app, assuming when a user goes to the ally, he has to say, get autheneticated, next wait for the lane and if lane is available, play can be initiated. For each stage, state can be represented, which is defined in `UserState`.

## Monitoring
There are several ways to monitor like below.

### CLI

The easiest way is with `dotnet-counters`. You can observe the distribution when you flip from `Even` to `Uneven` or vice - versa.

```
dotnet-counters monitor -n PerfRunner --counters PerfService.PerfRunner
```

### Prometheus
Prometheus is used to more or less display or relay the instrumentation metrics to apps like Grafana or similar. To install you may have to download and run it on your distribution. It should be as straight as `./prometheus`. The config file for it is [here](PerfRunner/prometheus.yml) or append the scrape config like below.

```
  - job_name: 'PerfRunner'
    scrape_interval: 1s # poll very quickly for a more responsive demo
    static_configs:
      - targets: ['localhost:9186']
```
Look for Perf.. in graph. You should see the graph like [here](Screens/Prometheus.png) or [this](http://localhost:9186/metrics) link should have the data from the `PerfRunner`s.

### Grafana
To run Grafana as a docker container, run.

```
sudo docker run --name=grafana -p 3000:3000 grafana/grafana-enterprise admin
```
First login should be `admin` and `admin`.

In Connections --> Data sources, install the `Prometheus` data source plugin in it and all the dashboards as well. In settings use `docker0` ip address instead of `localhost` as Grafana docker container has to see prometheus.

Update the prometheus uid in the dashboard [file](PerfRunner/grafanaDashboard.json) and import it to the dashboards. If all went well, you should see the data in the dashboard like [here](Screens/Grafana.png).

To get uid or to create a new dashboard with data shource as `Prometheus` and type Perf.. to get something like `PerfService_Runs_count` and similar to get metrics for it. In the new dashboard settings, see the json model to get the prometheus id from there.

P.S. - Make sure any test is running when you watch for the data.

### Run results
Please refer [here](Screens/300PerSecondActionLoad.png) for a sample run with upto 300 users per second with 12 core processor and having about 17% CPU usage and about 1.5 GB memory usage.

### Sample commands
To run from tests directly `CLI` with `PerfRunner` running.

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

### Issues
Please report any issues [here](https://github.com/ragavendra/PerfService/issues). This can be ranging from a minor defect to a valid feature request.

### Contribution
If you would like to contribute to thie repository, please read [CONTRIBUTING](CONTRIBUTING.md) before creating your PR.

### Customizations/ Support
<a href="https://sites.google.com/view/garden-systems" target="_blank"><img src="Garden-Systems-logos_transparent.svg" style="width:100px;height:100px"></a>

### Donations
If you like using this repository and like to donate, please visit the below link. This work is made possible with donations like yours. PM for customizations and implementations .

<a href="https://www.buymeacoffee.com/ragavendra"><img src="https://img.buymeacoffee.com/button-api/?text=Buy me a pop&emoji=🥃&slug=ragavendra&button_colour=FFDD00&font_colour=000000&font_family=Cookie&outline_colour=000000&coffee_colour=ffffff" /></a>

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ZKRHDCLG22EJA)
