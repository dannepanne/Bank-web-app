using BankStartWeb.Data;
using BankStartWeb.Services;
using BankStartWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.AccountView
{
    [Authorize(Roles = "Admin, Cashier")]
    public class TransactionFromAccountModel : PageModel
    {
        public TransactionFromAccountModel(ApplicationDbContext context, IAccountServices accountServices)
        {
            _accountServices = accountServices;
            _context = context;
        }

        

        [BindProperty]
        public string OperationType { get; set; }
        
        [BindProperty]
        public int AccountFrom { get; set; }
        [BindProperty]
        public decimal TransferSum { get; set; }
        public string Type { get; set; } //CashDeposit osv
        public string TransactionType { get; set; } //Debit & Credit

        public CustomerTransactionViewModel currentCustomerView = new CustomerTransactionViewModel();

        public List<SelectListItem> CustAccounts { get; set; }
        public List<SelectListItem> OpType = new List<SelectListItem>();

        private readonly IAccountServices _accountServices;
        private readonly ApplicationDbContext _context;

        public void OnGet(int custId)
        {
            var currentCust = _context.Customers.Include(x => x.Accounts).First(x => x.Id == custId);
            currentCustomerView.Id = currentCust.Id;
            currentCustomerView.Name = currentCust.Givenname + " " + currentCust.Surname;
            currentCustomerView.Accounts = currentCust.Accounts;
            SetAll();
        }


        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _accountServices.AccountWithdrawal(AccountFrom, TransferSum);
                _context.SaveChanges();
                return RedirectToPage("/AccountView/TransactionFromAccount", new { custId = currentCustomerView.Id });

            }
            return RedirectToPage("/AccountView/TransactionFromAccount", new { custId = currentCustomerView.Id });

        }


        public void SetAll()
        {
            CustAccounts = currentCustomerView.Accounts.Select(y => new SelectListItem
            {
                Text = "Kontonummer: " + y.Id + " " + "Summa p� konto:" + " " + y.Balance,
                Value = y.Id.ToString()
            }).ToList();
            CustAccounts.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "V�lj ett konto"
            });
            SetTransactionType();
        }

        public void SetTransactionType()
        {
            OpType.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "V�lj en transaktionstyp"
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
