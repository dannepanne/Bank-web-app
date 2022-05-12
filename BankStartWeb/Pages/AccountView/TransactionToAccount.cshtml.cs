using BankStartWeb.Data;
using BankStartWeb.Services;
using BankStartWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace BankStartWeb.Pages.AccountView
{
    public class TransactionToAccountModel : PageModel
    {
     
        public TransactionToAccountModel(ApplicationDbContext context, IAccountServices accountServices, IToastNotification toastNotification)
        {
            _accountServices = accountServices;
            _context = context;
            _toastNotification = toastNotification;
        }
        [BindProperty]
        public string OperationType { get; set; }
        [BindProperty]
        public int AccountTo { get; set; }
      
        [BindProperty]
        public decimal TransferSum { get; set; }
        //public string Type { get; set; } //CashDeposit osv

        public CustomerTransactionViewModel currentCustomerView = new CustomerTransactionViewModel();

        public List<SelectListItem> CustAccounts { get; set; }
        public List<SelectListItem> OpType = new List<SelectListItem>();
        private readonly IAccountServices _accountServices;
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;

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
                var errorcode = _accountServices.AccountDeposit(AccountTo, TransferSum, OperationType);
                if (errorcode == IAccountServices.Errorcode.ThatWentWell)
                {
                    _toastNotification.AddSuccessToastMessage(errorcode.ToString() + " " +
                                                              "Pengar överförda till konto " + AccountTo);
                    return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = custId });

                }
                else
                {
                    _toastNotification.AddSuccessToastMessage(errorcode.ToString() + "Överföring till konto " + AccountTo + " misslyckades");
                    ;
                    return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = custId });
                }
                
            }
            _toastNotification.AddAlertToastMessage("Något gick fel");
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
           
        }
    }
}
