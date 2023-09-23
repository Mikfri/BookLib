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
        public int _nextID = 1;
        public List<Book> _bookList;

        public BookRepository()
        {
            _bookList = new List<Book>
            {
                // selvom der står ++ for første _nextID er den = 1, og ikke 2.. Sårn er det :/
                new Book(_nextID++, "C#: Essentials", 130),
                new Book(_nextID++, "Vue for experts", 1100),
                new Book(_nextID++, "Java for dummies", 800),
                new Book(_nextID++, "Python scripting", 420),
                new Book(_nextID++, "Clients and Servers", 600),
            };
        }

        public Book Add(Book book)
        {
            book.Validate();       //svarer her til ModelState.IsValid
            book.ID = _nextID++;
            _bookList.Add(book);
            return book;
        }

        public IEnumerable<Book> Get(int? maxPrice, int? minPrice, string? titleIncludes, string? orderBy)
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
            return _bookList.Find(b => b.ID == id);
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
            values.Validate();
            Book? existingBook = GetByID(id);     //Vi har denne del for at sikre os vi ikke kan Update på en book vi måske liige har slettet
            if (existingBook == null)
                return null;

            existingBook.Title = values.Title;
            existingBook.Price = values.Price;
            return existingBook;
        }

    }
}
