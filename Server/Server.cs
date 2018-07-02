using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using NetExchanger;

namespace NetServer
{
    public class Server
    {
        private IPAddress _ip;
        private int _port;
        private TcpListener _listener;

        public Server(IPAddress ip, int port)
        {
            _ip = ip;
            _port = port;
            _listener = new TcpListener(ip, port);
        }

        public void Run()
        {
            _listener.Start();
            Console.WriteLine("Сервер запущен");
            try
            {
                while (true)
                {
                    Console.WriteLine("Ожидание подключения.......");
                    var client = _listener.AcceptTcpClient();
                    var thread = new Thread(() =>
                    {
                        ServeClient(new Exchanger(client));
                    });
                    thread.IsBackground = true;
                    thread.Start();
                    Console.WriteLine("Клиент подключился");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка в работе сервера");
            }
            finally
            {
                _listener.Stop();
                Console.WriteLine("Сервер остановлен");
            }
        }
        
        private static void ServeClient(Exchanger exchanger)
        {
            try
            {
                while (exchanger.Connected)
                {
                    var array = exchanger.ReceiveData().Data;
                    Console.WriteLine("Получен от клиента исходный массив");
                    Array.Reverse(array);
                    Console.WriteLine("Массив перевернут");
                    var data = new ExchangeData(array);
                    exchanger.SendData(data);
                    Console.WriteLine("Перевернутый массив отправлен клиенту");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка соединения/клиент отсоединился ");
            }
            finally
            {
                exchanger.Dispose();
            }
        }
    }
}
