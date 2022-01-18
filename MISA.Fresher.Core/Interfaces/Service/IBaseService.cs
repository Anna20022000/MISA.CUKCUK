using MISA.CukCuk.Core.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Interfaces.Service
{
    public interface IBaseService<T>
    {
        /// <summary>
        /// Thực hiện thêm mới đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <returns>Số lượng bản ghi thêm mới thành công</returns>
        /// createdBy: CTKimYen (13/1/2022)
        int? Insert(T entity);
        /// <summary>
        /// Thực hiện Sửa một đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <param name="entityId">Khóa chính</param>
        /// <returns>Số lượng bản ghi sửa thành công</returns>
        /// createdBy: CTKimYen (13/1/2022)
        int Update(T entity, Guid entityId); 
        /// <summary>
        /// Thực hiện xóa một hoặc nhiều đối tượng
        /// </summary>
        /// <param name="listEntityId">Danh sách khóa chính</param>
        /// <returns>Số lượng bản ghi xóa thành công</returns>
        /// createdBy: CTKimYen (15/1/2022)
        int? Delete(List<Guid> listEntityId);
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
