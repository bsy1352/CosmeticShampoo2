using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            string msg = "avbd$daadfgcd";
            string header = msg.Substring(0, msg.IndexOf("$"));
            msg = msg.Remove(0,header.Length+1);
            Console.WriteLine(msg);
            Console.WriteLine(header);
        }
    }
}
