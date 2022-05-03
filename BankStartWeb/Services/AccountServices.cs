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




        //public bool RegisterCustomerCorrect(Customer cust)
        //{
        //    if(_context.Customers.FirstOrDefault(c => c.Id == cust.Id) != null)
        //    {

        //        return true;
        //    }
        //    else
        //    return false;
        //}


        public IAccountServices.Errorcode AccountTransfer(int AccountToId, int AccountFromId, decimal TransferSum)
        {
            var accFrom = _context.Accounts.FirstOrDefault(a => a.Id == AccountFromId);
            var accTo = _context.Accounts.FirstOrDefault(a => a.Id == AccountToId);

            if (TransferFromOk(accFrom, TransferSum))
            {
          
                Transaction transFrom = new Transaction { Operation = "Transfer", Amount = TransferSum, Date = DateTime.Now, Type = "Debit", NewBalance = accFrom.Balance - TransferSum };
                Transaction transTo = new Transaction { Operation = "Transfer", Amount = TransferSum, Date = DateTime.Now, Type = "Credit", NewBalance = accTo.Balance + TransferSum };

                accFrom.Transactions.Add(transFrom);
                accTo.Transactions.Add(transTo);

                _context.SaveChanges();
                return IAccountServices.Errorcode.ThatWentWell;
            }
            
                return IAccountServices.Errorcode.NotEnoughCash;
        }

        public IAccountServices.Errorcode AccountDeposit(int AccountToId, decimal TransferSum)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Id == AccountToId);
            account.Balance += TransferSum;
            var transaction = new Transaction
            {
                Operation = "Deposit",
                Amount = TransferSum,
                Date = DateTime.Now,
                NewBalance = account.Balance += TransferSum,
                Type = "Credit"
            };
            account.Transactions.Add(transaction);
            _context.SaveChanges();
            return IAccountServices.Errorcode.ThatWentWell;
        }

        public IAccountServices.Errorcode AccountWithdrawal(int AccountFromId, decimal TransferSum)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Id == AccountFromId);
            if (account.Balance - TransferSum < 0)
                return IAccountServices.Errorcode.NotEnoughCash;
            
            account.Balance -= TransferSum;
            var transaction = new Transaction
            {
            Operation = "Withdrawal",
            Amount = TransferSum,
            Date = DateTime.Now,
            NewBalance = account.Balance -= TransferSum,
            Type = "Debit"
            };
            account.Transactions.Add(transaction);
            _context.SaveChanges();

            return IAccountServices.Errorcode.ThatWentWell;
        }
        


    }
}
//public string OperationType { get; set; }
//[BindProperty]
//public int AccountTo { get; set; }
//[BindProperty]
//public int AccountFrom { get; set; }
//[BindProperty]
//public int TransferSum { get; set; }
//public string Type { get; set; } //CashDeposit osv
//public string TransactionType { get; set; } //Debit & Credit
//public DateTime Date { get; set; }
//public decimal NewBalance { get; set; }
