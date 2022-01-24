using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using Targv20Shop.Core.Domain;
using Targv20Shop.Core.Dtos;
using Targv20Shop.Core.ServiceInterface;
using Targv20Shop.Data;

namespace Targv20Shop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly Targv20ShopDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FileServices
            (
                Targv20ShopDbContext context,
                IWebHostEnvironment env
            )
        {
            _context = context;
            _env = env;
        }


        public async Task<ExistingFilePath> RemoveImage(ExistingFilePathDto dto)
        {
            var imageId = await _context.ExistingFilePath
                .FirstOrDefaultAsync(x => x.FilePath == dto.FilePath);

            string photoPath = _env.WebRootPath + "\\multipleFileUpload\\" + dto.FilePath;

            File.Delete(photoPath);

            _context.ExistingFilePath.Remove(imageId);
            await _context.SaveChangesAsync();

            return imageId;
        }

        public async Task<ExistingFilePath> RemoveImages(ExistingFilePathDto[] dto)
        {
            foreach (var dtos in dto)
            {
                var fileId = await _context.ExistingFilePath
                    .FirstOrDefaultAsync(x => x.FilePath == dtos.FilePath);

                string photoPath = _env.WebRootPath + "\\multipleFileUpload\\" + dtos.FilePath;

                File.Delete(photoPath);

                _context.ExistingFilePath.Remove(fileId);
                await _context.SaveChangesAsync();
            }
            return null;
        }

        public string ProcessUploadedFile(ProductDto dto, Product product)
        {
            string uniqueFileName = null;

            if (dto.Files != null && dto.Files.Count > 0)
            {
                if (!Directory.Exists(_env.WebRootPath + "\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_env.WebRootPath + "\\multipleFileUpload\\");
                }

                foreach (var photo in dto.Files)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "multipleFileUpload");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);

                        ExistingFilePath paths = new ExistingFilePath
                        {
                            Id = Guid.NewGuid(),
                            FilePath = uniqueFileName,
                            ProductId = product.Id
                        };

                        _context.ExistingFilePath.Add(paths);
                    }
                }
            }

            return uniqueFileName;
        }

        public string ProcessUploadedFile(CarDto dto, Car car)
        {
            string uniqueFileName = null;

            if (dto.Files != null && dto.Files.Count > 0)
            {
                if (!Directory.Exists(_env.WebRootPath + "\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_env.WebRootPath + "\\multipleFileUpload\\");
                }

                foreach (var photo in dto.Files)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "multipleFileUpload");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);

                        ExistingFilePath paths = new ExistingFilePath
                        {
                            Id = Guid.NewGuid(),
                            FilePath = uniqueFileName,
                            CarId = car.Id
                        };

                        _context.ExistingFilePath.Add(paths);
                    }
                }
            }

            return uniqueFileName;
        }



    }
}
