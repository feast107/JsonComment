using Another = Feast.JsonComment.JsonCommentAttribute;
using Differ = Feast.JsonComment;

namespace Feast.JsonComment.Run.Folder
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
            [Feast.JsonComment.JsonComment]
            [JsonComment]
            [Another]
            public partial class InnerClass
            {
            }
        }
    }

    [Feast.JsonComment.JsonComment]
    [JsonComment]
    [Another]
    [Differ.JsonComment]
    public class FullNamespace
    {
    }
}
