using BankStartWeb.Data;
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
    
        public List<Transaction> transactions { get; set; }
        public int accountId { get; set; }
        public string customerName { get; set; }

        public void OnGet(int accId)
        {

            //Account currentAccount = new Account();
            var currentAccount = _context.Accounts.Include(x => x.Transactions).First(x => x.Id == accId);
            transactions = currentAccount.Transactions;
            accountId = currentAccount.Id;

        }
    }
}
