using Another = Feast.JsonAnnotation.JsonAnnotationAttribute;
using Differ = Feast.JsonAnnotation;
using Full = Feast.JsonAnnotation;

namespace Feast.JsonAnnotation.Run.Folder
{
    namespace InnerSpace
    {
        [Feast.JsonAnnotation.JsonAnnotation]
        [JsonAnnotation]
        [Another]
        public class InnerClass { }
    }

    [Feast.JsonAnnotation.JsonAnnotation]
    [JsonAnnotation]
    [Another]
    [Differ.JsonAnnotation]
    public class FullNamespace { }

    [Differ.JsonAnnotation]
    public partial class DifferedNamedSpace { }

    [Full.JsonAnnotation]
    public class FullNamedSpace { }

}
