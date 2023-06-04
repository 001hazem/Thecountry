using AutoMapper;
using AutoMapper.Configuration.Annotations;
using CrudOpreations.Data;
using CrudOpreations.Enums;
using CrudOpreations.Models;
using CrudOpreations.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrudOpreations.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private ApplicationDbContext _da;
        private UserManager<User> _userManager;
        private IFileService _fileService;
        private RoleManager<IdentityRole> _roleManager;
        private IMapper _mapper;

        public UserController(ApplicationDbContext da, UserManager<User> userManager, IFileService fileService,RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _da = da;
            _userManager = userManager;
            _fileService = fileService;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
           var user =_da.Users.Where(x=>!x.IsDelete)
            //.Select(x=>new UserViewModel()
            //{
            //    Id = x.Id,
            //    PhoneNumber= x.PhoneNumber,
            //    UserName= x.UserName,
            //    CreatedAt   = x.CreatedAt,
            //    Email= x.Email,
            //})
            .ToList();
            var usersVm= _mapper.Map<List<User>,List<UserViewModel>>(user);

            return View(usersVm);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel Input)
        {
            if(ModelState.IsValid) { 

            var ReperditedUser = _da.Users.Any(x=>x.Email== Input.Email &&!x.IsDelete);
                if(ReperditedUser)
                {
                    TempData["Message"] = "e: The User Is Exist";
                    return View(Input);
                }


             var careetUser = _mapper.Map<CreateUserViewModel, User>(Input);
         
            careetUser.UserName = Input.Email;
            
            careetUser.CreatedAt = DateTime.Now;

            if(Input.Image != null)
            {
                careetUser.Image = await _fileService.SaveFile(Input.Image, "Images");
            }

				await _userManager.CreateAsync(careetUser, Input.Password);

                if (Input.UsersType == Enums.UserType.Admin)
                {
                    await _userManager.AddToRoleAsync(careetUser, "Admin");
                }
                else if (Input.UsersType == Enums.UserType.Employee)
                {
                    await _userManager.AddToRoleAsync(careetUser, "Employee");
                }

                TempData["Message"] = "s: User Is Added successfully";

            return RedirectToAction("Index");

            }

            return View(Input);
        }





        public IActionResult Delete(string id) {
            var deleteUser =_da.Users.SingleOrDefault(x=>x.Id==id&&!x.IsDelete);

            if(deleteUser==null)
            {
                return NotFound();
            }
            deleteUser.IsDelete = true;
            _da.Users.Update(deleteUser);
            _da.SaveChanges();

            TempData["Message"] = "s: User Is deieted successfully";

            return RedirectToAction("Index");
        
        }

		public async Task<IActionResult> InitRoles()
		{
			if (!_da.Roles.Any())
			{
				var roles = new List<string>();
				roles.Add("Admin");
				roles.Add("Employee");

				foreach (var role in roles)
				{
					await _roleManager.CreateAsync(new IdentityRole(role));
				}
			}
			return RedirectToAction("Index");

		}


	}
}
