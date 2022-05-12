using BankStartWeb.Data;
using BankStartWeb.Infrastrucure.Validation;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace BankStartWeb.Pages.CreateEditCustomer
{
    [Authorize(Roles = "Admin")]

    public class EditCustomerModel : PageModel
    {
        private readonly ICustomerServices _customerServices;
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;

        public EditCustomerModel(ApplicationDbContext context, ICustomerServices customerServices, IToastNotification toastNotification)
        {
            _customerServices = customerServices;
            _context = context;
            _toastNotification = toastNotification;
        }
        
        public int custId { get; set; }
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
        [CorrectZipcode(ErrorMessage = "Endast siffror i postnumret")]
        public string Zipcode { get; set; }

        public string CountryCode { get; set; }


        public int TelephoneCountryCode { get; set; }
        [BindProperty]
        [Required]
        [CorrectTelephone(ErrorMessage = "Endast siffror i telefonnummret")]
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
            custId = currentCustomer.Id;

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

        public IActionResult OnPost(int custId)
        {
            
            if(ModelState.IsValid)
            {

                _customerServices.UpdateCustomer(custId, Givenname, Surname, City, Zipcode, Streetaddress, EmailAddress, CountryId, Telephone, TelephoneCountryCode);
                _toastNotification.AddSuccessToastMessage("Lyckad uppdatering av " + Surname);

                return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = custId });
               
            }

            SetAll();
            _toastNotification.AddErrorToastMessage("Kunde inte uppdatera " + Surname + " se över dina inmatningar.");
            return Page();
            

        }
    }
}
