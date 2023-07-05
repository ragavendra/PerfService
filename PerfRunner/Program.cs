namespace PerfRunner
{
   using System.Threading.Tasks.Dataflow;
   using PerfRunner.Tests;

   public static class Program
   {
      /*
      /// <summary>
      /// PE for the Perf Runner or Srvc.
      /// </summary>
      public static async Task Main(string[] args)
      {


         CreateHostBuilder(args).Build().Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder.UseStartup<AppStart>());*/

      public static void Main()
      {
         Action<string> action = SomeFunc;

         var type = typeof(ITestBase);
         var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));

         Type actionType = null;
         string action_ = "Login";

         // check if the actions exist
            foreach (var type_ in types)
            {
               if(type_.FullName.ToLowerInvariant().EndsWith("." + action_))
               {
                  actionType = type_;
               } 
            }

         // action = typeof(Login).GetMembers().First();
         // var res = typeof("Login");
         var name = typeof(ITestBase).GetMembers().First().Name;
         // var name = typeof(ITestBase).GetNestedTypes
         // action = ITestBase.RunTest_;
         var actionBlock = new ActionBlock<int>(

                     // Simulate work by suspending the current thread.
                     // testBase => testBase.RunTest(Guid, _logger),
                     // test => test.RunTest_("some"),
                     // test => test.RunTest__("some"),
                     // test => test.ToLower(),
                     act => Thread.Sleep(act),

                     // Specify a maximum degree of parallelism.
                     new ExecutionDataflowBlockOptions
                     {
                        MaxDegreeOfParallelism = Environment.ProcessorCount
                     }
                        );

         static void SomeFunc(string someStr)
         {
            Console.WriteLine("Hello is " + someStr);

         }

         // actionBlock.Post(new Login());
         Console.WriteLine(actionBlock.Post(3000));

      }

   }

}