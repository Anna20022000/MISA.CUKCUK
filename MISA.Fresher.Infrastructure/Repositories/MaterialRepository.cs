using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Infrastructure.Repositories
{
    public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
    {
        public MaterialRepository(IConfiguration configuration) : base(configuration)
        {

        }
        #region Functions
        /// <summary>
        /// Thực hiện lấy tất cả bản ghi của bảng Nguyên vật liệu (bao gồm cả thông tin: tên ĐVT chính, tên Nhóm NVL) 
        /// </summary>
        /// <returns>Tất cả bản ghi của bảng NVL</returns>
        /// CreatedBy: CTKimYen (15/1/2022)
        public override IEnumerable<Material> GetAll()
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                // Thực thi lấy dữ liệu trong db:
                var entities = sqlConnection.Query<Material>(sql: $"SELECT * FROM view_{_className}");
                return entities;
            }
        }

        public string GetNewCode(string materialPreCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("m_String", materialPreCode);
            parameters.Add("m_NewCode", direction: ParameterDirection.Output);
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                var sql = "Proc_GetNewMaterialCode";
                // Thực thi lấy dữ liệu trong db:
                sqlConnection.Query<string>(sql, param: parameters, commandType: CommandType.StoredProcedure);
                string newCode = parameters.Get<string>("m_NewCode");
                return newCode;
            }
        }

        /// <summary>
        /// Thêm mới một Nguyên vật liệu và nhiều Đơn vị chuyển đổi (nếu có)
        /// </summary>
        /// <param name="entity">Đối tượng Nguyên vật liệu</param>
        /// <returns>Số lượng bản ghi thêm mới thành công</returns>
        /// CreatedBy: CTKimYen (18/1/2022)
        public override int Insert(BaseEntity entity)
        {
            Material material = entity as Material;
            material.MaterialId = Guid.NewGuid();

            int result = 0;
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    // Thêm mới NVL
                    result += Add(material, transaction);
                    if (result > 0 && material.Conversions.Count > 0)
                    {
                        // Thêm mới danh sách Đơn vị chuyển đổi
                        foreach (Conversion item in material.Conversions)
                        {
                            item.MaterialId = material.MaterialId;
                            result += Add(item, transaction);
                        }
                    }
                    if (result < material.Conversions.Count + 1) transaction.Rollback();
                    else transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        #endregion
    }
}
