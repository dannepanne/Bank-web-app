using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BankStartWeb.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageRolesModel : PageModel
    {
        [BindProperty]
        public string epost { get; set; }
        [BindProperty]
        public string password { get; set; }
        [BindProperty]
        [Compare(nameof(password))]
        public string password2 { get; set; }

        public string role { get; set; }

        private readonly UserManager<IdentityUser> _userManager;


        public ManageRolesModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                CreateUserIfNotExists(epost, password, new string[] { role });
                
            }

            return Page();
        }

        private void CreateUserIfNotExists(string email, string password, string[] roles)
        {
        if (_userManager.FindByEmailAsync(epost).Result != null) ;

        var user = new IdentityUser { UserName = epost, Email = epost, EmailConfirmed = true };


        _userManager.CreateAsync(user, password).Wait();
        _userManager.AddToRolesAsync(user, roles).Wait();
        }
    }
}
