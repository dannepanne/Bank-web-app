using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankStartWeb.Pages.CustomerView
{
    public class CustomerViewSingleModel : PageModel
    {
        
            public int Id { get; set; }
            public string Givenname { get; set; }
            public string Surname { get; set; }
            public string Streetaddress { get; set; }


            private readonly ApplicationDbContext _context;

            public CustomerViewSingleModel(ApplicationDbContext context)
            {
                _context = context;
            }
            public void OnGet(int custId)
            {

                Customer currentCustomer = new Customer();

                currentCustomer = _context.Customers.First(x => x.Id == custId);
                Id = currentCustomer.Id;
                Givenname = currentCustomer.Givenname;
                Surname = currentCustomer.Surname;
                Streetaddress = currentCustomer.Streetaddress;

            }
        
    }
}
