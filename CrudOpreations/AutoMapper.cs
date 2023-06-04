using AutoMapper;
using CrudOpreations.Models;
using CrudOpreations.ViewModel;

namespace CrudOpreations
{
    public class AutoMapper :Profile
    {
        public AutoMapper() {
            CreateMap<User, UserViewModel>();
            CreateMap<CreateUserViewModel, User>();
        }
    }
}
