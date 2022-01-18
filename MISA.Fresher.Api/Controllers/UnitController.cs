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
    public class UnitController : BaseController<Unit>
    {
        public UnitController(IBaseRepository<Unit> baseRepository, IBaseService<Unit> baseService)
            : base(baseRepository, baseService)
        {}
    }
}
