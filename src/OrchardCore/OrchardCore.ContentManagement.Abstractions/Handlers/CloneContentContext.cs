namespace OrchardCore.ContentManagement.Handlers
{
    public class CloneContentContext : ShortCircutedContentContext
    {
        public ContentItem CloneContentItem { get; set; }

        public CloneContentContext(ContentItem contentItem, ContentItem cloneContentItem)
            : base(contentItem)
        {
            CloneContentItem = cloneContentItem;
        }
    }
}
