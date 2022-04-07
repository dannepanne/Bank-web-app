using BankStartWeb.Data;

namespace BankStartWeb.Services
{
    public interface IAccountServices
    {

        decimal AccountTransferCalculatorInOut(decimal decacc1, decimal decacc2, string operation);

        bool EnoughBalance(Account acc, int withdraw);
        decimal AccTotalAmount(List<Account> accList);
    }
}