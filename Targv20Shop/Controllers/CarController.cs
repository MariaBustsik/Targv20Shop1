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

namespace Targv20Shop.Controllers
{
    public class CarController : Controller
    {

        private readonly Targv20ShopDbContext _context;
        private readonly ICarService _carService;

        public CarController
            (
                Targv20ShopDbContext context,
                ICarService carService
            )
        {
            _context = context;
            _carService = carService;
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
                    Crew = x.Crew,
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
                Crew = model.Crew,
                ConstructedAt = model.ConstructedAt,
                Mass = model.Mass,
                Name = model.Name,
                Prize = model.Prize,
                Type = model.Type,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
                Files = model.Files,
                Image = model.Image.Select(x => new FileToDatabaseDto
                {
                    Id = x.Id,
                    ImageData = x.ImageData,
                    ImageTitle = x.ImageTitle,
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

            var photos = await _context.FileToDatabase
                .Where(x => x.CarId == id)
                .Select(m => new ImagesViewModel
                {
                    ImageData = m.ImageData,
                    Id = m.Id,
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(m.ImageData)),
                    ImageTitle = m.ImageTitle,
                    CarId = m.Id
                }).ToArrayAsync();

            var model = new CarViewModel();

            model.Id = car.Id;
            model.Mass = car.Mass;
            model.Name = car.Name;
            model.Prize = car.Prize;
            model.Type = car.Type;
            model.Crew = car.Crew;
            model.ConstructedAt = car.ConstructedAt;
            model.CreatedAt = car.CreatedAt;
            model.ModifiedAt = car.ModifiedAt;
            model.Image.AddRange(photos);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CarViewModel model)
        {
            var dto = new CarDto()
            {
                Id = model.Id,
                Crew = model.Crew,
                Mass = model.Mass,
                Name = model.Name,
                ConstructedAt = model.ConstructedAt,
                Prize = model.Prize,
                Type = model.Type,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
                Image = model.Image.Select(x => new FileToDatabaseDto
                {
                    Id = x.Id,
                    ImageData = x.ImageData,
                    ImageTitle = x.ImageTitle,
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
    }
}
