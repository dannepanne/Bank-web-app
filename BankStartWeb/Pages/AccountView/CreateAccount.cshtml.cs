using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.AccountView
{
    public class CreateAccountModel : PageModel
    {
        private readonly IAccountServices _accountServices;
        private readonly ApplicationDbContext _context;

        public CreateAccountModel(ApplicationDbContext context, IAccountServices accountServices)
        {
            _accountServices = accountServices;
            _context = context;
        }
        public decimal AccountsTotal { get; set; }
        public int Id { get; set; }

        public string Name { get; set; }



        [BindProperty]
        public string CustAccountType { get; set; }
        public List<Account> Accounts { get; set; }
        public List<SelectListItem> AllAccountTypes { get; set; }

        public Customer currentCustomer { get; set; }
        public void OnGet(int custId)
        {
            currentCustomer = _context.Customers.Include(x => x.Accounts).First(x => x.Id == custId);
            Id = currentCustomer.Id;
            Name = currentCustomer.Givenname + " " + currentCustomer.Surname;
            Accounts = currentCustomer.Accounts;
            AccountsTotal = _accountServices.AccTotalAmount(Accounts);

            AllAccountTypes = _accountServices.GetAccountTypes();
            AllAccountTypes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Välj en kontotyp"
            });
        }




        public void OnPost()
        {
            currentCustomer.Accounts.Add(new Account() { AccountType = CustAccountType, Created = DateTime.Now, Balance = 0, Transactions = new List<Transaction>() });
        }
    }
}
