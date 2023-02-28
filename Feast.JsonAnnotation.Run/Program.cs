using System.Diagnostics;

namespace Feast.JsonAnnotation.Run
{
    internal class Program
    {
        static void Main(string[] args)
        {
            JsonAnnotation.Generate();
            var fs = new StackTrace(true).GetFrame(0)!.GetFileName();
        }
    }
}
