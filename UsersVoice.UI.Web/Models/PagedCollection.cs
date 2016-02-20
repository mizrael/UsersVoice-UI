using System.Collections.Generic;
using System.Linq;

namespace UsersVoice.UI.Web.Models
{
    public class PagedCollection<T>
    {
        public PagedCollection(IEnumerable<T> items, long page, long pageSize, long totalPagesCount, long totalItemsCount)
        {
            this.Items = items;
            this.Page = page;
            this.PageSize = pageSize;
            this.TotalItemsCount = totalItemsCount;
            this.TotalPagesCount = totalPagesCount;
        }

        public long Page { get; private set; }

        public long PageSize { get; private set; }

        public long TotalPagesCount { get; private set; }

        public long TotalItemsCount { get; private set; }

        public IEnumerable<T> Items { get; private set; }

        public static readonly PagedCollection<T> Empty = new PagedCollection<T>(Enumerable.Empty<T>(), 0, 0, 0, 0);

    }
}