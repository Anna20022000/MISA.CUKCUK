using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.CukCuk.Core.CukcukAttribute.ValidationAttribute;

namespace MISA.CukCuk.Core.Entities
{
    /// <summary>
    /// Thông tin Nhóm nguyên vật liệu
    /// CreatedBy: CTKimYen (14/1/2022)
    /// </summary>
    public class MaterialCategory : BaseEntity
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid MaterialCategoryId { get; set; }
        /// <summary>
        /// Mã Nhóm NVL
        /// </summary>
        [NotEmpty]
        [Unique]
        public string MaterialCategoryCode { get; set; }
        /// <summary>
        /// Tên nhóm NVL
        /// </summary>
        [NotEmpty]
        public string MaterialCategoryName { get; set; }
        /// <summary>
        /// Diễn giải
        /// </summary>
        public string MaterialCategoryNote { get; set; }
    }
}
