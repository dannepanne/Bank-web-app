using BankStartWeb.Data;
using BankStartWeb.Infrastrucure.Validation;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Pages.CreateEditCustomer
{
    public class EditCustomerModel : PageModel
    {
        private readonly ICustomerServices _customerServices;
        private readonly ApplicationDbContext _context;

        public EditCustomerModel(ApplicationDbContext context, ICustomerServices customerServices)
        {
            _customerServices = customerServices;
            _context = context;
        }
        
        public int Id { get; set; }
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

       

        public List<SelectListItem> AllCountries { get; set; }
        public void OnGet(int custId)
        {
            
            var currentCustomer = _context.Customers.Include(x => x.Accounts).First(x => x.Id == custId);
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

        public IActionResult OnPost(int Id)
        {
            if(ModelState.IsValid)
            {

                _customerServices.UpdateCustomer(Id, Givenname, Surname, City, Zipcode, Streetaddress, EmailAddress, CountryId, Telephone, TelephoneCountryCode);

                return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = Id });
               
            }
            return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = Id });

        }
    }
}
