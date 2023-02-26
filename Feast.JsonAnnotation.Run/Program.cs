namespace Feast.JsonAnnotation.Run
{
    internal class Program
    {
        static void Main(string[] args)
        {
            _ = new BasicUsing();
            _ = new FullNamedSpace();
            _ = new AnotherNaming();
            _ = new DifferedNamedSpace();
            _ = new FullNamedSpace();
        }
    }
}