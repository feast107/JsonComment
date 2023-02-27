using Another = Feast.JsonAnnotation.JsonAnnotationAttribute;
using Differ = Feast.JsonAnnotation;
using Full = Feast.JsonAnnotation;

namespace Feast.JsonAnnotation.Run
{
    
    [JsonAnnotation]
    public partial class BasicUsing
    {
        /// <summary>
        /// ???
        /// </summary>
        [Another]
        public partial class SubClass
        {

        }

    }

    [Another]
    public class AnotherNaming { }

    [Feast.JsonAnnotation.JsonAnnotation]
    public class FullNamespace { }

    [Differ.JsonAnnotation]
    public class DifferedNamedSpace { }

    [Full.JsonAnnotation]
    public class FullNamedSpace { }
}