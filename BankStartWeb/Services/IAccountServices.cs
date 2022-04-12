using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankStartWeb.Services
{
    public interface IAccountServices
    {

        decimal AccountTransferCalculatorInOut(decimal decacc1, decimal decacc2, string operation);
        public List<SelectListItem> GetAccountTypes();

        decimal AccTotalAmount(List<Account> accList);
    }
    

   
}



//först kolla private bool så att inget konto hamnar på minus. Kör denna som if i metod för öerföring - skapa ny transfer OM bool går igenom

//withdraw function
//deposit functino
//transfer function

//Enum ErrorCode

