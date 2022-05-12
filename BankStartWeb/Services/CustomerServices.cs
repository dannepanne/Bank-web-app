using BankStartWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ApplicationDbContext _context;

        public CustomerServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public void UpdateCustomer(int Id, string Givenname, string Surname, string City, string Zipcode, string Streetaddress, string EmailAddress, string CountryId, string Telephone, int TelephoneCountryCode)
        {
            
            var currentCustomer = _context.Customers.Include(x => x.Accounts).First(x => x.Id == Id);
            currentCustomer.Givenname = Givenname;
            currentCustomer.Surname = Surname;
            currentCustomer.City = City;
            currentCustomer.Zipcode = Zipcode;
            currentCustomer.Streetaddress = Streetaddress;
            currentCustomer.EmailAddress = EmailAddress;
            currentCustomer.TelephoneCountryCode = TelephoneCountryCode;
            currentCustomer.Country = CountryId;
            currentCustomer.Telephone = Telephone;

            _context.SaveChanges();

        }
    }
}
