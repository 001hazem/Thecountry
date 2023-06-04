using CrudOpreations.Enums;
using Microsoft.AspNetCore.Identity;

namespace CrudOpreations.Models
{
    public class User: IdentityUser
    {
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDelete { get; set; }
        public UserType UsersType { get; set; }
    }
}
