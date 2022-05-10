using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Infrastrucure.Validation
{
    public class CorrectBirthDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var date = DateTime.Now;
            DateTime dt = DateTime.Parse(value.ToString()); 
            if (dt > date)
                return false;
            return true;
        }
    }
}
