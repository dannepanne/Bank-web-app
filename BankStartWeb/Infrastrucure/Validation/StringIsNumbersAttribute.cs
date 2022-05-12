using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Infrastrucure.Validation
{
    public class StringIsNumbersAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string valuestring = value.ToString();
            return valuestring.All(Char.IsDigit);
        }

    }
}
