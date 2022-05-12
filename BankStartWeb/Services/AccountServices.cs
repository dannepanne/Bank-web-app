using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankStartWeb.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly ApplicationDbContext _context;

        public AccountServices(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool TransferFromOk(Account acc, decimal withdraw)
        {
            if (acc.Balance - withdraw >= 0)
                return true;
            else
                return false;
        }

        public decimal AccountTransferCalculatorInOut(decimal sum1, decimal sum2, string operation)
        {
            decimal sum = 0;
            if (operation == "in")
                sum = sum1 + sum2;
            else if (operation == "out")
                sum = sum1 - sum2;
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




        public IAccountServices.Errorcode RegisterCustomerCorrect(Customer cust)
        {
            if(cust.Givenname !=null && cust.Surname != null && cust.Birthday != null && cust.City != null && cust.Country != null && cust.NationalId != null && cust.Streetaddress != null && cust.Zipcode != null)
            {
                return IAccountServices.Errorcode.OK;
            }
            else
            return IAccountServices.Errorcode.No;
        }


        public IAccountServices.Errorcode AccountTransfer(int AccountToId, int AccountFromId, decimal TransferSum)
        {
            if (AccountExists(AccountToId) == IAccountServices.Errorcode.ThatWentWell)
            {
                var accFrom = _context.Accounts.FirstOrDefault(a => a.Id == AccountFromId);
                var accTo = _context.Accounts.FirstOrDefault(a => a.Id == AccountToId);

                if (TransferSum > 0)
                {
                    if (TransferFromOk(accFrom, TransferSum))
                    {

                        Transaction transFrom = new Transaction { Operation = "Transfer", Amount = TransferSum, Date = DateTime.Now, Type = "Debit", NewBalance = accFrom.Balance - TransferSum };
                        Transaction transTo = new Transaction { Operation = "Transfer", Amount = TransferSum, Date = DateTime.Now, Type = "Credit", NewBalance = accTo.Balance + TransferSum };

                        accFrom.Transactions.Add(transFrom);
                        accTo.Transactions.Add(transTo);
                        accFrom.Balance = accFrom.Balance - TransferSum;
                        accTo.Balance = accTo.Balance + TransferSum;
                        _context.SaveChanges();
                        return IAccountServices.Errorcode.ThatWentWell;
                    }
                    return IAccountServices.Errorcode.NotEnoughCash;
                }

                return IAccountServices.Errorcode.CantTransferNegativeAmount;
            }

            return IAccountServices.Errorcode.IncorrectTargetId;
        }

        public IAccountServices.Errorcode AccountDeposit(int AccountToId, decimal TransferSum, string operation)
        {
            if (TransferSum > 0)
            {
                var account = _context.Accounts.FirstOrDefault(a => a.Id == AccountToId);
                account.Balance = account.Balance + TransferSum;
                var transaction = new Transaction
                {
                    Operation = operation,
                    Amount = TransferSum,
                    Date = DateTime.Now,
                    NewBalance = account.Balance,
                    Type = "Credit"
                };
                account.Transactions.Add(transaction);
                
                _context.SaveChanges();
                return IAccountServices.Errorcode.ThatWentWell;
            }

            return IAccountServices.Errorcode.CantTransferNegativeAmount;
        }

        public IAccountServices.Errorcode AccountWithdrawal(int AccountFromId, decimal TransferSum, string operation)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Id == AccountFromId);
            if (account.Balance - TransferSum < 0)
                return IAccountServices.Errorcode.NotEnoughCash;
            
            account.Balance = account.Balance - TransferSum;
            var transaction = new Transaction
            {
            Operation = operation,
            Amount = TransferSum,
            Date = DateTime.Now,
            NewBalance = account.Balance,
            Type = "Debit"
            };
            account.Transactions.Add(transaction);
            _context.SaveChanges();

            return IAccountServices.Errorcode.ThatWentWell;
        }

        public IAccountServices.Errorcode AccountExists(int accId)
        {
            var acc = _context.Accounts.FirstOrDefault(a => a.Id == accId);
            if (acc != null)
            {
                return IAccountServices.Errorcode.ThatWentWell;
            }

            return IAccountServices.Errorcode.IncorrectTargetId;
        }

    }
}
