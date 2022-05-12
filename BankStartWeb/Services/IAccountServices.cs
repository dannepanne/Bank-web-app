using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BankStartWeb.Services
{
    public interface IAccountServices
    {

        
        public List<SelectListItem> GetAccountTypes();

        decimal AccTotalAmount(List<Account> accList);
        public Errorcode AccountTransfer(int AccountToId, int AccountFromId, decimal TransferSum);

        public Errorcode AccountWithdrawal(int AccountFromId, decimal TransferSum, string operation);

        public Errorcode AccountDeposit(int AccountToId,decimal TransferSum, string operation);

        public Errorcode RegisterCustomerCorrect(Customer cust);




        public enum Errorcode
        {
            ThatWentWell,
            NotEnoughCash,
            IncorrectTargetId,
            OK,
            CantDoThat,
            CantTransferNegativeAmount,
            No
        };

    }



}



//först kolla private bool så att inget konto hamnar på minus. Kör denna som if i metod för öerföring - skapa ny transfer OM bool går igenom

//withdraw function
//deposit functino
//transfer function

//Enum ErrorCode

