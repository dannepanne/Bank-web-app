using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace BankStartWeb.Pages
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public int Inputcustid { get; set; }

        private readonly IToastNotification _toastNotification;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext applicationDbContext, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public int AntalKunder { get; set; }
        public decimal TotalSummaKonton { get; set; }
        public int AntalKonton { get; set; }

        public void OnGet()
        {
            AntalKunder = _applicationDbContext.Customers.Count();
            AntalKonton = _applicationDbContext.Accounts.Count();
            TotalSummaKonton = _applicationDbContext.Accounts.Sum(item => item.Balance);
        }
        public IActionResult OnPost()
        {
            
            int custId = Inputcustid;
            if (_applicationDbContext.Customers.FirstOrDefault(c => c.Id == custId) == null)
            {
                _toastNotification.AddErrorToastMessage("Kunden finns ej!");
                return Page();
            }

            return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = custId });


        }
    }
}