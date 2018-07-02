using System;
using System.Threading;
using System.Net.Sockets;
using NetExchanger;

namespace NetClient
{
    public class Client
    {
        private string _host;
        private int _port;

        public Client(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public void Run()
        {
            try
            {
                using (var exchanger = new Exchanger(new TcpClient(_host, _port)))
                {
                    Console.WriteLine("Соединение установлено");
                    var thread = new Thread(() =>
                    {
                        try
                        {
                            while (exchanger.Connected)
                            {
                                var array = exchanger.ReceiveData().Data;
                                ShowArray(array, true);
                                Console.Write("Нажмите <Enter> для продолжения....");
                            }
                        }
                        catch
                        {
                            Console.WriteLine();
                            Console.WriteLine("Ошибка сериализации/обрыв связи с сервером");
                            exchanger?.Dispose();
                        }
                    });
                    thread.Start();
                    while (exchanger.Connected)
                    {
                        var isOk = false;
                        byte size = 0;
                        while (!isOk)
                        {
                            Console.Write($"Введите размерность массива (от {byte.MinValue + 1} до {byte.MaxValue}):");
                            try
                            {
                                size = Convert.ToByte(Console.ReadLine());
                                if (size == 0)
                                    throw new ArgumentException();
                                isOk = true;
                            }
                            catch
                            {
                                Console.WriteLine("Неверное значение размерности!!!");
                                isOk = false;
                            }
                        }
                        var array = GenerateArray(size);
                        ShowArray(array, false);
                        try
                        {
                            exchanger.SendData(new ExchangeData(array));
                            Console.ReadLine();
                        }
                        catch
                        {
                            Console.WriteLine();
                            Console.WriteLine("Ошибка сериализации/обрыв связи с сервером");
                            exchanger?.Dispose();
                        }
                    }
                    thread.Join();
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Ошибка соединения/сервер не доступен");
            }
        }

        private static int[] GenerateArray(byte arraySize)
        {
            var array = new int[arraySize];
            var rnd = new Random(DateTime.Now.Millisecond);
            const int min = -1000;
            const int max = 1000;
            for (byte i = 0; i < arraySize; i++)
                array[i] = rnd.Next(min, max);
            return array;
        }

        private static void ShowArray(int[] array, bool isResult)
        {
            Console.WriteLine();
            if (isResult)
                Console.WriteLine("Результирующий массив:");
            else
                Console.WriteLine("Исходный массив:");
            Console.WriteLine("----------------------------------------------");
            if (array != null && array.Length > 0)
            {
                foreach (var element in array)
                    Console.Write(element.ToString() + " ");
                Console.WriteLine();
            }
            else
                Console.WriteLine("Пустой!!!!");
            Console.WriteLine("----------------------------------------------");
        }
    }
}
