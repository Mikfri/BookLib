using System.Xml.Linq;

namespace BookLib
{

    /* ---: OPG BESKRIVELSE :--- 
     Lav en klasse Book med properties (bemærk de forskellige constraints):

    Id, et tal
    Title, tekst-streng, min 3 tegn langt, må ikke være null
    Price, tal, 0 < price <= 1200
    samt en ToString() metode

    Der skal være validerings-metoder til de relevante properties.
    Valideringsmetoderne skal kaste passende exceptions

    Tilføj en unit test til dit projekt.
    Din unit test skal have et godt “Code Coverage” */


    public class Book
    {
        public int ID { get; set; }         //ISBN
        public string Title { get; set; }
        public int Price { get; set; }

        public Book(int id, string title, int price)
        {
            ID = id;
            Title = title;
            Price = price;
        }

        public Book() { }

        public void ValidateTitle()
        {
            if (Title == null)
            {
                throw new ArgumentNullException("Title is null");
            }

            if (Title.Length < 4)
            {
                throw new ArgumentException($"Title must not be under {1} chars. {Title} is {Title.Length} character(s) long!");
            }
        }

        public void ValidatePrice()
        {
            if (Price < 0 || Price > 1200)
                throw new ArgumentOutOfRangeException($"Price isn't set within the allowed range");
        }

        public void Validate()
        {
            ValidateTitle();
            ValidatePrice();
        }

        public override string ToString()
        {
            return $"{ID} {Title} {Price}";
        }
    }
}