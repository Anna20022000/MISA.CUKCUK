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

        /// <summary>
        /// Thực hiện lọc dữ liệu theo điều kiện
        /// </summary>
        /// <param name="pageIndex">Trang cần lấy</param>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="objectFilters">Danh sách các đối tượng lọc</param>
        /// <returns>Danh sách bản ghi thỏa mãn</returns>
        /// CreatedDate: CTKimYen (16/1/2022)
        object Filter(int pageIndex, int pageSize, List<ObjectFilter> objectFilters);
    }
}
