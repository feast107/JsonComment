using Feast.JsonAnnotation;
using Another = Feast.JsonAnnotation.JsonAnnotationAttribute;
using Differ = Feast.JsonAnnotation;
using Full = Feast.JsonAnnotation;

namespace Feast.JsonAnnotation.Run
{
    [JsonAnnotation]
    public class BasicUsing { }

    [Another]
    public class AnotherNaming { }

    [Feast.JsonAnnotation.JsonAnnotation]
    public class FullNamespace { }

    [Differ.JsonAnnotation]
    public class DifferedNamedSpace { }

    [Full.JsonAnnotation]
    public class FullNamedSpace { }
}
