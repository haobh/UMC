using System.Collections.Generic;
using System.Linq;

namespace UMC.Web.Infrastructure.Core
{
    public class PaginationSet<T>
    {
        public int Page { set; get; }

        public int Count
        {
            get
            {
                //Neu có hien thi tong so ban ghi, nguoc lai hien thi 0
                return (Items != null) ? Items.Count() : 0;
            }
        }

        public int TotalPages { set; get; }
        public int TotalCount { set; get; }
        public int MaxPage { set; get; }
        public IEnumerable<T> Items { set; get; } //Dung de chua List thong tin ban ghi Object
    }
}