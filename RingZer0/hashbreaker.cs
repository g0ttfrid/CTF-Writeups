using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace hash_breaker
{
    class Program
    {
        static void Main()
        {
            WebClient client = new WebClient();
            string r = client.DownloadString("http://challenges.ringzer0team.com:10056/");

            Regex pattern = new Regex(@"----- BEGIN HASH -----<br \/>\n        (.*)<br />");
            Match match = pattern.Match(r);
            var m = match.Groups[1].Value;

            string result = "";
            var p = Parallel.For(1000, 10000, (i, Run) =>
            {
                var sourceBytes = Encoding.ASCII.GetBytes(i.ToString());
                var sha = new SHA1Managed();
                var hashBytes = sha.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLower();

                if (hash == m)
                {
                    Console.WriteLine($"{hash} > {i}");
                    result = i.ToString();
                    Run.Break();
                }
            });

            r = client.DownloadString($"http://challenges.ringzer0team.com:10056/?r={result}");

            Regex pattern1 = new Regex(@"FLAG-[0-9a-zA-Z]{24}");
            Match flag = pattern1.Match(r);
            Console.WriteLine(flag.Success ? flag.Value : "Not Flag");

            Console.ReadKey();
        }
    }
}
