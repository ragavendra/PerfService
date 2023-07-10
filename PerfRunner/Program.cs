namespace PerfRunner
{
   using System.Collections.Concurrent;

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


         // CreateHostBuilder(args).Build().Run();
         Entity entity = new Entity();
         entity.AddFirst(10);
         entity.AddFirst(60);
         // entity.AddLast(70);
         Console.WriteLine(entity.value);
         entity.Display();
      }

      record Entity_
      {
         public int value;
         public Entity_ next;

      }

      class Entity
      {
         public bool AddLast(int ele, Entity_ entity)
         {
            while(entity.next != null)
            {
               // Console.Write(value + " ");
               entity.next.value = value;
               next = next.next;   
            }
 
            Entity next_ = new Entity() { value = ele, next = null };
            next = next_;

            return true;
         }

         public bool AddFirst(int ele)
         {
            var entity = new Entity(){ value = ele, next = this };
            Display();
            return true;
         }

         public void Display()
         {
            Console.WriteLine("Elements are - ");

            while(next != null)
            {
               Console.Write(value + " ");
               next.value = value;
               next = next.next;   
            }
         }

         // add after ele the value value_
         public bool AddAfter(int ele, int value_)
         {
            // find the ele
            while(ele != value)
            {
               next.value = value;
               next = next.next;   
            }

            var entity = new Entity(){ value = value_ };

            // point next
            entity.next = this.next;

            next = entity;

            return true;
         }

         // add before ele the value value_
         public bool AddBefore(int ele, int value_)
         {
            var entity = new Entity(){ value = value_ };

            // point next
            entity.next = this.next;

            next = entity;

            return true;
         }
      }

      public static void SortInQueue(int[] numbers)
      {
         // 3 times size of un sorted array
         // int[] sortedNos = new int[numbers.Length * 3];
         LinkedList<int> sortedNos = new LinkedList<int>();
         Entity sortedList = new Entity();

         // list.value = 1;
         // list.next = new Entity();

         int mid = numbers.Length/2;

         Parallel.ForEach(
            numbers,
            number =>
            {
               if (number <= sortedNos?.First())
               {
                  // sortedNos.AddFirst(number);
                  sortedList.AddFirst(number);
                  return;
               }
               else
               {
                  // sortedNos.AddLast(number);
               }

               if (number > sortedNos?.Last())
               {
                  // sortedNos.AddLast(number);
                  sortedList.AddLast(number);
                  return;
               }

               if (number > sortedNos?.First())
               {
                  // move to next ele

                  // sortedNos.AddAfter(3, number);
                  sortedList.AddFirst(number);
                  return;
               }


            });





      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder.UseStartup<AppStart>());

   }
}