using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public static class Program
    {

        public static void Main()
        {
            // create client and get webpage
            WebClient client = new WebClient();
            string r = client.DownloadString("http://challenges.ringzer0team.com:10013/");

            // create pattern for get MESSAGE using regex
            Regex pattern = new Regex(@"[A-Za-z0-9]{1024}");
            Match match = pattern.Match(r);

            // encoding MESSAGE to SHA512
            var sourceBytes = Encoding.ASCII.GetBytes(match.ToString());
            var sha = new SHA512Managed();
            var hashBytes = sha.ComputeHash(sourceBytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLower();

            // sending the answer back
            r = client.DownloadString($"http://challenges.ringzer0team.com:10013/?r={hash}");

            // print FLAG
            Regex pattern1 = new Regex(@"FLAG-[0-9a-zA-Z]{26}");
            Match flag = pattern1.Match(r);
            Console.WriteLine(flag.Success ? flag.Value : "Not Flag");

            Console.ReadLine();
        }
    }
}
