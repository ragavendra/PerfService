// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
// using GrpcGreeterClient;
using PerfRunner;
using Microsoft.Extensions.DependencyInjection;
using PerfRunner.V1;
using System.Threading.Tasks.Dataflow;
using System.Diagnostics;

/* data flow block post
// Performs several computations by using dataflow and returns the elapsed
// time required to perform the computations.
static TimeSpan TimeDataflowComputations(int maxDegreeOfParallelism,
      int messageCount)
   {
      // Create an ActionBlock<int> that performs some work.
      var workerBlock = new ActionBlock<int>(
         // Simulate work by suspending the current thread.
         millisecondsTimeout => Thread.Sleep(millisecondsTimeout),
         // Specify a maximum degree of parallelism.
         new ExecutionDataflowBlockOptions
         {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
         });

      // Compute the time that it takes for several messages to
      // flow through the dataflow block.

      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      bool flag;

      for (int i = 0; i < messageCount; i++)
      {
         flag = workerBlock.Post(1000);
      }
      workerBlock.Complete();

      // Wait for all messages to propagate through the network.
      workerBlock.Completion.Wait();

      // Stop the timer and return the elapsed number of milliseconds.
      stopwatch.Stop();
      return stopwatch.Elapsed;
   }

int processorCount = Environment.ProcessorCount;
int messageCount = processorCount;

// Print the number of processors on this computer.
Console.WriteLine("Processor count = {0}.", processorCount);

TimeSpan elapsed;

// Perform two dataflow computations and print the elapsed
// time required for each.

// This call specifies a maximum degree of parallelism of 1.
// This causes the dataflow block to process messages serially.
elapsed = TimeDataflowComputations(1, messageCount);
Console.WriteLine("Degree of parallelism = {0}; message count = {1}; " +
   "elapsed time = {2}ms.", 1, messageCount, (int)elapsed.TotalMilliseconds);

// Perform the computations again. This time, specify the number of
// processors as the maximum degree of parallelism. This causes
// multiple messages to be processed in parallel.
elapsed = TimeDataflowComputations(processorCount, messageCount);
Console.WriteLine("Degree of parallelism = {0}; message count = {1}; " +
   "elapsed time = {2}ms.", processorCount, messageCount, (int)elapsed.TotalMilliseconds);
*/

/*
// static
var factory = new StaticResolverFactory( addr => new[]
{
    new BalancerAddress("localhost", 80),
    new BalancerAddress("localhost", 82)
}
);*/

/*
var services = new ServiceCollection();

// dns resolver
services.AddSingleton<ResolverFactory>(
    sp => new DnsResolverFactory(refreshInterval: TimeSpan.FromSeconds(30)));
*/

// static resolver
// services.AddSingleton<ResolverFactory>(factory);

// var host_ = "https://localhost:7233";
var host_ = "http://localhost:5277";
using var channel = GrpcChannel.ForAddress(host_);

/*
// dns resolver
using var channel = GrpcChannel.ForAddress(
    "dns:///sample-host",
    new GrpcChannelOptions { Credentials = Grpc.Core.ChannelCredentials.Insecure,
    ServiceProvider = services.BuildServiceProvider(),
    ServiceConfig = new Grpc.Net.Client.Configuration.ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } } );
*/

/*
var cli = new Greeter.GreeterClient(channel);
var rep = await cli.SayHelloAsync(new HelloRequest { Name = "GreetCli" });
*/

/*
var cli = new Perf.PerfClient(channel);

var rep = await cli.PingAsync(new PingRequest { Name = "PingCli" });
// var rep = cli.Ping(new PerfRunner.PingRequest { Name = "PingCli" });
*/

/*
var cli = new Perf.PerfClient(channel);

var testRequest = new TestRequest { Name = "FirstTest", Guid = Guid.NewGuid().ToString(), Rate = 3 };
testRequest.Actions.Add(new ActionOption(){Name = "Login"});

// not wait for task to complete
var rep = cli.RunTestAsync(testRequest);

// sleep 10 secs
Thread.Sleep(3_000);

var testRequest2 = new TestRequest { Name = "SecondTest", Guid = Guid.NewGuid().ToString(), Rate = 7 };
testRequest2.Actions.Add(new ActionOption(){Name = "TestBase"});

// not wait for task to complete
var rep2 = cli.RunTestAsync(testRequest2);

// sleep 10 secs
Thread.Sleep(3_000);

// var rep_ = await cli.StopAllTestsAsync(new StopAllTestsRequest {} );
var rep_ = await cli.StopTestAsync(new StopTestRequest { Guid = testRequest.Guid } );

// sleep 10 secs
Thread.Sleep(3_000);

// try again
var rep_a = await cli.UpdateRateAsync(new UpdateRateRequest { Guid = testRequest2.Guid, Rate = 6 } );

// sleep 10 secs
Thread.Sleep(3_000);

// try again
var rep_1 = await cli.StopTestAsync(new StopTestRequest { Guid = testRequest2.Guid } );

// Console.WriteLine("Greeting: " + rep.Message);
Console.WriteLine("Stop Test Resp: " + rep_.Status);
// Console.ReadKey();
*/
/*
Func<string, int> someFunc;

int thisMethod(string name)
{
    return name.Length;
}

someFunc = thisMethod;
var res = someFunc("strr");
*/