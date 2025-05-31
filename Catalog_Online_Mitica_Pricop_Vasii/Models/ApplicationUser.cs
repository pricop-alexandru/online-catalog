using Microsoft.AspNetCore.Identity;

namespace Catalog_Online_Mitica_Pricop_Vasii.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; } = string.Empty;
    }
}
