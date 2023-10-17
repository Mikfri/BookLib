using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookLib
{
    /*Opgave 2 Repository-klasse
    Fortsæt i projektet fra forrige opgave.

    Lav en klasse BooksRepository
    Klassen skal indeholde en liste af Book objekter. Indsæt mindst 5 objekter i listen.
    Klassen skal have flg metoder:

    Get()
    Returnerer en kopi af listen af alle Book objekter: Brug en "copy constructor"
      - Get() skal give mulighed for at filtrere på Price.
        Get() skal give mulighed for at sortere/orderBy på Title eller Price.

      - GetById(int id)
        Returnerer Book objektet med det angivne id - eller null.
      - Add(Book book)
        Tilføjer id til  book objektet. Tilføjer book til listen. Returnerer book objektet
      - Delete(int id)
        Sletter book objektet med det angivne id. Returnerer book objektet - eller null.
      - Update(int id, Book values)
        Book objektet med det angivne id opdateres med values.
        Returnerer det opdaterede book objekt - eller null.

    Din repository-klasse skal unit testes: Du kan nøjes med at teste tre metoder.
    Din testede metoder skal have et godt “Code Coverage”

    Besvarelsen på opgave 1 + 2 skal lægges i et GitHub repository.*/

    public class BookRepository : IBookRepository
    {
        public int _nextId = 1;
        public List<Book> _bookList;

        public BookRepository()
        {
            _bookList = new List<Book>
            {
                // selvom der står ++ for første _nextID er den = 1, og ikke 2.. Sårn er det :/
                new Book(_nextId++, "C#: Essentials", 130),
                new Book(_nextId++, "Vue for experts", 1100),
                new Book(_nextId++, "Java for dummies", 800),
                new Book(_nextId++, "Python scripting", 420),
                new Book(_nextId++, "Clients and Servers", 600),
            };
        }


        public ActionResult<Book> Add(Book book)
        {
            book.Id = _nextId++;

            if (_bookList.Any(b => b.Id == book.Id))
            {
                return new ConflictObjectResult($"En bog med ID: {book.Id} eksisterer allerede");
            }

            if (_bookList.Any(b => b.Title == book.Title))
            {
                // Returnér konfliktstatus (409) med fejlmeddelelse
                return new ConflictObjectResult($"En bog med titlen: '{book.Title}' eksisterer allerede");
            }

            book.Validate();
            _bookList.Add(book);

            // Returnér den tilføjede bog
            return new OkObjectResult(book);
        }
        //public Book Add(Book book)
        //{
        //    book.Validate();       //svarer her til ModelState.IsValid
        //    book.Id = _nextID++;

        //    // Sørg for, at der ikke kan tilføjes to objekter med samme titel
        //    if (_bookList.Any(b => b.Title == book.Title))
        //    {
        //        throw new ArgumentException($"En bog med titlen: '{book.Title}' eksisterer allerede.");
        //    }

        //    _bookList.Add(book);
        //    return book;
        //}

        public IEnumerable<Book> Get(int? maxPrice = null, int? minPrice = null, string? titleIncludes = null, string? orderBy = null)
        {
            IEnumerable<Book> filteredBooks = new List<Book>(_bookList);
            //var filteredBooks = _bookList.AsEnumerable(); //Alternativt..

            if (maxPrice.HasValue && minPrice.HasValue)
            {
                filteredBooks = filteredBooks.Where(b => b.Price >= minPrice && b.Price <= maxPrice);
            }
            else if (maxPrice.HasValue)
            {
                filteredBooks = filteredBooks.Where(b => b.Price <= maxPrice);
            }
            else if (minPrice.HasValue)
            {
                filteredBooks = filteredBooks.Where(b => b.Price >= minPrice);
            }

            if (titleIncludes != null)
            {
                filteredBooks = filteredBooks.Where(b => b.Title.Contains(titleIncludes));
            }

            if (orderBy != null)
            {
                orderBy = orderBy.ToLower();
                switch (orderBy)
                {
                    case "title":
                    case "title_asc":
                        filteredBooks = filteredBooks.OrderBy(b => b.Title);
                        break;
                    case "title_desc":
                        filteredBooks = filteredBooks.OrderByDescending(b => b.Title);
                        break;
                    case "price":
                    case "price_asc":
                        filteredBooks = filteredBooks.OrderBy(b => b.Price);
                        break;
                    case "price_desc":
                        filteredBooks = filteredBooks.OrderByDescending(b => b.Price);
                        break;
                    default:
                        break;
                }
            }
            return filteredBooks;
        }


        public Book? GetByID(int id)
        {
            return _bookList.Find(b => b.Id == id);
            //return _bookList.FirstOrDefault(book => book.ID == id); // alternativt..
        }

        public Book? Remove(int id)
        {
            Book? book = GetByID(id);
            if (book == null)
                return null;

            _bookList.Remove(book);
            return book;
        }

        public Book? Update(int id, Book values)
        {
            Book? existingBook = GetByID(id);
            if (existingBook == null)
                return null;

            existingBook.Title = values.Title;
            existingBook.Price = values.Price;
            values.Validate();
            return existingBook;
        }
        //public bool TryUpdate(int id, Book values, out Book updatedBook)    //Alternativt.. Her tilføjes en ´bool´ (out)
        //{
        //    updatedBook = GetByID(id);

        //    if (updatedBook == null)
        //    {
        //        return false;
        //    }

        //    values.Validate();

        //    updatedBook.Title = values.Title;
        //    updatedBook.Price = values.Price;
        //    return true;
        //}

    }
}
