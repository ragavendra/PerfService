using System.Collections.Concurrent;

namespace PerfLoader;

public static class Afce
{
    private static object _lock = new object();

    /// <summary>
    /// Removes the item in a thread safe manner.
    /// </summary>
    /// <typeparam name="T">Usually Concurrent bag only.</typeparam>
    /// <param name="bag">Exact instance of type T.</param>
    /// <param name="it">Exact item to be removed.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Adds the item if it is not already present in a thread safe manner.
    /// </summary>
    /// <typeparam name="T">Usually Concurrent bag only.</typeparam>
    /// <param name="bag">Exact instance of type T.</param>
    /// <param name="it">Exact item to be removed.</param>
    /// <returns></returns>
    public static bool AddItem<T>(this ConcurrentBag<T> bag, T it)
    {
        lock (_lock)
        {
            if (!bag.Contains(it))
            {
                bag.Add(it);
            }

            return true;
        }
    }
}