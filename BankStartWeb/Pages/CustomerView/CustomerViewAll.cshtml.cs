using BankStartWeb.Data;
using BankStartWeb.Infrastrucure.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankStartWeb.Pages.CustomerView
{
    
    public class CustomerViewAllModel : PageModel
    {
        private readonly ApplicationDbContext _applicationdDbContext;

        public CustomerViewAllModel(ApplicationDbContext applicationDbContext)
        {
            _applicationdDbContext = applicationDbContext;
        }

        [BindProperty(SupportsGet = true)]
        public string CustSearch { get; set; }

        
        public List<CustomerListView> CustomersList { get; set; }

        public int PageNo { get; set; }
        public int TotalPageCount { get; set; }


        public string SortOrder { get; set; }
        public string SortCol { get; set; }

        public class CustomerListView
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime BirthDay { get; set; }
            public string Email { get; set; }
            public string City { get; set; }

        }

        public void OnGet(string col = "Id", string order = "asc", int pageno = 1, string custSearch = "")
        {

            PageNo = pageno;
            SortCol = col;
            SortOrder = order;

            CustSearch = custSearch;
            var c = _applicationdDbContext.Customers.AsQueryable();

            

            c = c.OrderBy(col, order == "asc" ? ExtensionMethods.QuerySortOrder.Asc : ExtensionMethods.QuerySortOrder.Desc);
            if (!string.IsNullOrEmpty(CustSearch))
            {
                c = c.Where(cust => cust.Givenname.Contains(CustSearch) || cust.Surname.Contains(CustSearch)|| cust.City.Contains(CustSearch));
            }

            


            var pageResult = c.GetPaged(PageNo, 20);
            TotalPageCount = pageResult.PageCount;

            CustomersList = pageResult.Results.Select(c => new CustomerListView
            {
                Id = c.Id,
                FirstName = c.Givenname,
                LastName = c.Surname,
                Email = c.EmailAddress,
                City = c.City,
            }).ToList();

        }
    }
}
