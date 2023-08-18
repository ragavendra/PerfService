using System.Collections.Concurrent;

namespace PerfLoader;

public static class Afce
{
    private static object _lock = new object();

    public static bool RemoveItem<T>(this ConcurrentBag<T> bag, T it)
    {
        lock (_lock)
        {
            List<T> grpcChannels = bag.ToList();
            bool flag = false;
            flag = grpcChannels.Remove(it);

            bag.Clear();

            foreach (var item in grpcChannels)
            {
                bag.Add(item);
            }
            return flag;
        }
    }
}