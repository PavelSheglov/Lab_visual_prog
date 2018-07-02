using System.Net;

namespace NetServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(IPAddress.Any, 25000);
            server.Run();
        }
    }
}
