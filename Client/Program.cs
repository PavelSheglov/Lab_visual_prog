namespace NetClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client("localhost", 25000);
            client.Run();
        }
    }
}
