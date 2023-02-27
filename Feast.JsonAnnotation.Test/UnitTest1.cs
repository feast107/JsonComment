using Feast.JsonAnnotation.Run;

namespace Feast.JsonAnnotation.Test
{
    public class UnitTest1
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test()
        {
            var str = new BasicUsing().Generate();
            Console.WriteLine(str);
            
        }
    }
}