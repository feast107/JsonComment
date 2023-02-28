using System.Diagnostics;

namespace Feast.JsonAnnotation.Run
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string str = new BasicUsing().Generate();
            Console.WriteLine(str);
            var fs = new StackTrace(true).GetFrame(0)!.GetFileName();
        }
    }
}