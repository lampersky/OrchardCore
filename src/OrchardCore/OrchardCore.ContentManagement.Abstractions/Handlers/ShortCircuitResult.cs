using System.Collections.Generic;

namespace OrchardCore.ContentManagement.Handlers
{
    public class ShortCircuitResult : SucceededResult
    {
        private readonly List<string> _reasons = new();

        public IReadOnlyList<string> Reasons => _reasons;

        public bool Cancelled { get; set; } = false;

        public void ShortCircuit(string reason)
        {
            Cancelled = true;
            _reasons.Add(reason);
        }
    }
}
