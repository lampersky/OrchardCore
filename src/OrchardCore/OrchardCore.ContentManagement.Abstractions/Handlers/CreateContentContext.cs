namespace OrchardCore.ContentManagement.Handlers
{
    public class CreateContentContext : ShortCircutedContentContext
    {
        public CreateContentContext(ContentItem contentItem) : base(contentItem)
        {
        }
    }
}
