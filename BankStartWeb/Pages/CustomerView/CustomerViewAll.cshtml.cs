using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.CustomerView
{
    public class CustomerViewAllModel : PageModel
    {
        private readonly ApplicationDbContext _applicationdDbContext;

        public CustomerViewAllModel(ApplicationDbContext applicationDbContext)
        {
            _applicationdDbContext = applicationDbContext;
        }

        [BindProperty(SupportsGet =true)]
        public string CustSearch { get; set; }
        public List<CustomerListView> CustomersList { get; set; }

        public class CustomerListView
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime BirthDay { get; set; }
            public string Email { get; set; }

        }

        public void OnGet(string col, string order, string custSearch)
        {
            CustSearch = custSearch;
            var c = _applicationdDbContext.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(CustSearch))
            {
                c = c.Where(cust => cust.Givenname.Contains(CustSearch) || cust.Surname.Contains(CustSearch));

            }
            if (col == "email")
            {
                if (order == "asc") c = c.OrderBy(cus => cus.EmailAddress);
                else c = c.OrderByDescending(cus => cus.EmailAddress);
            }

            CustomersList = c.Select(x => new CustomerListView
            {
                Id = x.Id,
                FirstName = x.Givenname,
                LastName = x.Surname,
                Email = x.EmailAddress
            }).ToList();

        }
    }
}