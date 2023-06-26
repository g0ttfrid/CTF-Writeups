using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace hash_breaker_reloaded
{
    class Program
    {
        static void Main()
        {
            // create client e get webpage
            WebClient client = new WebClient();
            string r = client.DownloadString("http://challenges.ringzer0team.com:10057/");

            // create pattern for get HASH & SALT using regex
            Regex patternHash = new Regex(@"----- BEGIN HASH -----<br \/>\n        (.*)<br />");
            Regex patternSalt = new Regex(@"----- BEGIN SALT -----<br \/>\n        (.*)<br />");
            Match phash = patternHash.Match(r);
            Match psalt = patternSalt.Match(r);
            var h = phash.Groups[1].Value;
            var s = psalt.Groups[1].Value;

            string result = "";

            // using Parallel to generate a list of pins simultaneously 
            var p = Parallel.For(1000, 10000, (i, Run) =>
            {
                // encoding pin+salt to SHA1
                string secret = $"{i}{s}";
                var sourceBytes = Encoding.ASCII.GetBytes(secret);
                var sha = new SHA1Managed();
                var hashBytes = sha.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLower();

                // comparing the encoded pin with the hash
                // exit after finding the result
                if (hash == h)
                {
                    Console.WriteLine("HASH:SALT:RESULT");
                    Console.WriteLine($"{hash}:{s}:{i}");
                    result = i.ToString();
                    Run.Break();
                }
            });

            // sending the answer back 
            r = client.DownloadString($"http://challenges.ringzer0team.com:10057/?r={result}");

            // print FLAG
            Regex pattern1 = new Regex(@"FLAG-[0-9a-zA-Z]{24}");
            Match flag = pattern1.Match(r);
            Console.WriteLine(flag.Success ? flag.Value : "Not Flag");

            Console.ReadKey();
        }
    }
}
