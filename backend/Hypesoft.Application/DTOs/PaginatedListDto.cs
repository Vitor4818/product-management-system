using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hypesoft.Application.DTOs
{
    public class PaginatedListDto<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public PaginatedListDto(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            // Calcula o total de páginas de forma segura.
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public PaginatedListDto() { }
    }
}
