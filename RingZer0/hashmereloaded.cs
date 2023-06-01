using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public static class Program
    {

        public static string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();
            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byteList.ToArray());
        }

        public static void Main()
        {
            // create client e get webpage
            WebClient client = new WebClient();
            string r = client.DownloadString("http://challenges.ringzer0team.com:10014/");

            // create pattern for get MESSAGE using regex
            Regex pattern = new Regex(@"[0-1]{8192}");
            Match match = pattern.Match(r);

            // convert binary to string
            var str = BinaryToString(match.ToString());

            // encoding MESSAGE to SHA512
            var sourceBytes = Encoding.UTF8.GetBytes(str);
            var sha = new SHA512Managed();
            var hashBytes = sha.ComputeHash(sourceBytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLower();

            // sending the answer back 
            r = client.DownloadString($"http://challenges.ringzer0team.com:10014/?r={hash}");

            // print FLAG
            Regex pattern1 = new Regex(@"FLAG-[0-9a-zA-Z]{26}");
            Match flag = pattern1.Match(r);
            Console.WriteLine(flag.Success ? flag.Value : "Not Flag");
            Console.ReadLine();
        }
    }
}
