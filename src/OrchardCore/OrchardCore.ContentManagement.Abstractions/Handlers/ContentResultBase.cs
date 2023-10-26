namespace OrchardCore.ContentManagement.Handlers
{
    public abstract class ContentResultBase : IContentResult
    {
        public bool Succeeded { get; set; }

        public ContentResultBase(bool isSucceeded)
        {
            Succeeded = isSucceeded;
        }
    }

    public abstract class ContentResultBase<T> : IContentResult<T>
    {
        public bool Succeeded { get; set; }

        public ContentResultBase(bool isSucceeded)
        {
            Succeeded = isSucceeded;
        }
    }

    public class NotSucceededResult : ContentResultBase
    {
        public string Reason { get; set; }

        public NotSucceededResult() : base(false)
        {
        }

        public NotSucceededResult(string reason) : this()
        {
            Reason = reason;
        }
    }

    public class SucceededResult : ContentResultBase
    {
        public SucceededResult() : base(true)
        {
        }
    }

    public class SucceededResult<T> : ContentResultBase<T>
    {
        public T Result { get; set; }
        public SucceededResult(T result) : base(true)
        {
            Result = result;
        }
    }
}
