namespace OrchardCore.ContentManagement.Handlers
{
    public class RemoveContentContext : ShortCircutedContentContext
    {
        public RemoveContentContext(ContentItem contentItem, bool noActiveVersionLeft = false) : base(contentItem)
        {
            NoActiveVersionLeft = noActiveVersionLeft;
        }

        public bool NoActiveVersionLeft { get; }
    }
}
