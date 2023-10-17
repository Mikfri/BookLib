using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib
{
    public interface IBookRepository
    {
        ActionResult<Book> Add(Book book);
        IEnumerable<Book> Get(int? maxPrice = null, int? minPrice = null, string? titleIncludes = null, string? orderBy = null);
        Book? GetByID(int id);
        Book? Remove(int id);
        Book? Update(int id, Book values);
    }
}
