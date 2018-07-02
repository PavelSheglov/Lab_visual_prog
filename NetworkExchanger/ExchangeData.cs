using System;

namespace NetExchanger
{
    [Serializable]
    public class ExchangeData
    {
        public int[] Data { get; private set; }

        public ExchangeData(int[] array)
        {
            Data = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
                Data[i] = array[i];
        }
    }
}
