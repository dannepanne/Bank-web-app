using BankStartWeb.Data;
using BankStartWeb.Infrastrucure.Validation;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BankStartWeb.Pages.CreateEditCustomer
{
    public class CreateCustomerModel : PageModel
    {
        public IAccountServices _accountServices { get; }

        private readonly ApplicationDbContext _context;

        public CreateCustomerModel(ApplicationDbContext context, IAccountServices accountServices)
        {
            _accountServices = accountServices;
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
        public string Zipcode { get; set; }

        public string CountryCode { get; set; }
        [BindProperty]
        [StringLength(13)]
        [MinLength(12, ErrorMessage = "Du måste skriva in personnummer, 12 siffror")]
        [Required]
        [StringIsNumbers(ErrorMessage ="Endast siffror i personnummret")]
        public string NationalId { get; set; } 
        public int TelephoneCountryCode { get; set; }
        [BindProperty]
        [Required]
        public string Telephone { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        [BindProperty]
        [Required]
        public string CountryId { get; set; }
        [BindProperty]
        [Required]
        public string CustAccountType { get; set; }

        public List<SelectListItem> AllCountries { get; set; }

        public List<SelectListItem> AllAccountTypes { get; set; }
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
            AllAccountTypes = _accountServices.GetAccountTypes();
            AllAccountTypes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Välj en kontotyp"
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


        public IActionResult OnPost()
        {
            
                SetCountryCodeNumber(CountryId);
                
            

            
            if (ModelState.IsValid)
            {
                FixPersId(NationalId);
                if (!_context.Customers.Any(e => e.NationalId == NationalId))
                {
                    Customer newcust = new Customer();
                    newcust.Country = CountryId;
                    newcust.Telephone = Telephone;
                    newcust.City = City;
                    newcust.Streetaddress = Streetaddress;
                    newcust.Zipcode = Zipcode;
                    newcust.Birthday = Birthday;
                    newcust.Surname = Surname;
                    newcust.Givenname = Givenname;
                    newcust.EmailAddress = EmailAddress;
                    newcust.NationalId = NationalId;
                    newcust.Accounts.Add(new Account() { AccountType = CustAccountType, Created = DateTime.Now, Balance = 0, Transactions = new List<Transaction>() });
                    //_context.Customers.Add(new Customer());
                    //_context.SaveChanges();
                    return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = newcust.Id });

                }

                return Page();

             
            }

            return Page();
        }


        public void FixPersId(string personId)
        {   

                List<string> stringList = new List<string>();
                foreach (var item in personId)
                {
                    stringList.Add(item.ToString());
                }
                
                stringList.Insert(8, "-");

                NationalId = String.Join("", stringList);

        }
    }
}
