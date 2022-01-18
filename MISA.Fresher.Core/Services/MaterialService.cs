using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Exceptions;
using MISA.CukCuk.Core.Interfaces.Repository;
using MISA.CukCuk.Core.Interfaces.Service;
using MISA.CukCuk.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Services
{
    public class MaterialService : BaseService<Material>, IMaterialService
    {
        #region Declare
        private readonly IMaterialRepository _materialRepository;
        #endregion

        #region Contructor
        public MaterialService(IMaterialRepository materialRepository) : base(materialRepository)
        {
            _materialRepository = materialRepository;
        }
        #endregion

        #region Functions
        public string GetNewCode(string materialName)
        {
            string materialPreCode = "";
            if (!String.IsNullOrEmpty(materialName))
            {
                // Thay thế nhieeuf ký tự space = 1 ký tự space (nếu có) và viết hoa
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);
                materialName = regex.Replace(materialName, " ").ToUpper().Trim();
                // Lấy ra chữ cái đầu của các từ đứng sau dấu cách
                foreach (var c in materialName.Split(' '))
                {
                    materialPreCode += c.Substring(0, 1);
                }
            }
            return _materialRepository.GetNewCode(materialPreCode);
        }

        /// <summary>
        /// Thực hiện kiểm tra ĐV chuyển đổi có trùng với ĐVT chính hoặc trùng nhau hay không
        /// </summary>
        /// <param name="entity">Nguyên vật liệu cần validate</param>
        /// <returns>true - dữ liệu hợp lệ; false - dữ liệu không hợp lệ</returns>
        /// createdBy: CTKimYen (18/1/2022)
        protected override bool ValidateObjCustom(Material entity)
        {
            List<Guid> dvts = new List<Guid>();
            var dvtc = entity.UnitId;
            // nếu list ĐVCĐ có chứa đối tượng
            if (entity.Conversions.Count > 0)
            {
                foreach (Conversion item in entity.Conversions)
                {
                    // kiểm tra id của ĐVCĐ
                    // Nếu trùng với id của ĐVT chính -> false
                    if (dvtc.CompareTo(item.UnitId) == 0)
                    {
                        throw new ResponseNotValidException(Resources.Error_Msg_Dupplicate_Unit);
                    }
                    // Nếu các id của các ĐTCĐ trùng với nhau -> false
                    if (dvts.Contains(item.UnitId))
                        throw new ResponseNotValidException(Resources.Error_Msg_Dupplicate_Conversion);
                    // Nếu tm thì add vào list ĐVT
                    dvts.Add(item.UnitId);
                }
            }
            return true;
        }
        #endregion
    }
}
