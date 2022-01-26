using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Entities
{
    /// <summary>
    /// Thông tin đối tunowgj sắp xếp
    /// CreatedBy: CTKimYen
    /// </summary>
    public class ObjectSort
    {
        /// <summary>
        /// Cột cần lọc
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// Điều kiện sắp xếp
        /// true-Tăng dần; false-Giảm dần
        /// </summary>
        public string Sort { get; set; }
    }
}
