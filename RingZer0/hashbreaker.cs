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
            // create client e get webpage
            WebClient client = new WebClient();
            string r = client.DownloadString("http://challenges.ringzer0team.com:10056/");

            // create pattern for get HASH using regex
            Regex pattern = new Regex(@"----- BEGIN HASH -----<br \/>\n        (.*)<br />");
            Match match = pattern.Match(r);
            var m = match.Groups[1].Value;

            string result = "";

            // using Parallel to generate a list of pins simultaneously 
            var p = Parallel.For(1000, 10000, (i, Run) =>
            {
                // encoding pin (i) to SHA1
                var sourceBytes = Encoding.ASCII.GetBytes(i.ToString());
                var sha = new SHA1Managed();
                var hashBytes = sha.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLower();

                // comparing the encoded pin with the hash
                // exit after finding the result
                if (hash == m)
                {
                    Console.WriteLine($"{hash} > {i}");
                    result = i.ToString();
                    Run.Break();
                }
            });

            // sending the answer back 
            r = client.DownloadString($"http://challenges.ringzer0team.com:10056/?r={result}");

            // print FLAG
            Regex pattern1 = new Regex(@"FLAG-[0-9a-zA-Z]{24}");
            Match flag = pattern1.Match(r);
            Console.WriteLine(flag.Success ? flag.Value : "Not Flag");

            Console.ReadKey();
        }
    }
}
