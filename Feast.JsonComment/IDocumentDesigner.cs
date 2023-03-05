using System;
using System.Linq.Expressions;

namespace Feast.JsonComment
{
    public interface IDocumentDesigner
    {
        void OnConfiguring(DocumentConfig config);
    }

    public class DocumentConfig
    {
        public Expression<Func<string>> Path { get; set; }
    }
}
