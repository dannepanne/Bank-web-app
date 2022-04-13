using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankStartWeb.Pages
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public int Inputcustid { get; set; }

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext applicationDbContext)
        {
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
            return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = custId });


        }
    }
}