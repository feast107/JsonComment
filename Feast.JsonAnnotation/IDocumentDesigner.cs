using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Feast.JsonAnnotation
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
