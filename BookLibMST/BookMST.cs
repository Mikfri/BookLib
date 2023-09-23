using BookLib;

namespace BookLibMST
{
    [TestClass]
    public class BookMST
    {
        //ARRANGE
        private Book bookOK = new Book { ID = 1, Title = "C#: Essentials", Price = 230 };
        private Book bookInvalidPriceTooLow = new Book { ID = 2, Title = "Python for dummies", Price = -1 };
        private Book bookInvalidPriceTooHigh = new Book { ID = 3, Title = "Python for experts", Price = 1201 };
        private Book bookInvalidTitleNull = new Book { ID = 4, Title = null, Price = 100 };
        private Book bookInvalidTitleShort = new Book { ID = 5, Title = "", Price = 1200 };

        [TestMethod]
        public void ToStringTest()
        {
            string str = bookOK.ToString();   // act
            Assert.AreEqual("1 C#: Essentials 230", str);  // assert
        }

        [TestMethod]
        public void ValidatePriceTest()
        {
            bookOK.ValidatePrice();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => bookInvalidPriceTooLow.ValidatePrice());
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => bookInvalidPriceTooHigh.ValidatePrice());
        }

        [TestMethod]
        public void ValidateTitleTest()
        {
            bookOK.ValidateTitle();
            Assert.ThrowsException<ArgumentNullException>(() => bookInvalidTitleNull.ValidateTitle());
            Assert.ThrowsException<ArgumentException>(() => bookInvalidTitleShort.ValidateTitle());
        }

        [TestMethod()]
        public void ValidateTest()
        {
            bookOK.Validate();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => bookInvalidPriceTooLow.Validate());
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => bookInvalidPriceTooHigh.Validate());
            Assert.ThrowsException<ArgumentNullException>(() => bookInvalidTitleNull.Validate());
            Assert.ThrowsException<ArgumentException>(() => bookInvalidTitleShort.Validate());
        }

        [TestMethod()]
        [DataRow(1)]
        [DataRow(100)]
        [DataRow(1200)]
        //[DataRow(-32)]    //Denne metode vil cast en exception!
        public void ValidatePricesTest(int price)
        {
            bookOK.Price = price;
            bookOK.ValidatePrice();
        }
    }
}