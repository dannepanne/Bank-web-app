using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BankStartWeb.Services.AccountServices;

namespace BankTest.Services
{ 
    [TestClass]
    public class AccountServicesTest
    {

        [TestMethod]
        public void Register_Customer_Should_Return_False()
        {
            CreateCustomer("Anders", 10);
            var cust = _context.Customers.FirstOrDefault(e => e.Id == 1);
            cust.City = null;
            var result = _sut.RegisterCustomerCorrect(cust);
            Assert.AreEqual(IAccountServices.Errorcode.No, result);

        }
        [TestMethod]
        public void Register_Customer_Should_Return_True()
        {
            _context.Customers.Add(new Customer
            {
                Surname = "Efternamn",
                Givenname = "Förnamn",
                Streetaddress = "Gatan 1",
                City = "Stad",
                Zipcode = "12345",
                Country = "Långtbortistan",
                CountryCode = "5",
                NationalId = "19121212-1212",
                TelephoneCountryCode = 04,
                Telephone = "121212",
                EmailAddress = "tolv@mail.com",
                Birthday = DateTime.Today,
                Accounts = new List<Account>()

            });
            _context.SaveChanges();

            var result = _context.Customers.FirstOrDefault(e => e.Id == 1);
            Assert.AreEqual(result.NationalId, "19121212-1212");
        }
    
        
        [TestMethod]
        public void Account_Transfer_Cannot_Transfer_Negative_Amount()
        {
            CreateAccount(500);
            CreateAccount(500);
            var result = _sut.AccountTransfer(2, 1, -200);
            Assert.AreEqual(IAccountServices.Errorcode.CantTransferNegativeAmount, result);
        }
        [TestMethod]
        public void Account_Transfer_Cannot_Recieve_Negative_Amount()
        {
            CreateAccount(500);
            var result = _sut.AccountDeposit(1, -100);
            Assert.AreEqual(IAccountServices.Errorcode.CantTransferNegativeAmount, result);
        }

        

        private AccountServices _sut;
        private ApplicationDbContext _context;

        public AccountServicesTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            _context = new ApplicationDbContext(options);
            _sut = new AccountServices(_context);

        }

        public void CreateAccount(decimal sum)
        {
            _context.Accounts.Add(new Account
            {
                Balance = sum,
                Created = DateTime.Now,
                Transactions = new List<Transaction>(),
                AccountType = "Savings",
            });
            _context.SaveChanges();

        }

        public void CreateCustomer(string name, int number)
        {
            _context.Customers.Add(new Customer
            {
                Surname = name + name,
                Givenname = name,
                Streetaddress = name + "gatan " + number,
                City = name + "köping",
                Zipcode = (number * 100).ToString(),
                Country = name + "republiken",
                CountryCode = number.ToString(),
                NationalId = (00000000 + number).ToString(),
                TelephoneCountryCode = number,
                Telephone = (number + 35).ToString(),
                EmailAddress = name + number.ToString() + "@mail.com",
                Birthday = DateTime.Today,
                Accounts = new List<Account>()

            });
            _context.SaveChanges();
        }
        

        [TestMethod]
        public void Account_Transfer_That_Went_Well()
        {
            CreateAccount(500);
            CreateAccount(500);
            var result = _sut.AccountTransfer(2, 1, 200);
            Assert.AreEqual(IAccountServices.Errorcode.ThatWentWell, result);
        }

        [TestMethod]
        public void Account_Transfer_Not_Enough_cash()
        {
            CreateAccount(500);
            CreateAccount(500);
            var result = _sut.AccountTransfer(2, 1, 1000);
            Assert.AreEqual(IAccountServices.Errorcode.NotEnoughCash, result);
        }

        [TestMethod]
        public void Account_Exists_Should_Return_Ok()
        {
            CreateAccount(500);
            var result = _sut.AccountExists(1);
            Assert.AreEqual(IAccountServices.Errorcode.ThatWentWell, result);
        }

        [TestMethod]
        public void Account_Exists_Should_Return_IncorrectTargetId()
        {
            CreateAccount(500);
            var result = _sut.AccountExists(2);
            Assert.AreEqual(IAccountServices.Errorcode.IncorrectTargetId, result);
        }

    }
}
