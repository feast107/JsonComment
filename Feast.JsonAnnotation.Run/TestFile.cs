﻿using Another = Feast.JsonAnnotation.JsonAnnotationAttribute;
using Differ = Feast.JsonAnnotation;
using Full = Feast.JsonAnnotation;

namespace Feast.JsonAnnotation.Run
{
    
    [JsonAnnotation]
    public partial class BasicUsing
    {
        [Another]
        public partial class SubClass
        {

        }

    }

    [Another]
    public partial class AnotherNaming { }

    [Feast.JsonAnnotation.JsonAnnotation]
    public class FullNamespace { }

    [Differ.JsonAnnotation]
    public partial class DifferedNamedSpace { }

    [Full.JsonAnnotation]
    public class FullNamedSpace { }
}