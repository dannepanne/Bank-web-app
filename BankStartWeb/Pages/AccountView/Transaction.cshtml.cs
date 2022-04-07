using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.AccountView
{
    public class TransactionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TransactionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int ToAccountId { get; set; }
        public int FromAccountId { get; set; }
        public int TransferSum { get; set; }
        public string Type { get; set; }       
        public string Operation { get; set; }
        public DateTime Date { get; set; }       
        public decimal NewBalance { get; set; }


        public void OnGet(int custId)
        {
            var currentCustomer = _context.Customers.Include(x => x.Accounts).ThenInclude(x => x.Transactions).First(x => x.Id == custId);

        }
    }
}
