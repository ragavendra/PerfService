namespace PerfRunner
{
    public static class Program
   {
      /// <summary>
      /// PE for the Perf Runner or Srvc.
      /// </summary>
      public static async Task Main(string[] args)
      {
        /*
         var logDirName = $"{DateTime.UtcNow:yyyy.MM.dd_HH.mm.ss}";
         var logOutputDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "logs", logDirName));
         if (!Directory.Exists(logOutputDir))
         {
            Directory.CreateDirectory(logOutputDir);
         }*/

         CreateHostBuilder(args).Build().Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder.UseStartup<AppStart>());

   }
}