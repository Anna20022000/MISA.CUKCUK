using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Interfaces.Repository
{
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// Thực hiện lấy tất cả bản ghi của bảng T
        /// </summary>
        /// <returns>Tất cả bản ghi</returns>
        public IEnumerable<T> GetAll();
        /// <summary>
        /// Thực hiện lấy một đối tượng theo khóa chính
        /// </summary>
        /// <param name="entityId">Khóa chính</param>
        /// <returns>Đối tượng có khóa chính cần tìm</returns>
        /// CreatedBy: CTKimYen (13/1/2022)
        public T GetById(Guid entityId);
        /// <summary>
        /// Thực hiện Thêm mới một đối tượng vào cơ sở dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số lượng bản ghi thêm mới thành công</returns>
        /// CreatedBY: CTKimYen (13/1/2022)
        public int Insert(BaseEntity entity);
        /// <summary>
        /// Thực hiện Sửa một đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <param name="entityId">Khóa chính</param>
        /// <returns>Số lượng bản ghi sửa thành công</returns>
        /// createdBy: CTKimYen (13/1/2022)
        public int Update(BaseEntity entity, Guid entityId);

        /// <summary>
        /// Thực hiện xóa một đối tượng
        /// </summary>
        /// <param name="entityId">Khóa chính</param>
        /// <returns>Số lượng bản ghi xóa thành công</returns>
        /// createdBy: CTKimYen (13/1/2022)
        public int Delete(Guid entityId);

        /// <summary>
        /// Thực hiện kiểm tra giá trị thuộc tính của 1 đối tượng có tồn tại trong csdl
        /// </summary>
        /// <param name="propName">Tên thuộc tính</param>
        /// <param name="propValue">Giá trị của thuộc tính</param>
        /// <param name="entityId">Giá trị Khóa chính của đối tượng (nếu có)</param>
        /// <returns>true - đã tồn tại; false - chưa tồn tại</returns>
        /// CreatedBy: CTKimYen (13/1/2022)
        public bool CheckExist(string propName, string propValue, string entityId);

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
