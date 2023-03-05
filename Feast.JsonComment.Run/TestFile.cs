using AnotherClassName = Feast.JsonComment.JsonCommentAttribute;
using AnotherNamespace = Feast.JsonComment;

namespace Feast.JsonComment.Run
{
    [JsonComment]
    [AnotherClassName]
    [AnotherNamespace.JsonComment]
    [Feast.JsonComment.JsonComment]
    public partial class BasicUsing
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [AnotherClassName]
        public partial class SubClass
        {

        }

    }

    [AnotherClassName]
    public partial class AnotherNaming { }

    
}