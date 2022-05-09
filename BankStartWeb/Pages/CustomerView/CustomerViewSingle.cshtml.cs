using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.CustomerView
{
    public class CustomerViewSingleModel : PageModel
    {
        public class CustomerViewSingleViewModel
        {
        public decimal AccountsTotal { get; set; }
        public int Id { get; set; }
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string Streetaddress { get; set; }


        public List<Account> Accounts { get; set; }
        }

        public CustomerViewSingleViewModel currentCustomerSingleViewModel  = new CustomerViewSingleViewModel();

        private readonly ApplicationDbContext _context;
        private readonly IAccountServices _accountServices;

        public CustomerViewSingleModel(ApplicationDbContext context, IAccountServices accountServices)
        {
            _context = context;
            _accountServices = accountServices;
        }

        

        public void OnGet(int custId)
        {
            
            var currentCustomer = _context.Customers.Include(x => x.Accounts).First(x => x.Id == custId);
            currentCustomerSingleViewModel.Id = currentCustomer.Id;
            currentCustomerSingleViewModel.Givenname = currentCustomer.Givenname;
            currentCustomerSingleViewModel.Surname = currentCustomer.Surname;
            currentCustomerSingleViewModel.Streetaddress = currentCustomer.Streetaddress;
            currentCustomerSingleViewModel.Accounts = currentCustomer.Accounts;
            currentCustomerSingleViewModel.AccountsTotal = _accountServices.AccTotalAmount(currentCustomerSingleViewModel.Accounts);
        }

    }
}
