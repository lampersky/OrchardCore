namespace OrchardCore.ContentManagement.Handlers
{
    public class ShortCircutedContentContext : ContentContextBase
    {
        public ShortCircutedContentContext(ContentItem contentItem) : base(contentItem) { }

        public ShortCircuitResult ShortCircuitResult { get; } = new ShortCircuitResult();
    }
}
