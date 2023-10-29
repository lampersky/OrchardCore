namespace OrchardCore.ContentManagement.Handlers
{
    public class ShortCircutedContentContext : ContentContextBase
    {
        public ShortCircutedContentContext(ContentItem contentItem) : base(contentItem) { }

        public ShortCircuitResult ShortCircuitResult { get; } = new ShortCircuitResult();
    }

    public static class ShortCircutedContentContextExtensions
    {
        public static void ShortCircuit(this ShortCircutedContentContext context, string reason)
        {
            context.ShortCircuitResult.ShortCircuit(reason);
        }
    }
}
