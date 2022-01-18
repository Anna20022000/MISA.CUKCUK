using MISA.CukCuk.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Interfaces.Repository
{
    public interface IMaterialRepository : IBaseRepository<Material>
    {

        /// <summary>
        /// Lấy ra mã nguyên vật liệu mới
        /// </summary>
        /// <returns>Mã nguyên vật liệu mới</returns>
        /// CreatedBy: CTKimYen (13/1/2022)
        public string GetNewCode(string materialPreCode);

    }
}
