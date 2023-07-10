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
         /*
         Entity_ entity_ = new Entity_(new Entity(){ value = 06 });
         entity_.AddLast(10);
         entity_.AddLast(60);
         entity_.AddFirst(3);
         entity_.AddLast(1);
         entity_.AddAfter(6, 12);
         entity_.AddBefore(6, 8);
         // Console.WriteLine(entity_.value);
         entity_.Display();*/
         // entity_.Display();

         var resp = SortInQueue(new int[] { 10, 3, 24, 15 });

         foreach (var item in resp)
         {
            Console.Write(item + " "); 
         }
      }

      record Entity
      {
         public int value;
         public Entity next;

      }

      class Entity_
      {
         private Entity _entity;

         private Entity _firstEntity;

         private Entity _lastEntity;

         public Entity_(Entity entity)
         {
            _entity = entity;
            _firstEntity = entity;
         }

         public bool AddLast(int ele)
         {
            var entity = _entity;
            while(entity.next != null)
            {
               entity = entity.next;   
            }
 
            entity.next = new Entity() { value = ele, next = null };

            _lastEntity = entity;

            return true;
         }

         public bool AddFirst(int ele)
         {
            var entity = new Entity(){ value = ele, next = _entity };
            _entity = entity;
            return true;
         }

         public int First()
         {
            return _entity.value;
         }

         public int Last()
         {
            return _lastEntity.value;
         }

         public void Display()
         {
            Console.WriteLine("Elements are - ");

            var entity = _entity;

            do {
               Console.Write(entity.value + " ");
               // entity.next.value = entity.value;
               entity = entity.next;   
            }
            while(entity != null);
         }

         // add after ele the value value_
         public bool AddAfter(int ele, int value_)
         {
            var entity = _entity;

            // find the ele
            while(entity.value != ele)
            {
               entity = entity.next;   
            }

            if(entity.value == ele)
            {

               Entity newEntity = new Entity() { value = value_, next = entity.next };
               entity.next = newEntity;
               // _entity = entity;

            }
            else
            {
               Console.WriteLine(ele + " not found!");
            }

            return true;
         }

         // add before ele the value value_
         public bool AddBefore(int ele, int value_)
         {
            var entity = _entity;
            var last = entity;

            // find the ele
            while(entity.value != ele)
            {
               last = entity;
               entity = entity.next;   
            }

            if(entity.value == ele)
            {

               Entity newEntity = new Entity() { value = value_, next = entity };
               last.next = newEntity;
               // entity.next = newEntity;
               // _entity = entity;

            }
            else
            {
               Console.WriteLine(ele + " not found!");
            }

            return true;
         }

         public Entity_ MoveIndexNext()
         {
            var entity_ = _entity;
            if(entity_.next != null)
            {
               entity_ = entity_.next;
            }

            return new Entity_(entity_);
         }


      }

      public static int[] SortInQueue(int[] numbers)
      {
         // 3 times size of un sorted array
         // int[] sortedNos = new int[numbers.Length * 3];
         // LinkedList<int> sortedNos = new LinkedList<int>();
         Entity_ sortedList = new Entity_(new Entity(){ value = 0 });

         // list.value = 1;
         // list.next = new Entity();

         // int mid = numbers.Length/2;

         Parallel.ForEach(
            numbers,
            number =>
            {
               if (number <= sortedList?.First())
               {  
                  sortedList.AddFirst(number);
                  return;
               }

               if (number > sortedList?.Last())
               {
                  sortedList.AddLast(number);
                  return;
               }

               // insert between
               if (number > sortedList?.First())
               {
                  if(number <= sortedList.Last())
                  {
                     if (number == sortedList.Last())
                     {
                        sortedList.AddLast(number);
                     }
                     else
                     {
                        var sortedList_ = sortedList;
                        var first = sortedList_.First();

                        // sortedList.AddAfter(, number);
                        do
                        {
                           sortedList_ = sortedList_.MoveIndexNext();
                           first = sortedList_.First();
                        }while(first <= number);

                        sortedList_.AddAfter(first, number);
                     }
                  }
                  // move to next ele

                  // sortedNos.AddAfter(3, number);
          //        sortedList.AddFirst(number);
                  return;
               }


            });

            var sortedList1 = sortedList;
            int[] result = new int[numbers.Length];

            for (int i = 0; i < numbers.Length; i++)
            {
               
              int item_ = sortedList1.First();
              sortedList1 = sortedList1.MoveIndexNext();
              result[i] = item_;
            }

            return result;
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder.UseStartup<AppStart>());

   }
}