using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Targv20Shop.Core.Dtos;
using Targv20Shop.Core.ServiceInterface;
using Targv20Shop.Data;
using Targv20Shop.Models.Car;
using Targv20Shop.Models.Files;

namespace Targv20Shop.Controllers
{
    public class CarController : Controller
    {

        private readonly Targv20ShopDbContext _context;
        private readonly ICarService _carService;
        private readonly IFileServices _fileService;

        public CarController
            (
                Targv20ShopDbContext context,
                ICarService carService,
                IFileServices fileService
            )
        {
            _context = context;
            _carService = carService;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _context.Car
                .Select(x => new CarListViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Mass = x.Mass,
                    Prize = x.Prize,
                    Type = x.Type,
                    ConstructedAt = x.ConstructedAt,
                    CreatedAt = x.CreatedAt,
                    ModifiedAt = x.ModifiedAt
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            CarViewModel model = new CarViewModel();

            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CarViewModel model)
        {
            var dto = new CarDto()
            {
                Id = model.Id,
                ConstructedAt = model.ConstructedAt,
                Mass = model.Mass,
                Name = model.Name,
                Prize = model.Prize,
                Type = model.Type,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
                Files = model.Files,
                ExistingFilePaths = model.ExistingFilePaths
                .Select(x => new ExistingFilePathDto
                {
                    PhotoId = x.PhotoId,
                    FilePath = x.FilePath,
                    CarId = x.CarId
                }).ToArray()
            };

            var result = await _carService.Add(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var car = await _carService.Edit(id);
            if (car == null)
            {
                return NotFound();
            }

            var photos = await _context.ExistingFilePath
                .Where(x => x.CarId == id)
                .Select(y => new ExistingFilePathViewModel
                {
                    FilePath = y.FilePath,
                    PhotoId = y.Id
                })
                .ToArrayAsync();


            var model = new CarViewModel();

            model.Id = car.Id;
            model.Mass = car.Mass;
            model.Name = car.Name;
            model.Prize = car.Prize;
            model.Type = car.Type;
            model.ConstructedAt = car.ConstructedAt;
            model.CreatedAt = car.CreatedAt;
            model.ModifiedAt = car.ModifiedAt;
            model.ExistingFilePaths.AddRange(photos);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CarViewModel model)
        {
            var dto = new CarDto()
            {
                Id = model.Id,
                Mass = model.Mass,
                Name = model.Name,
                ConstructedAt = model.ConstructedAt,
                Prize = model.Prize,
                Type = model.Type,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
                Files = model.Files,
                ExistingFilePaths = model.ExistingFilePaths
                .Select(x => new ExistingFilePathDto
                {
                    PhotoId = x.PhotoId,
                    FilePath = x.FilePath,
                    CarId = x.CarId
                })
            };

            var result = await _carService.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var car = await _carService.Delete(id);

            if (car == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(ExistingFilePathViewModel model)
        {
            var dto = new ExistingFilePathDto()
            {
                FilePath = model.FilePath
            };

            var image = await _fileService.RemoveImage(dto);
            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
