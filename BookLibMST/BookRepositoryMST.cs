﻿using BookLib;
using static System.Net.Mime.MediaTypeNames;

namespace BookLibMST
{
    [TestClass]
    public class BookRepositoryMST
    {
        private IBookRepository _bookRepo;        
        private List<Book> _originalBookList;

        Book invalidBook = new Book() { Title = "" };

        /// <summary>
        /// [TestInitialize]
        /// Kører før hver testmetode.
        /// Her oprettes en ny instans af BookRepository og kopierer den statiske
        /// _bookList til _originalBookList. For at sikrer man har en kopi
        /// af den oprindelige liste, før du udfører nogen testoperationer.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _bookRepo = new BookRepository();
            //_originalBookList = new List<Book>(BookRepository._bookList);

            /* new Book(_nextID++, "C#: Essentials", 130),
               new Book(_nextID++, "Vue for experts", 1100),
               new Book(_nextID++, "Java for dummies", 800),
               new Book(_nextID++, "Python scripting", 420),
               new Book(_nextID++, "Clients and Servers", 600),*/

        }

        [TestMethod]
        public void AddBookTest()
        {
            Book bookSummer = new() { Title = "Summerville", Price = 380 };
            Assert.AreEqual(6, _bookRepo.Add(bookSummer).ID);   // Har den fået tildelt _nextID?
            Assert.AreEqual(6, _bookRepo.Get().Count());        // Er der nu i alt x bøger?

            Assert.ThrowsException<ArgumentException>(() => _bookRepo.Add(invalidBook));
        }


        [TestMethod]
        public void GetOrderByTest()   //Test på vore IEnumerable<Actor> Get
        {
            IEnumerable<Book> books = _bookRepo.Get();
            Assert.AreEqual(5, books.Count());          //Vi har 5 bøger i vores list.. Så det er vi forventer at få tilbage

            IEnumerable<Book> booksByTitle = _bookRepo.Get(orderBy: "title");
            Assert.AreEqual(booksByTitle.First().Title, "C#: Essentials");

            IEnumerable<Book> booksByTitleDesc = _bookRepo.Get(orderBy: "title_desc");
            Assert.AreEqual(booksByTitleDesc.First().Title, "Vue for experts");

            IEnumerable<Book> booksPriceDesc = _bookRepo.Get(orderBy: "price_desc");
            Assert.AreEqual(booksPriceDesc.First().Title, "Vue for experts");
        }


        [TestMethod]
        public void GetTitleIncludesTest()
        {
            IEnumerable<Book> booksIncludesFor = _bookRepo.Get(titleIncludes: "for", orderBy: "title");
            Assert.IsTrue(booksIncludesFor.All(b => b.Title.Contains("for")));
            Assert.AreEqual(2, booksIncludesFor.Count());
            Assert.AreEqual(booksIncludesFor.First().Title, "Java for dummies");

            // Test med titleIncludes "NonexistentTitle"
            IEnumerable<Book> booksNotExistingTitle = _bookRepo.Get(titleIncludes: "NonexistentTitle");
            Assert.IsFalse(booksNotExistingTitle.Any()); // Forventer ingen resultater
        }

        [TestMethod]
        public void GetPriceFilterTest()
        {
            IEnumerable<Book> booksRange400And800 = _bookRepo.Get(minPrice: 400, maxPrice: 800);
            Assert.IsTrue(booksRange400And800.All(b => b.Price >= 400 && b.Price <= 800));
            // .All     fungere som en bool for alle elementerne i listen. Er de sande?
            // .Where   fungere som et filter for elementerne i listen

            IEnumerable<Book> booksRangeNullAnd300 = _bookRepo.Get(minPrice: null, maxPrice: 300);
            Assert.IsTrue(booksRangeNullAnd300.All(b => b.Price <= 800));

            IEnumerable<Book> booksRange450AndNull = _bookRepo.Get(minPrice: 450, maxPrice: null);
            Assert.IsTrue(booksRange450AndNull.All(b => b.Price >= 450));

            IEnumerable<Book> allBooks = _bookRepo.Get(minPrice: null, maxPrice: null);
            // Forvent, at antallet af returnerede bøger skal være lig med det samlede antal bøger
            Assert.AreEqual(_bookRepo.Get().Count(), allBooks.Count());
        }

        [TestMethod]
        public void GetByIDTest()
        {
            // Hent en bog med eksisterende ID
            Book bookID4Python = _bookRepo.GetByID(4);
            Assert.IsNotNull(bookID4Python);
            Assert.AreNotEqual("C#: Essentials", bookID4Python.Title);
            Assert.AreEqual("Python scripting", bookID4Python.Title);
            Assert.IsNotNull(_bookRepo.GetByID(5));

            Assert.IsNull(_bookRepo.GetByID(6));
        }

        [TestMethod]
        public void UpdateBookTest()
        {
            // Opret en bog, som du vil opdatere
            Book originalBook = new Book { Title = "Original Title", Price = 100 };
            // Bogen tilføres _bookRepo listen og får nu sit _nextID++
            _bookRepo.Add(originalBook);

            Book updatedBook = new Book { Title = "Updated Title", Price = 200 };
            _bookRepo.Update(originalBook.ID, updatedBook);

            // Tjek om de opdaterede egenskaber stemmer overens med forventningerne
            Assert.AreEqual("Updated Title", originalBook.Title);
            Assert.AreEqual(200, originalBook.Price);


        }



        //[TestCleanup]
        //public void Cleanup()
        //{
        //    _bookRepo = null; // Nulstiller _bookRepo

        //    // Gendan den oprindelige _bookList ved at kopiere dataene fra _originalBookList
        //    BookRepository._bookList = new List<Book>(_originalBookList);
        //}
    }
}
