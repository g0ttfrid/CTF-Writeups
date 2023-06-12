using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public static class Program
    {
        public static void Main()
        {
            WebClient client = new WebClient();
            string r = client.DownloadString("http://challenges.ringzer0team.com:10032/");

            Regex pattern = new Regex(@"----- BEGIN MESSAGE -----<br \/>\n        (.*) = \?<br />");
            Match match = pattern.Match(r);
            var m = match.Groups[1].Value;
            List<string> list = m.Split(' ').ToList();

            int value1 = Convert.ToInt32(list[0]);
            int value2 = Convert.ToInt32(list[2], 16);
            int value3 = Convert.ToInt32(list[4], 2);
            int result = value1 + value2 - value3;

            r = client.DownloadString($"http://challenges.ringzer0team.com:10032/?r={result}");

            Regex pattern1 = new Regex(@"FLAG-[0-9a-zA-Z]{24}");
            Match flag = pattern1.Match(r);
            Console.WriteLine(flag.Success ? flag.Value : "Not Flag");
            Console.ReadLine();
        }
    }
}