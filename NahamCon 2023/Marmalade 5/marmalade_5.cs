using NeoSmart.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Marmalade_5
{
    static class Program
    {
        public static void Main()
        {
            /* 
            Headers = {"alg": "MD5_HMAC"} = eyJhbGciOiJNRDVfSE1BQyJ9
            Payload = {"username": "test"} = eyJ1c2VybmFtZSI6InRlc3QifQ
            Signature = "3R1XbK5O2t6MZ0ir6KJdRw"
            secret fsrwjcfszeg*****
            */

            // crunch 5 5 -f /usr/share/crunch/charset.lst lalpha -o crunch.lst
            List<string> file = File.ReadAllLines(@"D:\\vm-share\\crunch.lst").ToList();

            var part = file.Partition(1000);
            var c = part.Count();

            var p = Parallel.For(0, c, (i, Run) =>
            {
                var hPayload = "eyJhbGciOiJNRDVfSE1BQyJ9.eyJ1c2VybmFtZSI6InRlc3QifQ";
                var start = "fsrwjcfszeg";

                foreach (string end in part[i])
                {
                    var secret = $"{start}{end}";
                    var sBytes = Encoding.UTF8.GetBytes(secret.ToString());
                    var pBytes = Encoding.UTF8.GetBytes(hPayload.ToString());
                    var md5 = new HMACMD5(sBytes);
                    var hashBytes = md5.ComputeHash(pBytes);
                    string sign = UrlBase64.Encode(hashBytes);
                    
                    if (sign == "3R1XbK5O2t6MZ0ir6KJdRw")
                    {
                        // var secret = "fsrwjcfszegvsyfa";
                        Console.WriteLine($"[+] secret: {secret}");

                        // Payload = {"username": "admin"} = eyJ1c2VybmFtZSI6ImFkbWluIn0
                        hPayload = "eyJhbGciOiJNRDVfSE1BQyJ9.eyJ1c2VybmFtZSI6ImFkbWluIn0";

                        sBytes = Encoding.UTF8.GetBytes(secret.ToString());
                        pBytes = Encoding.UTF8.GetBytes(hPayload.ToString());
                        md5 = new HMACMD5(sBytes);
                        hashBytes = md5.ComputeHash(pBytes);
                        sign = UrlBase64.Encode(hashBytes);

                        Console.WriteLine($"[+] Token admin: {hPayload}.{sign}");

                        Run.Stop();
                        return;
                    }
                }
            });

            Console.ReadKey();
        }

        public static List<List<T>> Partition<T>(this List<T> values, int chunkSize)
        {
            return values.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
