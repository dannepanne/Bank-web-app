using BankStartWeb.Data;

namespace BankStartWeb.Services
{
    public interface IAccountServices
    {
        bool EnoughBalance(Account acc, int withdraw);
    }
}