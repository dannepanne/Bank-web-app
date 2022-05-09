using BankStartWeb.Data;
using BankStartWeb.Infrastrucure.Paging;
using BankStartWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.AccountView
{
    public class AccountViewSingleModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AccountViewSingleModel(ApplicationDbContext context)
        {
            _context = context;
            
        }

        public class CustomerTransactionView
        {

        }

        public List<Transaction> transactions { get; set; }
        public int accountId { get; set; }
        public string customerName { get; set; }

        public void OnGet(int accId, int custId)
        {
            //var currentAccount = _context.Customers.Include(x=>x.Accounts).ThenInclude(x=>x.Transactions).First(x => x.Id == accId);
            var currentAccount = _context.Accounts.Include(x => x.Transactions).First(x => x.Id == accId);
            transactions = currentAccount.Transactions;
            accountId = currentAccount.Id;
            var customer = _context.Customers.FirstOrDefault(x => x.Id == custId);
            customerName = customer.Givenname + " " + customer.Surname;

        }

        public IActionResult OnGetFetchMore(int accId, int pageNo)
        {
            var query = _context.Accounts.Where(x => x.Id == accId)
                .SelectMany(e => e.Transactions)
                .OrderBy(a => a.Amount);

            var r = query.GetPaged(pageNo, 5);

            var list = r.Results.Select(t=> new Transaction
                {
                    Type = t.Type,
                    Id = t.Id,
                    Amount = t.Amount,
                    Operation = t.Operation,
                    Date = t.Date,
                    NewBalance = t.NewBalance
                }).ToList();

            bool lastPage = pageNo == r.PageCount;

            return new JsonResult(new { items = list, lastPage = lastPage });

        }

    }
}
