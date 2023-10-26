namespace OrchardCore.ContentManagement.Handlers
{
    public class UpdateContentContext : ShortCircutedContentContext
    {
        public UpdateContentContext(ContentItem contentItem) : base(contentItem)
        {
            UpdatingItem = contentItem;
        }

        public ContentItem UpdatingItem { get; set; }
    }
}
