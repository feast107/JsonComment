using Another = Feast.JsonAnnotation.JsonAnnotationAttribute;
using Differ = Feast.JsonAnnotation;
using Full = Feast.JsonAnnotation;

namespace Feast.JsonAnnotation.Run.Folder
{
    namespace InnerSpace
    {

        /// <summary>
        /// <include file='Document.xml' path='Document/Namespace[@name="InnerSpace"]/Class[@name="Class"]/code'/>
        /// </summary>
        public partial class Class
        {
            /// <summary>
            /// <include file='Document.xml' path='Document/Namespace[@name="InnerSpace"]/Class[@name="Class"]/Class[@name="InnerClass"]'/>
            /// </summary>
            [Feast.JsonAnnotation.JsonAnnotation]
            [JsonAnnotation]
            [Another]
            public partial class InnerClass
            {
            }
        }
    }

    [Feast.JsonAnnotation.JsonAnnotation]
    [JsonAnnotation]
    [Another]
    [Differ.JsonAnnotation]
    public class FullNamespace
    {
    }
}
