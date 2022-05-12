using BankStartWeb.Data;
using BankStartWeb.Services;
using BankStartWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace BankStartWeb.Pages.AccountView
{
    [Authorize(Roles = "Admin, Cashier")]
    public class TransactionModel : PageModel
    {
        private readonly IAccountServices _accountServices;
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;

        public TransactionModel(ApplicationDbContext context, IAccountServices accountServices, IToastNotification toastNotification)
        {
            _accountServices = accountServices;
            _context = context;
            _toastNotification = toastNotification;
        }



        public string OperationType { get; set; }
        [BindProperty]
        public int AccountTo { get; set; }
        [BindProperty]
        public int AccountFrom { get; set; }
        [BindProperty]
        public decimal TransferSum { get; set; }
        public string Type { get; set; } //CashDeposit osv
        public string TransactionType { get; set; } //Debit & Credit

        public CustomerTransactionViewModel currentCustomerView = new CustomerTransactionViewModel();

        public List<SelectListItem> CustAccounts = new List<SelectListItem>();
        public List<SelectListItem> OpType = new List<SelectListItem>();

        public void OnGet(int custId)
        {
            
            var currentCust = _context.Customers.Include(x => x.Accounts).First(x => x.Id == custId);
            currentCustomerView.Id = currentCust.Id;
            currentCustomerView.Name = currentCust.Givenname + " " + currentCust.Surname;
            currentCustomerView.Accounts = currentCust.Accounts;
            SetAll();
        }

        public IActionResult OnPost(int custId)
        {
            
            if (ModelState.IsValid)
            {
                var errorcode = _accountServices.AccountTransfer(AccountTo, AccountFrom, TransferSum);
                if (errorcode == IAccountServices.Errorcode.ThatWentWell)
                {
                    _toastNotification.AddSuccessToastMessage(errorcode.ToString() + " " +
                                                              "Pengar överförda till konto " + AccountTo);
                    return RedirectToPage("/AccountView/AccountViewSingle", new { accId = AccountFrom, custId = custId });

                }
                _toastNotification.AddErrorToastMessage(errorcode.ToString() + " Kunde inte genomföra överföringen");
                return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = custId });
            }
            _toastNotification.AddErrorToastMessage("Kunde inte genomföra överföringen");
            return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = custId });
        }


        public void SetAll()
        {
            CustAccounts = currentCustomerView.Accounts.Select(y => new SelectListItem
            {
                Text = "Kontonummer: " + y.Id + " " + "Summa på konto:" + " " + y.Balance,
                Value = y.Id.ToString()
            }).ToList();
            CustAccounts.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Välj ett konto"
            });

        }





    }
}



