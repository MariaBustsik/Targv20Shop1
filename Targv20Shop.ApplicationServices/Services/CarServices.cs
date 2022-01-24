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
        private readonly IFileServices _file;


        public CarServices
            (
                Targv20ShopDbContext context,
                IFileServices file
            )
        {
            _context = context;
            _file = file;
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
            

            car.Id = Guid.NewGuid();
            car.Name = dto.Name;
            car.Mass = dto.Mass;
            car.Prize = dto.Prize;
            car.Type = dto.Type;
            car.ConstructedAt = dto.ConstructedAt;
            car.CreatedAt = DateTime.Now;
            car.ModifiedAt = DateTime.Now;
            _file.ProcessUploadedFile(dto, car);

            await _context.Car.AddAsync(car);
            await _context.SaveChangesAsync();

            return car;
        }

        public async Task<Car> Update(CarDto dto)
        {
            Car car = new Car();
          

            car.Id = dto.Id;
            car.Name = dto.Name;
            car.Mass = dto.Mass;
            car.Prize = dto.Prize;
            car.Type = dto.Type;
            car.ConstructedAt = dto.ConstructedAt;
            car.CreatedAt = dto.CreatedAt;
            car.ModifiedAt = dto.ModifiedAt;

            _file.ProcessUploadedFile(dto, car);

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

        
        
    }
}
