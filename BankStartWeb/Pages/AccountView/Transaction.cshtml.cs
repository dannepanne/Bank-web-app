using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        
        public string OperationType { get; set; }
        [BindProperty]
        public int AccountTo { get; set; }
        [BindProperty]
        public int AccountFrom { get; set; }
        [BindProperty]
        public int TransferSum { get; set; }
        public string Type { get; set; }
        public string TransactionType { get; set; } //Debit & Credit
        public DateTime Date { get; set; }
        public decimal NewBalance { get; set; }
        public Customer currentCustomer { get; set; }

        public List<SelectListItem> CustAccounts { get; set; }
        public List<SelectListItem> OpType = new List<SelectListItem>();

        public void OnGet(int custId)
        {
            currentCustomer = _context.Customers.Include(x => x.Accounts).ThenInclude(x => x.Transactions).First(x => x.Id == custId);
            SetAll();
        }

        public void OnPost()
        {

        }


        public void SetAll()
        {
            CustAccounts = currentCustomer.Accounts.Select(y => new SelectListItem
            {
                Text = "Kontonummer: " + y.Id + " " + "Summa på konto:" + " " + y.Balance,
                Value = y.Id.ToString()
            }).ToList();
            CustAccounts.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Välj ett konto"
            });
            SetTransactionType();
        }

        public void SetTransactionType()
        {
            OpType.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Välj en transaktionstyp"
            });
            OpType.Add(new SelectListItem
            {
                Value = "Deposit cash",
                Text = "Kontantinsättning"
            });
            OpType.Add(new SelectListItem
            {
                Value = "Salary",
                Text = "Lön"
            });
            OpType.Add(new SelectListItem
            {
                Value = "Transfer",
                Text = "Överföring"
            });
            OpType.Add(new SelectListItem
            {
                Value = "ATM withdrawal",
                Text = "Bankomatuttag"
            });
            OpType.Add(new SelectListItem
            {
                Value = "Payment",
                Text = "Betalning"
            }); 
        }
            



    }
}

        
        
      