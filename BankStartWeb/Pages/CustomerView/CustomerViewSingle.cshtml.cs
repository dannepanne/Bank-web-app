using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.CustomerView
{
    public class CustomerViewSingleModel : PageModel
    {
        
            public int Id { get; set; }
            public string Givenname { get; set; }
            public string Surname { get; set; }
            public string Streetaddress { get; set; }

             public List<Account> Accounts { get; set; }

             private readonly ApplicationDbContext _context;

            public CustomerViewSingleModel(ApplicationDbContext context)
            {
                _context = context;
            }

       

        public void OnGet(int custId)
            {

                //Customer currentCustomer = new Customer();
                
                var currentCustomer = _context.Customers.Include(x => x.Accounts).First(x => x.Id == custId);
                Id = currentCustomer.Id;
                Givenname = currentCustomer.Givenname;
                Surname = currentCustomer.Surname;
                Streetaddress = currentCustomer.Streetaddress;
                Accounts = currentCustomer.Accounts;
            }
        
    }
}
