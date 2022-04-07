using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Pages.CreateEditCustomer
{
    public class CreateCustomerModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateCustomerModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string Streetaddress { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string NationalId { get; set; }
        [Range(0, 9999)]
        public int TelephoneCountryCode { get; set; }
        public string Telephone { get; set; }
        public string EmailAddress { get; set; }
        public DateTime Birthday { get; set; }
        public string CountryId { get; set; }

        public List<SelectListItem> AllCountries { get; set; }

        
        public void OnGet()
        {
            SetAll();
        }

        public void SetAll()
        {
            AllCountries = Enum.GetValues<Countries>().Select(c => new SelectListItem
            {
                Text = c.ToString(),
                Value = c.ToString()
            }).ToList();
            AllCountries.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Välj ett land"
            });
        }

        public void SetCountryCodeNumber(string land)
        {
            if (land == "Sverige")
            {
                CountryCode = "SE";
                TelephoneCountryCode = 46;                  
                    }
            else if (land == "Norge")
            {
                CountryCode = "NO";
                TelephoneCountryCode = 47;
            }
            else if (land == "Finland")
            {
                CountryCode = "FI";
                TelephoneCountryCode = 48;
            }
        }


        public void OnPost()
        {
            SetCountryCodeNumber(Country);
            Customer newcust = new Customer();
            newcust.Country = Country;
            newcust.Telephone = Telephone;
            newcust.City = City;
            newcust.Streetaddress = Streetaddress;
            newcust.Zipcode = Zipcode;
            newcust.Birthday = Birthday;
            newcust.Surname = Surname;
            newcust.Givenname = Givenname;
            newcust.EmailAddress = EmailAddress;
            newcust.NationalId = NationalId;
            newcust.Accounts.Add(new Account() { AccountType = "Savings", Created = DateTime.Now, Balance = 0}); //skapa props för account - savings osv
            _context.Customers.Add(new Customer());
            _context.SaveChanges();
            
        }
    }
}
