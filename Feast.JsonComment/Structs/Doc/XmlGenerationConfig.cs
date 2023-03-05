namespace Feast.JsonComment.Structs.Doc
{
    internal struct XmlGenerationConfig
    {
        public XmlGenerationConfig() { }
        public required string Root { get; init; }= "Document";

        public required string Namespace { get; set; } = nameof(Namespace);

        public required string Class { get; set; } = nameof(Class);

        public required string FileName { get; init; }
    }
}
