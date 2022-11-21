using Microsoft.AspNetCore.Identity;

namespace Backend_AYNI.Models
{
    public class UserModel : IdentityUser
    {
        public ICollection<ForumModel> Forums { get; set; }
    }
}
