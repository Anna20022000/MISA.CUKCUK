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
    /// Thông tin Nguyên liệu nhập
    /// CreatedBy: CTKimYen (12/1/2022)
    /// </summary>
    [MasterEntity]
    public class Material : BaseEntity
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid MaterialId { get; set; }
        /// <summary>
        /// Mã NVL
        /// </summary>
        [NotEmpty]
        [Unique]
        [PropertyName("Mã nguyên vật liệu")]
        public string MaterialCode { get; set; }
        /// <summary>
        /// Tên NVL
        /// </summary>
        [NotEmpty]
        [PropertyName("Tên nguyên vật liệu")]
        public string MaterialName { get; set; }
        /// <summary>
        /// Số thời gian đến hạn
        /// </summary>
        public int? Expiry { get; set; }
        /// <summary>
        /// Đơn vị thời gian của HSD (Ngày/ Tháng/ Năm)
        /// </summary>
        public string TimeUnit { get; set; }
        /// <summary>
        /// Số lượng tồn kho tối thiểu
        /// </summary>
        public int? Quantity { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Đang theo dõi
        /// </summary>
        public int? IsFollow { get; set; }
        /// <summary>
        /// Mã Đơn vị tính - Khóa ngoại
        /// </summary>
        [NotEmpty]
        [PropertyName("Mã đơn vị tính")]
        public Guid UnitId { get; set; }
        /// <summary>
        /// Tên Đơn vị tính
        /// </summary>
        [ReadOnly]
        public string UnitName { get; set; }
        /// <summary>
        /// Mã Kho - Khóa ngoại
        /// </summary>
        public Guid? WarehouseId { get; set; }
        /// <summary>
        /// Mã nhóm nguyên vật liệu - Khóa ngoại
        /// </summary>
        public Guid? MaterialCategoryId { get; set; }
        /// <summary>
        /// Tên nhóm NVL
        /// </summary>
        [ReadOnly]
        public string MaterialCategoryName { get; set; }
        /// <summary>
        /// Danh sách đơn vị chuyển đổi
        /// </summary>
        [ReadOnly]
        [Details]
        public List<Conversion> Conversions { get; set; }
    }
}
