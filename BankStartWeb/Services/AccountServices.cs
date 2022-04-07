using BankStartWeb.Data;

namespace BankStartWeb.Services
{
    public class AccountServices : IAccountServices
    {

        public bool EnoughBalance(Account acc, int withdraw)
        {
            if (acc.Balance - withdraw >= 0)
                return true;
            else
                return false;
        }



    }
}
