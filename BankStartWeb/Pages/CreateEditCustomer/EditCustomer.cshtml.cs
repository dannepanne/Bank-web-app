using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankStartWeb.Pages.CreateEditCustomer
{
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
        }
    }
}
