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
        /// <summary>
        /// Thực hiện lấy mã NVL mới trong db
        /// </summary>
        /// <param name="materialName">tên NVL</param>
        /// <returns>Mã NVL mới</returns>
        /// CreatedBy: CTKimYen (13/1/2022)
        string GetNewCode(string materialName);

        /// <summary>
        /// Thực hiện lọc dữ liệu theo điều kiện
        /// </summary>
        /// <param name="pageIndex">Trang cần lấy</param>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="objectFilterJson">Danh sách các đối tượng lọc kiểu Json</param>
        /// <returns>Danh sách bản ghi thỏa mãn</returns>
        /// CreatedDate: CTKimYen (16/1/2022)
        object Filter(int pageIndex, int pageSize, string objectFilterJson);
    }
}
