using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Enum
{
    public static class Enumeration
    {
        /// <summary>
        /// Thực thi lấy text trong resource tương ứng với Enum
        /// </summary>
        /// <typeparam name="T">Kiểu Enum</typeparam>
        /// <param name="cukcukEnum">Đối tượng Enum</param>
        /// <returns>Text Enum tương ứng</returns>
        /// createdBy: CTKimYen (13/1/2022)
        public static string GetEnumTextByEnumName<T>(T cukcukEnum)
        {
            var enumPropName = cukcukEnum.ToString();
            var enumName = cukcukEnum.GetType();
            var resourceText = Properties.Resources.ResourceManager.GetString($"Enum_{enumName}_{enumPropName}");
            return resourceText;
        }
    }

    /// <summary>
    /// mã trạng thái HTTP res
    /// </summary>
    /// createdBy: CTKimYen (13/1/2022)
    public enum HttpStatusCode
    {
        /// <summary>
        /// Thành công
        /// </summary>
        Ok = 200,
        /// <summary>
        /// Thêm mới thành công
        /// </summary>
        Created = 201,
        /// <summary>
        /// Yêu cầu được chấp nhận nhưng việc xử lý chưa hoàn thành
        /// </summary>
        Accepted = 202,
        /// <summary>
        /// Yêu cầu đã được xử lý thành công nhưng phản hổi trống
        /// </summary>
        NoContent = 204,
        /// <summary>
        /// Máy chủ không hiểu được yêu cầu - lỗi dữ liệu gửi về
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// Request resource không tồn tại trên máy chủ
        /// </summary>
        NotFound = 404,
        /// <summary>
        /// Lỗi xử lý phía máy chủ
        /// </summary>
        InternalServerError = 500
    }
    /// <summary>
    /// Toán tử để so sánh điều kiện
    /// </summary>
    /// CreatedBy: CTKimYen (16/1/2022)
    public enum DbOperator
    {
        /// <summary>
        /// Chứa
        /// </summary>
        Contain = 0,
        /// <summary>
        /// Bằng
        /// </summary>
        EqualTo = 1,
        /// <summary>
        /// Bắt đầu bằng
        /// </summary>
        BeginWith = 2,
        /// <summary>
        /// Kết thúc bằng
        /// </summary>
        EndWith = 3,
        /// <summary>
        /// Không chứa
        /// </summary>
        NotContain = 4
    }
    /// <summary>
    /// Trạng thái của đối tượng - phục vụ cho việc chỉnh sửa
    /// </summary>
    /// CreatedBy: CTKimYen (16/1/2022)
    public enum State
    {
        /// <summary>
        /// Thêm
        /// </summary>
        Insert = 0,
        /// <summary>
        /// Sửa
        /// </summary>
        Update = 1,
        /// <summary>
        /// Xóa
        /// </summary>
        Delete = 2
    }
}
