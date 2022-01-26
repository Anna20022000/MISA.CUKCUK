using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Exceptions;
using MISA.CukCuk.Core.Interfaces.Repository;
using MISA.CukCuk.Core.Interfaces.Service;
using MISA.CukCuk.Core.Properties;
using Newtonsoft.Json;
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

        public object Filter(int pageIndex, int pageSize, string objectFilterJson, string objectSortJson)
        {
            // Convert string json to list object
            var objectFilters = JsonConvert.DeserializeObject<List<ObjectFilter>>(objectFilterJson);
            var objectSort = JsonConvert.DeserializeObject<ObjectSort>(objectSortJson);
            return _materialRepository.Filter(pageIndex, pageSize, objectFilters, objectSort);
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
                materialPreCode = ConvertToUnSign(materialPreCode);
            }
            return _materialRepository.GetNewCode(materialPreCode);
        }
        /// <summary>
        /// Thực hiện convert chuỗi có dấu sang chuỗi ko dấu
        /// </summary>
        /// <param name="s">Chuỗi cần convert</param>
        /// <returns>Chuỗi không dấu</returns>
        private string ConvertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
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
            // Id đơn vị tính chính
            var dvtc = entity.UnitId;
            // danh sách đvcđ của entity
            var conversions = entity.Conversions;
            // nếu list ĐVCĐ có chứa đối tượng
            if (conversions.Count > 0)
            {
                // Nếu danh sách đvcđ có đvcđ trùng Id đơn vị chính -> false
                if(conversions.Exists(c=> c.UnitId == dvtc))
                {
                    throw new ResponseNotValidException(Resources.Error_Msg_Dupplicate_Unit);
                }
                // kiểm tra trùng trong ds đvcđ
                else
                {
                    foreach (Conversion item in conversions)
                    {
                        //Nếu trong danh sách đvcđ có 2 đvcđ cùng id và trạng thái -> false
                        var condition = conversions.FindAll(c => c.UnitId == item.UnitId && c.State == item.State);
                        if ( condition.Count > 1)
                        {
                            throw new ResponseNotValidException(Resources.Error_Msg_Dupplicate_Conversion);
                        }
                    }
                }
            }
            return true;
        }

        #endregion
    }
}
