using System;

namespace Server
{
    static class Program
    {
        static void Main(string[] args)
        {
            ServerSocket server = new ServerSocket();
            server.Bind(1029);
            server.Listen(5);
            server.Accept();
            while (true)
                Console.ReadKey();
        }
    }
}
