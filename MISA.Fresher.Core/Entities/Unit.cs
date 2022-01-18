using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Entities
{
    /// <summary>
    /// Bảng Đơn vị tính
    /// CreateBy: CTKimYen (12/1/2022)
    /// </summary>
    public class Unit : BaseEntity
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid UnitId { get; set; }
        /// <summary>
        /// Tên đơn vị tính
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// Diễn giải
        /// </summary>
        public string Description { get; set; }
    }
}