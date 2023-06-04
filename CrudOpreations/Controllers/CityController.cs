using CrudOpreations.Data;
using CrudOpreations.Models;
using CrudOpreations.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrudOpreations.Controllers
{
	[Authorize(Roles = "Employee , Admin")]
	public class CityController : Controller
    {
        private ApplicationDbContext _da;
        private IFileService _fileService;

        public CityController(ApplicationDbContext da, IFileService fileService)
        {
            _da = da;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            var GetAll = _da.Cities.Include(x=>x.Countries).Where(x => !x.IsDelete).OrderByDescending(x => x.createAt).ToList();

            return View(GetAll);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["SendCountryId"]=new SelectList(_da.Countries.Where(x=>!x.IsDelete).ToList(),"Id","Name");

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(CityViewModel Input)
        {
            if (ModelState.IsValid)
            {
                var Reperdited = _da.Cities.Any(x => x.Name == Input.Name && !x.IsDelete);

                if (Reperdited)
                {
                    TempData["Message"] = "e: The Item Is Exist";
                    ViewData["SendCountryId"] = new SelectList(_da.Countries.Where(x => !x.IsDelete).ToList(), "Id", "Name");
                    return View(Input);
                }

                var CreateNew = new City();
                CreateNew.Name = Input.Name;
                CreateNew.CountryId= Input.CountryId;

                if (Input.Image != null)
                {
                    CreateNew.Image =await _fileService.SaveFile(Input.Image, "Images");
                }
                
                _da.Cities.Add(CreateNew);
                _da.SaveChanges();
                TempData["Message"] = "s: Country was added successfully";
                return RedirectToAction("Index");

            }
            ViewData["SendCountryId"] = new SelectList(_da.Countries.Where(x => !x.IsDelete).ToList(), "Id", "Name");
            return View();
        }




        [HttpGet]
        public async Task <IActionResult> Update(int Id)
        {
           var updateCity=_da.Cities.SingleOrDefault(x => x.Id == Id&& !x.IsDelete);
            ViewData["SendCountryId"] = new SelectList(_da.Countries.Where(x => !x.IsDelete).ToList(), "Id", "Name");

            if (updateCity == null)
            {
                return NotFound();

            }
            var vm = new UpdateCityViewModel();
            updateCity.Id = vm.Id;
            updateCity.Name=vm.Name;

            updateCity.CountryId= vm.CountryId;
            if (vm.Image != null)
            {
                updateCity.Image = await _fileService.SaveFile(vm.Image, "Images");
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCityViewModel Input)
        {
            if(ModelState.IsValid)
            {

            
            var updateCity = _da.Cities.SingleOrDefault(x => x.Id == Input.Id && !x.IsDelete);

            if (updateCity == null)
            {
                return NotFound();

            }
            
            updateCity.Id = Input.Id;
            updateCity.Name = Input.Name;
            updateCity.CountryId = Input.CountryId;
            if (Input.Image != null)
            {
                updateCity.Image = await _fileService.SaveFile(Input.Image, "Images");
            }
                _da.Cities.Update(updateCity);
                _da.SaveChanges();
                TempData["Message"] = "s: City was Updated successfully";
                return RedirectToAction("Index");


            }
            ViewData["SendCountryId"] = new SelectList(_da.Countries.Where(x => !x.IsDelete).ToList(), "Id", "Name");
            return View(Input);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var updateCity = _da.Cities.SingleOrDefault(x => x.Id == Id && !x.IsDelete);

            if (updateCity == null)
            {
                return NotFound();
            }
            
            updateCity.IsDelete = true;
            _da.Cities.Update(updateCity);
            _da.SaveChanges();
            TempData["Message"] = "s: The Item Is Delete";
            return RedirectToAction("Index");
        }

    }
}
