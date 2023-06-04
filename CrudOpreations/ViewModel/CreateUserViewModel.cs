using CrudOpreations.Enums;
using System.ComponentModel.DataAnnotations;

namespace CrudOpreations.ViewModel
{
    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Enter the Email")]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public IFormFile Image { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
		[Required]
		public UserType UsersType { get; set; }



	}
}
