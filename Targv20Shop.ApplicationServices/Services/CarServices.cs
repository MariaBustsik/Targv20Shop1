using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Targv20Shop.Core.Domain;
using Targv20Shop.Core.Dtos;
using Targv20Shop.Core.ServiceInterface;
using Targv20Shop.Data;

namespace Targv20Shop.ApplicationServices.Services
{
    public class CarServices : ICarService
    {
        private readonly Targv20ShopDbContext _context;


        public CarServices
            (
                Targv20ShopDbContext context
            )
        {
            _context = context;
        }

        public async Task<Car> Edit(Guid id)
        {
            var result = await _context.Car
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<Car> Add(CarDto dto)
        {
            Car car = new Car();
            FileToDatabase file = new FileToDatabase();

            car.Id = Guid.NewGuid();
            car.Name = dto.Name;
            car.Mass = dto.Mass;
            car.Prize = dto.Prize;
            car.Type = dto.Type;
            car.Crew = dto.Crew;
            car.ConstructedAt = dto.ConstructedAt;
            car.CreatedAt = DateTime.Now;
            car.ModifiedAt = DateTime.Now;

            if (dto.Files != null)
            {
                file.ImageData = UploadFile(dto, car);
            }

            await _context.Car.AddAsync(car);
            await _context.SaveChangesAsync();

            return car;
        }

        public async Task<Car> Update(CarDto dto)
        {
            Car car = new Car();
            FileToDatabase file = new FileToDatabase();

            car.Id = dto.Id;
            car.Name = dto.Name;
            car.Mass = dto.Mass;
            car.Prize = dto.Prize;
            car.Type = dto.Type;
            car.Crew = dto.Crew;
            car.ConstructedAt = dto.ConstructedAt;
            car.CreatedAt = dto.CreatedAt;
            car.ModifiedAt = dto.ModifiedAt;

            if (dto.Files != null)
            {
                file.ImageData = UploadFile(dto, car);
            }

            _context.Car.Update(car);
            await _context.SaveChangesAsync();

            return car;
        }

        public async Task<Car> Delete(Guid id)
        {
            var car = await _context.Car
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Car.Remove(car);
            await _context.SaveChangesAsync();

            return car;
        }

        public byte[] UploadFile(CarDto dto, Car car)
        {

            if (dto.Files != null && dto.Files.Count > 0)
            {
                foreach (var photo in dto.Files)
                {
                    using (var target = new MemoryStream())
                    {
                        FileToDatabase files = new FileToDatabase
                        {
                            Id = Guid.NewGuid(),
                            ImageTitle = photo.FileName,
                            CarId = car.Id
                        };

                        photo.CopyTo(target);
                        files.ImageData = target.ToArray();

                        _context.FileToDatabase.Add(files);
                    }
                }
            }
            return null;
        }
    }
}
