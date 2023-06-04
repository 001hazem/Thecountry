using CrudOpreations.Data;
using CrudOpreations.Models;
using CrudOpreations.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CrudOpreations.Controllers
{
	[Authorize(Roles = "Employee , Admin")]
	public class RegionController : Controller
    {
        private ApplicationDbContext _da;
        private IFileService _fileService;

        public RegionController( ApplicationDbContext da, IFileService fileService) { 
         _da= da;
         _fileService= fileService;

    
        }   
        public IActionResult Index()
        {
            var GetAll= _da.Regions.Include(x => x.Cities).Where(x=> !x.IsDelete).OrderByDescending(x => x.createAt).ToList();

            return View(GetAll);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["SendCountryId"] = new SelectList(_da.Cities.Where(x => !x.IsDelete).ToList(), "Id", "Name");

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(RegionViewModel Input)
        {
            if (ModelState.IsValid)
            {
                var Reperdited = _da.Regions.Any(x => x.Name == Input.Name && !x.IsDelete);

                if (Reperdited)
                {
                    TempData["Message"] = "e: The Item Is Exist";
                    ViewData["SendCountryId"] = new SelectList(_da.Cities.Where(x => !x.IsDelete).ToList(), "Id", "Name");
                    return View(Input);
                }

                var CreateNew = new Region();
                CreateNew.Name = Input.Name;
                CreateNew.CityId = Input.CityId;

                if (Input.Image != null)
                {
                    CreateNew.Image = await _fileService.SaveFile(Input.Image, "Images");
                }

                _da.Regions.Add(CreateNew);
                _da.SaveChanges();
                TempData["Message"] = "s: Country was added successfully";

                return RedirectToAction("Index");

            }

            ViewData["SendCountryId"] = new SelectList(_da.Cities.Where(x => !x.IsDelete).ToList(), "Id", "Name");

            return View();
        }

        [HttpGet]
        public IActionResult Update(int Id)
        {

            var updateData =_da.Regions.SingleOrDefault(x=>!x.IsDelete && x.Id == Id);
            ViewData["SendRegionsId"] = new SelectList(_da.Cities.Where(x => !x.IsDelete).ToList(), "Id", "Name");

            if (updateData == null) 
            {
                return NotFound();
            }
            var vm = new UpdateRegionViewModel();
            updateData.Id=vm.Id; 
            updateData.Name=vm.Name;
            updateData.CityId=vm.CityId;

            return View(vm);
            
           
        }

        public async Task <IActionResult> Update(UpdateRegionViewModel Input)
        {
            if (ModelState.IsValid)
            {
                var updateData = _da.Regions.SingleOrDefault(x => !x.IsDelete && x.Id == Input.Id);
                ViewData["SendRegionsId"] = new SelectList(_da.Cities.Where(x => !x.IsDelete).ToList(), "Id", "Name");

                if (updateData == null)
                {
                    return NotFound();
                }
                
                updateData.Id = Input.Id;
                updateData.Name = Input.Name;
                updateData.CityId = Input.CityId;

                if (Input.Image!=null)
                {
                    updateData.Image = await _fileService.SaveFile(Input.Image, "Images");
                }
                _da.Regions.Update(updateData);
                _da.SaveChanges();
                TempData["Message"] = "s: the Item Is updated successfully";
                return  RedirectToAction("Index");
            }

            ViewData["SendRegionsId"] = new SelectList(_da.Cities.Where(x => !x.IsDelete).ToList(), "Id", "Name");
            return View(Input);

        }

        public IActionResult Delete(int Id)
        {
            var deleteItem = _da.Regions.SingleOrDefault(x => x.Id == Id && !x.IsDelete);
            if (deleteItem == null)
            {
                return NotFound();

            }
            deleteItem.IsDelete = true;
            _da.Regions.Update(deleteItem);
            _da.SaveChanges();
            TempData["Message"] = "s: the Item Is Deleted successfully";
            return RedirectToAction("Index");

        }

    }
}
