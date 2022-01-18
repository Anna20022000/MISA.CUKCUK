using MISA.CukCuk.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.CukCuk.Core.CukcukAttribute.ValidationAttribute;

namespace MISA.CukCuk.Core.Entities
{
    /// <summary>
    /// Thông tin Đơn vị chuyển đổi
    /// CreatedBy: CTKimYen (13/1/2022)
    /// </summary>
    public class Conversion : BaseEntity
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid ConversionId { get; set; }
        /// <summary>
        /// Tỷ lệ chuyển đổi
        /// </summary>
        public int ConversionRate { get; set; }
        /// <summary>
        /// Phép tính
        /// </summary>
        public string Calculation { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Mã đơn vị tính - Khóa ngoại
        /// </summary>
        public Guid UnitId { get; set; }
        /// <summary>
        /// Mã NVL - Khóa ngoại
        /// </summary>
        public Guid MaterialId { get; set; }
        /// <summary>
        /// Trạng thái của đối tượng
        /// </summary>
        [ReadOnly]
        public State State { get; set; }
    }
}
