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
    }
}
