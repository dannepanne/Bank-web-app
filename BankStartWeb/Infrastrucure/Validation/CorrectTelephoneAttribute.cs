using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Infrastrucure.Validation
{
    public class CorrectTelephoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var allowableChars = "0123456789- ";
            string valuestring = value.ToString();
            foreach (var c in valuestring)
            {
                if (!allowableChars.Contains(c.ToString()))
                {
                    return false;
                }
            }

            return true;
        }


        
}
}
