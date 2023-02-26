using Feast.JsonAnnotation.Attributes;
using Another = Feast.JsonAnnotation.Attributes.JsonAnnotationAttribute;
using Differ = Feast.JsonAnnotation;
using Full = Feast.JsonAnnotation.Attributes;

namespace Feast.JsonAnnotation.Run
{
    [JsonAnnotation]
    public class BasicUsing { }

    [Another]
    public class AnotherNaming { }

    [Feast.JsonAnnotation.Attributes.JsonAnnotation]
    public class FullNamespace { }

    [Differ.Attributes.JsonAnnotation]
    public class DifferedNamedSpace { }

    [Full.JsonAnnotation]
    public class FullNamedSpace { }
}
