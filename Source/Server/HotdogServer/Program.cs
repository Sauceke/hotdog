using System;

namespace HotdogServer
{
    internal class Program
    {
        private const int defaultPort = 5365;

        public static void Main(string[] args)
        {
            int port = args.Length > 0 ? int.Parse(args[0]) : defaultPort;
            var server = new HotdogServer();
            server.Start(port);
            Console.WriteLine($"Hotdog server running on port {port}.");
            Console.ReadKey(true);
            server.Stop();
        }
    }
}
