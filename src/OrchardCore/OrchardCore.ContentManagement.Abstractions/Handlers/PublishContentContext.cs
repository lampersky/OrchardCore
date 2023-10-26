namespace OrchardCore.ContentManagement.Handlers
{
    public class PublishContentContext : ShortCircutedContentContext
    {
        public PublishContentContext(ContentItem contentItem, ContentItem previousContentItem) : base(contentItem)
        {
            PublishingItem = contentItem;
            PreviousItem = previousContentItem;
        }

        public ContentItem PublishingItem { get; set; }
        public ContentItem PreviousItem { get; set; }
    }
}
