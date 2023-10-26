namespace OrchardCore.ContentManagement.Handlers
{
    public class SaveDraftContentContext : ShortCircutedContentContext
    {
        public SaveDraftContentContext(ContentItem contentItem) : base(contentItem)
        {
        }
    }
}
