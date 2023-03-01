using AnotherClassName = Feast.JsonAnnotation.JsonAnnotationAttribute;
using AnotherNamespace = Feast.JsonAnnotation;

namespace Feast.JsonAnnotation.Run
{
    
    [JsonAnnotation]
    [AnotherClassName]
    [AnotherNamespace.JsonAnnotation]
    [Feast.JsonAnnotation.JsonAnnotation]
    public partial class BasicUsing
    {
        [AnotherClassName]
        public partial class SubClass
        {

        }

    }

    [AnotherClassName]
    public partial class AnotherNaming { }

    
}