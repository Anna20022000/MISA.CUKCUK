using MISA.CukCuk.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Entities
{
    /// <summary>
    /// Thông tin dùng để lọc dữ liệu
    /// CreatedBy: CTKimYen (16/1/2022)
    /// </summary>
    public class ObjectFilter
    {
        /// <summary>
        /// Cột cần lọc
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// Toán tử
        /// </summary>
        public DbOperator Operator { get; set; }
        /// <summary>
        /// Giá trị lọc
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Kiểu dữ liệu lọc (Int, String, Datetime,...)
        /// </summary>
        public string ValueType { get; set; }
        /// <summary>
        /// Kiểu quan hệ điều kiện với các cột khác
        /// </summary>
        public string AdditionalOperator { get; set; } = "AND";
        
    }
}
