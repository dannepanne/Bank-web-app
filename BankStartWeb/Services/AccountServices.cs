using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankStartWeb.Services
{
    public class AccountServices : IAccountServices
    {

        private bool TransferOk(Account acc, int withdraw)
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
                sum = decacc1 - decacc2;
            else
                sum = 0;
            return sum;
        }

        public decimal AccTotalAmount(List<Account> accList)
        {
            decimal sum = accList.Sum(x => x.Balance);
            return sum;
        }

        public List<SelectListItem> GetAccountTypes()
        {
            var returnlist = Enum.GetValues<AccountTypes>().Select(a => new SelectListItem
            {
                Text = a.ToString(),
                Value = a.ToString()
            }).ToList();

            return returnlist;
        }




        public enum Errorcode
        {
            ThatWentWell,
            NotEnoughCash,
        };
    }
}
