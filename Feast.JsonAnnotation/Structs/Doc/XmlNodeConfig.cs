using System;
using System.Collections.Generic;
using System.Text;

namespace Feast.JsonAnnotation.Structs.Doc
{
    internal struct XmlNodeConfig
    {
        public XmlNodeConfig() { }
        public required string Root { get; init; }= "Document";

        public required string Namespace { get; set; } = nameof(Namespace);

        public required string Class { get; set; } = nameof(Class);
    }
}
