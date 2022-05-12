using BankStartWeb.Data;

namespace BankStartWeb.ViewModels
{
    public class CustomerTransactionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
