namespace Feast.JsonComment.Test
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