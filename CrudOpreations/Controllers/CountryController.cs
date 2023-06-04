using CrudOpreations.Data;
using CrudOpreations.Models;
using CrudOpreations.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrudOpreations.Controllers
{
    [Authorize (Roles = "Employee , Admin")]
    public class CountryController : Controller
    {
        private ApplicationDbContext _da;
        private IFileService _fileService;

        public CountryController(ApplicationDbContext da, IFileService fileService)
        {
            _da = da;
            _fileService = fileService;


        }
        public IActionResult Index()
        {
            var GetAll = _da.Countries.Where(x => !x.IsDelete).OrderByDescending(x => x.createAt).ToList();

            return View(GetAll);
        }

        [HttpGet]
        public IActionResult Update(int Id)
        {
            var updateCountry = _da.Countries.SingleOrDefault(x => x.Id == Id && !x.IsDelete);

            if(updateCountry== null)
            {
                return NotFound();
            }
            var vm = new UpdateCountryViewModel();
            vm.Id = updateCountry.Id;
            vm.Name = updateCountry.Name;

            return View(vm);
        }
        [HttpPost]
        public IActionResult Update(UpdateCountryViewModel Input)
        {
            if (ModelState.IsValid)
            {
            var updateCountry = _da.Countries.SingleOrDefault(x => x.Id == Input.Id && !x.IsDelete);

            if (updateCountry == null)
            {
                return NotFound();
            }
            updateCountry.Name = Input.Name;
            _da.Countries.Update(updateCountry);
            _da.SaveChanges();

            TempData["Message"] = "w: The Iteam was updated successfully";

            return RedirectToAction("Index");
            }
            return View(Input);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var updateCountry = _da.Countries.SingleOrDefault(x => x.Id == Id && !x.IsDelete);

            if (updateCountry == null)
            {
                return NotFound();
            }

            updateCountry.IsDelete = true;
            _da.Countries.Update(updateCountry); 
            _da.SaveChanges();

            TempData["Message"] = "s: The Item Is Delete";
            return RedirectToAction("Index");

        }


        [HttpGet]
        public IActionResult Create() { 
           
            return View();
        
        }

        [HttpPost]
        public IActionResult Create(CountryViewModel Input)
        {
            if(ModelState.IsValid)
            {
                var Reperdited = _da.Countries.Any(x => x.Name == Input.Name && !x.IsDelete);

                if(Reperdited)
                {
                    TempData["Message"] = "e: The Item Is Exist";

                    return View(Input);
                }

                var CreateNew = new Country();
                CreateNew.Name = Input.Name;
                _da.Countries.Add(CreateNew);
                _da.SaveChanges();

                TempData["Message"] = "s: Country was added successfully";
                return RedirectToAction("Index");   

            }
            return View();
        }

    }
}
