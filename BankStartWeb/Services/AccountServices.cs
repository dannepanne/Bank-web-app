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

        public decimal AccountTransferCalculatorInOut(decimal decacc1, decimal decacc2, string operation)
        {
            decimal sum = 0;
            if (operation == "in")
                sum = decacc1 + decacc2;
            else if (operation == "out")
                sum= decacc1 - decacc2;
            else
                sum = 0;
            return sum;
        }

        public decimal AccTotalAmount(List<Account> accList)
        {
            decimal sum = accList.Sum(x => x.Balance);
            return sum;
        }

    }
}
