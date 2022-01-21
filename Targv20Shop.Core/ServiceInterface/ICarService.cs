using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Targv20Shop.Core.Domain;
using Targv20Shop.Core.Dtos;

namespace Targv20Shop.Core.ServiceInterface
{
    public interface ICarService
    {
        Task<Car> Edit(Guid id);
        Task<Car> Add(CarDto dto);
        Task<Car> Update(CarDto dto);
        Task<Car> Delete(Guid id);
    }
}
