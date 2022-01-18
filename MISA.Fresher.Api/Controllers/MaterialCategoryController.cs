using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repository;
using MISA.CukCuk.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [ApiController]
    public class MaterialCategoryController : BaseController<MaterialCategory>
    {
        public MaterialCategoryController(IBaseRepository<MaterialCategory> baseRepository, IBaseService<MaterialCategory> baseService)
            : base(baseRepository, baseService)
        {

        }
    }
}
