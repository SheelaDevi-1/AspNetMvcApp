using Automobile_Speed.Data;
using Automobile_Speed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automobile_Speed.Controllers
{
	public class BrandController : Controller
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public BrandController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
		{
			_dbContext = dbContext;
			_webHostEnvironment = webHostEnvironment;
		}

		[HttpGet]

		public IActionResult Index()
		{
			var brands = _dbContext.Brands.ToList();
			return View(brands);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Brand brand)
		{
			string webRootPath = _webHostEnvironment.WebRootPath;
			IFormFileCollection file = HttpContext.Request.Form.Files;

			if (file.Count > 0)
			{
				string newfilename = Guid.NewGuid().ToString();
				string upload = Path.Combine(webRootPath, @"image\brand");
				string extension = Path.GetExtension(file[0].FileName);

				using (FileStream fileStream = new FileStream(Path.Combine(upload, newfilename + extension), FileMode.Create))
				{
					file[0].CopyTo(fileStream);
				}

				brand.BrandLogo = @"\image\brand\" + newfilename + extension;
			}

			if (ModelState.IsValid)
			{
				_dbContext.Brands.Add(brand);
				_dbContext.SaveChanges();
				TempData["success"] = "Record Created successfully";
				return RedirectToAction(nameof(Index));
			}

			return View(brand);
		}

		[HttpGet]
		public IActionResult Details(Guid id)
		{
			Brand? brand = _dbContext.Brands.FirstOrDefault(b => b.Id == id);
			if (brand == null)
			{
				return NotFound();
			}
			return View(brand);
		}

		[HttpGet]
		public IActionResult Edit(Guid id)
		{
			Brand? brand = _dbContext.Brands.FirstOrDefault(b => b.Id == id);
			if (brand == null)
			{
				return NotFound();
			}
			return View(brand);
		}

		[HttpPost]
		public IActionResult Edit(Brand brand)
		{
			string webRootPath = _webHostEnvironment.WebRootPath;
			IFormFileCollection file = HttpContext.Request.Form.Files;

			if (file.Count > 0)
			{
				string newfilename = Guid.NewGuid().ToString();
				string upload = Path.Combine(webRootPath, @"image\brand");
				string extension = Path.GetExtension(file[0].FileName);

				Brand? objFromDb = _dbContext.Brands.AsNoTracking()
								  .FirstOrDefault(b => b.Id == brand.Id);

				if (objFromDb != null && objFromDb.BrandLogo != null)
				{
					string oldImagePath = Path.Combine(webRootPath, objFromDb.BrandLogo.Trim('\\'));
					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}
				}

				using (FileStream fileStream = new FileStream(Path.Combine(upload, newfilename + extension), FileMode.Create))
				{
					file[0].CopyTo(fileStream);
				}

				brand.BrandLogo = @"\image\brand\" + newfilename + extension;
			}

			if (ModelState.IsValid)
			{
				Brand? objFromDb = _dbContext.Brands.AsNoTracking()
								  .FirstOrDefault(b => b.Id == brand.Id);

				if (objFromDb == null)
				{
					return NotFound();
				}

				objFromDb.Name = brand.Name;
				objFromDb.EstablishedYear = brand.EstablishedYear;

				if (!string.IsNullOrEmpty(brand.BrandLogo))
				{
					objFromDb.BrandLogo = brand.BrandLogo;
				}

				_dbContext.Brands.Update(objFromDb);
				_dbContext.SaveChanges();

				TempData["warning"] = "Record updated Successfully";
				return RedirectToAction(nameof(Index));
			}

			return View(brand);
		}

		[HttpGet]
		public IActionResult Delete(Guid id)
		{
			Brand? brand = _dbContext.Brands.FirstOrDefault(b => b.Id == id);
			if (brand == null)
			{
				return NotFound();
			}
			return View(brand);
		}

		[HttpPost]
		public IActionResult Delete(Brand brand)
		{
			string webRootPath = _webHostEnvironment.WebRootPath;
			Brand? objFromDb = _dbContext.Brands.AsNoTracking().FirstOrDefault(b => b.Id == brand.Id);

			if (objFromDb == null)
			{
				return NotFound();
			}

			if (!string.IsNullOrEmpty(objFromDb.BrandLogo))
			{
				string oldImagePath = Path.Combine(webRootPath, objFromDb.BrandLogo.Trim('\\'));
				if (System.IO.File.Exists(oldImagePath))
				{
					System.IO.File.Delete(oldImagePath);
				}
			}

			_dbContext.Brands.Remove(objFromDb);
			_dbContext.SaveChanges();
			TempData["error"] = "Record Deleted Successfully";
			return RedirectToAction(nameof(Index));
		}
	}
}
