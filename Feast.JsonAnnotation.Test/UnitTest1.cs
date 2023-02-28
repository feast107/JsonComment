using Feast.JsonAnnotation.Run;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

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
            var fs = new StackTrace(true).GetFrames();
            var file = fs.First().GetFileName();
        }
    }
}