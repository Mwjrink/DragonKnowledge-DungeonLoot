using System;
using System.Collections.Concurrent;
using System.Threading;

namespace DKDG.Utils
{
    public static class ThreadPool
    {
        private static readonly Thread[] pool;
        internal static AutoResetEvent are;
        internal static ConcurrentQueue<Action> tasx;

        static ThreadPool()
        {
            are = new AutoResetEvent(false);
            int poolSize = 10;
            pool = new Thread[poolSize];
            for (int i = 0; i < poolSize; i++)
                pool[i] = new Thread(() =>
                {
                    while (true)
                        if (!tasx.TryDequeue(out Action current))
                            are.WaitOne();
                        else
                            current();
                });
        }

        public static void Run(Action a)
        {
            tasx.Enqueue(a);
            are.Set();
        }
    }
}
