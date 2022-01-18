using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Entities
{
    /// <summary>
    /// Thông tin Kho
    /// CreatedBy: CTKimYen (12/1/2022)
    /// </summary>
    public class Warehouse : BaseEntity
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid WarehouseId { get; set; }
        /// <summary>
        /// Mã Kho
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// Tên kho
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }
    }
}
