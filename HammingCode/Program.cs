using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HammingCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var line = Console.ReadLine();
            var codeHammingCode = ToCodeHammingCode(line);
            var decodeHammingCode = ToDecodeHammingCode(codeHammingCode);
            Console.Out.WriteLine(codeHammingCode);
            Console.Out.WriteLine(decodeHammingCode);
        }

        static string ToCodeHammingCode(string message)
        {
            var infoMessage = GetBinary(message, 8);
            
            var stringBuilderToMedMessage = new StringBuilder();

            var indexMedMessage = 0;
            for (var i = 0; i < infoMessage.Length; indexMedMessage++)
            {
                string value;
                if (IsPowerOfTwo(indexMedMessage + 1))
                {
                    value = "0";
                }
                else
                {
                    value = infoMessage[i].ToString();
                    i++;
                }

                stringBuilderToMedMessage.Append(value);
            }

            var amountOfControlBit = (int)Math.Sqrt(stringBuilderToMedMessage.Length) + 1;
            for (var indexControlBit = 0; indexControlBit < amountOfControlBit; indexControlBit++)
            {
                var count = 0;
                var stringBuilderToNumberOfElement = new StringBuilder();
                //Формируем строчку в таблице
                for (var i = 0; i < stringBuilderToMedMessage.Length; i++)
                {
                    var binIndex = string.Concat(GetBinary(i + 1, amountOfControlBit).Reverse().ToArray());
                    stringBuilderToNumberOfElement.Append(binIndex[indexControlBit]);
                }
                //Сравниваем строчку с сообщением
                for (var i = 0; i < stringBuilderToMedMessage.Length; i++)
                {
                    if (stringBuilderToMedMessage[i] == '1' && stringBuilderToNumberOfElement[i] == '1')
                    {
                        count++;
                    }
                }
                

                var rValue = (count % 2).ToString();
                if (rValue == "1")
                {
                    var pow = (int)Math.Pow(2, indexControlBit);
                    stringBuilderToMedMessage[pow-1] = '1';
                }
            }

            return stringBuilderToMedMessage.ToString();
        }

        static string ToDecodeHammingCode(string bits)
        {
            var amountOfControlBit = (int)Math.Sqrt(bits.Length) + 1;
            var stringBuilderBits = new StringBuilder(bits);
            
            for (var indexControlBit = 0; indexControlBit < amountOfControlBit; indexControlBit++)
            {
                var count = 0;
                var stringBuilderToNumberOfElement = new StringBuilder();
                //Формируем строчку в таблице
                for (var i = 0; i < stringBuilderBits.Length; i++)
                {
                    var binIndex = string.Concat(GetBinary(i + 1, amountOfControlBit).Reverse().ToArray());
                    stringBuilderToNumberOfElement.Append(binIndex[indexControlBit]);
                }
                //Сравниваем строчку с сообщением
                for (var i = 0; i < stringBuilderBits.Length; i++)
                {
                    if (stringBuilderBits[i] == '1' && stringBuilderToNumberOfElement[i] == '1')
                    {
                        count++;
                    }
                }
                

                var rValue = (count % 2).ToString();
                if (rValue == "1")
                {
                    Console.Out.WriteLine("Received broken message");
                    return string.Empty;
                }
            }
            
            for (var i = amountOfControlBit - 1; i >= 0; i--)
            {
                var index = (int) Math.Pow(2, i) - 1;
                stringBuilderBits.Remove(index, 1);
            }


            var decodeHammingCode = stringBuilderBits.ToString();
            
            var substring = GetBytesFromBinaryString(decodeHammingCode);
            var value = Encoding.ASCII.GetString(substring);

            return value;
        }

        static string GetBinary(int value)
        {
            var resultStr = string.Empty;
            for (var i = 0; value > 0; i++)
            {
                resultStr = value % 2 + resultStr;
                value /= 2;
            }

            return resultStr;
        }
        
        static string GetBinary(int value, int amountOfBits)
        {
            var resultStr = string.Empty;
            for (var i = 0; value > 0; i++)
            {
                resultStr = value % 2 + resultStr;
                value /= 2;
            }

            while (resultStr.Length < amountOfBits)
            {
                resultStr = "0" + resultStr;
            }
            
            return resultStr;
        }

        static string GetBinary(string value)
        {
            var stringBuilder = new StringBuilder();
            foreach (var letter in value)
            {
                stringBuilder.Append(GetBinary(letter));
            }

            return stringBuilder.ToString();
        }
        
        static string GetBinary(string value, int length)
        {
            var stringBuilder = new StringBuilder();
            foreach (var letter in value)
            {
                stringBuilder.Append(GetBinary(letter, length));
            }

            return stringBuilder.ToString();
        }
        
        static bool IsPowerOfTwo(int val)
        {
            return val != 0 && (val & (val - 1)) == 0;
        }
        
        static byte[] GetBytesFromBinaryString(string binary)
        {
            var list = new List<byte>();

            for (var i = 0; i < binary.Length; i += 8)
            {
                var t = binary.Substring(i, 8);

                list.Add(Convert.ToByte(t, 2));
            }

            return list.ToArray();
        }
    }
}