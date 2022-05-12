using BankStartWeb.Data;

namespace BankStartWeb.Services
{
    public interface ICustomerServices
    {
        public void UpdateCustomer(int Id, string Givenname, string Surname, string City, string Zipcode, string Streetaddress, string EmailAddress, string CountryId, string Telephone, int TelephoneCountryCode);



    }
}