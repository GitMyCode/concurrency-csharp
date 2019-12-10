using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace Synchronization
{
    class Program
    {
        public static Random rnd = new Random();
        public static ImmutableHashSet<int> set = ImmutableHashSet.Create<int>();
        private int value = 10;
        static void Main(string[] args)
        {
            Task.Run(async () => await MainAsync(args)).GetAwaiter().GetResult();
            // int worker = 0;
            // int io = 0;
            // ThreadPool.GetAvailableThreads(out worker, out io);
            // Console.WriteLine("Thread pool threads available at startup: ");
            // Console.WriteLine("   Worker threads: {0:N0}", worker);
            // Console.WriteLine("   Asynchronous I/O threads: {0:N0}", io);
            // Console.Read();
            Console.WriteLine("Count threads:  " + set.Count());
        }

        static async Task MainAsync(string[] args)
        {
            var program = new Program();
            // var threads = new List<Thread>();
            var tasks = Enumerable.Range(1, 500).Select(x => program.MyMethodAsync());

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        async Task MyMethodAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(rnd.Next(1, 5)));
            Console.WriteLine("Start ");
            Thread thread = Thread.CurrentThread;
            var msg =  String.Format("   Background: {0}\n", thread.IsBackground) +
               String.Format("   Thread Pool: {0}\n", thread.IsThreadPoolThread) +
               String.Format("   Thread ID: {0}\n", thread.ManagedThreadId);
            Console.WriteLine(msg);
            set = set.Add(thread.ManagedThreadId);
            //Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(1, 5)));
            int originalValue = value;
            await Task.Delay(TimeSpan.FromSeconds(rnd.Next(1, 5)));
            //Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(1, 5)));
            originalValue = originalValue + 1;
            //Thread.Sleep(TimeSpan.FromSeconds(rnd.Next(1, 5)));
            value = originalValue - 1;
            await Task.Delay(TimeSpan.FromSeconds(rnd.Next(1, 5)));
            if (value != 10)
            {
                Console.WriteLine(value);
            }
        }
    }
}
