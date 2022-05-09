using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Pages.AccountView
{
    public class CreateAccountModel : PageModel
    {

        public class CustomerViewModel
            {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<Account> Accounts { get; set; }
        }

        private readonly IAccountServices _accountServices;
        private readonly ApplicationDbContext _context;

        public CreateAccountModel(ApplicationDbContext context, IAccountServices accountServices)
        {
            _accountServices = accountServices;
            _context = context;
        }
        public decimal AccountsTotal { get; set; }
        //public int Id { get; set; }

        //public string Name { get; set; }


        [Required]
        [BindProperty]
        public string CustAccountType { get; set; }
        
        public List<SelectListItem> AllAccountTypes { get; set; }

        public CustomerViewModel currentCustomerView = new CustomerViewModel();
        public void OnGet(int custId)
        {
            var currentCust = _context.Customers.Include(x => x.Accounts).First(x => x.Id == custId);
            currentCustomerView.Id = currentCust.Id;
            currentCustomerView.Name = currentCust.Givenname + " " + currentCust.Surname;
            currentCustomerView.Accounts = currentCust.Accounts;
            AccountsTotal = _accountServices.AccTotalAmount(currentCustomerView.Accounts);

            AllAccountTypes = _accountServices.GetAccountTypes();
            AllAccountTypes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Välj en kontotyp"
            });
        }




        public IActionResult OnPost(int custId)
        {
            if (ModelState.IsValid)
            {
                var currentCustomer = _context.Customers.Include(x => x.Accounts).First(x => x.Id == custId);
                currentCustomer.Accounts.Add(new Account() { AccountType = CustAccountType, Created = DateTime.Now, Balance = 0, Transactions = new List<Transaction>() });
                _context.SaveChanges();
                return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = custId });
            }
            return RedirectToPage("/CustomerView/CustomerViewSingle", new { custId = custId });
        }
    }
}
