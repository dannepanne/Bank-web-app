using BankStartWeb.Data;
using BankStartWeb.Infrastrucure.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Pages.CreateEditCustomer
{
    public class EditCustomerModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditCustomerModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [Required]
        [BindProperty]
        public string Givenname { get; set; }
        [BindProperty]
        [Required]
        public string Surname { get; set; }
        [BindProperty]
        [Required]
        public string Streetaddress { get; set; }
        [BindProperty]
        [Required]
        public string City { get; set; }
        [BindProperty]
        [Required]
        [StringIsNumbers(ErrorMessage = "Endast siffror i postnumret")]
        public string Zipcode { get; set; }

        public string CountryCode { get; set; }


        public int TelephoneCountryCode { get; set; }
        [BindProperty]
        [Required]
        [StringIsNumbers(ErrorMessage = "Endast siffror i telefonnummret")]
        public string Telephone { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
    
        [BindProperty]
        [Required]
        public string CountryId { get; set; }
        [BindProperty]
        [Required]

        public Customer currentCustomer { get; set; }

        public List<SelectListItem> AllCountries { get; set; }
        public void OnGet(int custId)
        {
            currentCustomer = _context.Customers.Include(x => x.Accounts).First(x => x.Id == custId);
            Givenname = currentCustomer.Givenname;
            Surname = currentCustomer.Surname;
            City = currentCustomer.City;
            Zipcode = currentCustomer.Zipcode;
            Streetaddress = currentCustomer.Streetaddress;
            EmailAddress = currentCustomer.EmailAddress;
            TelephoneCountryCode = currentCustomer.TelephoneCountryCode;
            CountryId = currentCustomer.Country;
            Telephone = currentCustomer.Telephone;

            SetAll();
        }
        public void SetAll()
        {
            AllCountries = Enum.GetValues<Countries>().Select(c => new SelectListItem
            {
                Text = c.ToString(),
                Value = c.ToString()
            }).ToList();
            
            
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

        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
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

                return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = currentCustomer.Id });
                //Spara till kund?
            }
            return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = currentCustomer.Id });

        }
    }
}
