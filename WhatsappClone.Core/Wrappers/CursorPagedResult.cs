using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Wrappers
{
    public class CursorPagedResult<T>
    {
        public IReadOnlyList<T> Items { get; }
        public bool HasNextPage { get; }
        public string? NextCursor { get; } 

        public CursorPagedResult(List<T> items, int limit, Func<T, string> cursorSelector)
        {
            HasNextPage = items.Count > limit;

            if (HasNextPage)
            {
                items.RemoveAt(items.Count - 1);
            }

            Items = items;

            NextCursor = items.Count > 0 ? cursorSelector(items.Last()) : null;
        }
    }
}
