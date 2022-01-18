using MISA.CukCuk.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Interfaces.Service
{
    public interface IMaterialService : IBaseService<Material>
    {
        string GetNewCode(string materialName);
        
    }
}
