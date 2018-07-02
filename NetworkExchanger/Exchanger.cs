using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;


namespace NetExchanger
{
    public class Exchanger : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private BinaryFormatter _formatter;

        public bool Connected => _client.Connected;

        public Exchanger(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
            _formatter = new BinaryFormatter();
        }

        public void SendData(ExchangeData data)
        {
            _formatter.Serialize(_stream, data);
        }

        public ExchangeData ReceiveData()
        {
            return (ExchangeData)_formatter.Deserialize(_stream);
        }

        public void Dispose()
        {
            _client?.Client.Dispose();
        }
    }
}
