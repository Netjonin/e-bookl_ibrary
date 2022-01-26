using E_BOOKLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.DTOs
{
    public class PaginatedListDto<T>
    {
        public PageMeta MetaData { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PaginatedListDto()
        {
            Data = new List<T>();
        }
    }
}
