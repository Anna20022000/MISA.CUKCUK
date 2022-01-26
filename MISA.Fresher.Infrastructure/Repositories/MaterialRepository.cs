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
        /// <summary>
        ///  Thực hiện lấy ra một NVL theo khóa chính và danh sách ĐVCĐ của NVL đó (nếu có)
        /// </summary>
        /// <param name="entityId">khóa chính</param>
        /// <returns>NVL có khóa chính cần tìm</returns>
        public override Material GetById(Guid entityId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("m_MaterialId", entityId);
            var procName = string.Format(ProcGetSingle, "Material");
            using (var multi = _dbConnection.QueryMultiple(procName, parameters, commandType: CommandType.StoredProcedure))
            {
                var material = multi.ReadSingleOrDefault<Material>();
                if (material != null)
                {
                    material.Conversions = multi.Read<Conversion>().ToList();
                }
                return material;
            }
        }

        public object Filter(int pageIndex, int pageSize, List<ObjectFilter> objectFilters, ObjectSort objectSort)
        {
            string columns = string.Empty;
            string where = string.Empty;
            string sort = string.Empty;
            string table = _className.ToLower();
            DynamicParameters parameters = new DynamicParameters();

            if (objectFilters.Count > 0)
            {
                foreach (var item in objectFilters)
                {
                    // thêm điều kiện vào chuỗi where
                    // Kiểm tra toán tử
                    switch (item.Operator)
                    {
                        case Core.Enum.DbOperator.Contain:
                            where += $" {item.Column} LIKE '%{item.Value}%' {item.AdditionalOperator}";
                            break;
                        case Core.Enum.DbOperator.EqualTo:
                            where += $" {item.Column} = '{item.Value}' {item.AdditionalOperator}";
                            break;
                        case Core.Enum.DbOperator.BeginWith:
                            where += $" {item.Column} LIKE '{item.Value}%' {item.AdditionalOperator}";
                            break;
                        case Core.Enum.DbOperator.EndWith:
                            where += $" {item.Column} LIKE '%{item.Value}' {item.AdditionalOperator}";
                            break;
                        case Core.Enum.DbOperator.NotContain:
                            where += $" {item.Column} NOT LIKE '%{item.Value}%' {item.AdditionalOperator}";
                            break;
                        default:
                            break;
                    }
                }
                // Cắt bỏ điều kiện AND/OR thừa ở cuối chuỗi
                if (where.Length > 0)
                    where = where.Substring(0, where.LastIndexOf(" "));
            }
            // Thêm tên cột và điều kiện sắp xếp vào chuỗi sort
            if (!string.IsNullOrEmpty(objectSort.Column))
            {
                sort += $"{objectSort.Column} {objectSort.Sort}";
            }
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                var sql = "Proc_Filter";
                parameters.Add("m_Table", table);
                parameters.Add("m_PageSize", pageSize);
                parameters.Add("m_PageIndex", pageIndex);
                parameters.Add("m_Column", columns);
                parameters.Add("m_Where", where);
                parameters.Add("m_Sort", sort);
                parameters.Add("m_TotalRecord", direction: ParameterDirection.Output);
                parameters.Add("m_TotalPage", direction: ParameterDirection.Output);
                // Thực thi lấy dữ liệu trong db:
                var entities = sqlConnection.Query<Material>(sql, param: parameters, commandType: CommandType.StoredProcedure);
                var totalPage = parameters.Get<int>("m_TotalPage");
                var totalRecord = parameters.Get<int>("m_TotalRecord");

                return new
                {
                    TotalRecord = totalRecord,
                    TotalPage = totalPage,
                    Data = entities
                };
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
                    result += Insert(material, transaction);
                    if (result > 0 && material.Conversions.Count > 0)
                    {
                        // Thêm mới danh sách Đơn vị chuyển đổi
                        foreach (Conversion item in material.Conversions)
                        {
                            item.MaterialId = material.MaterialId;
                            result += Insert(item, transaction);
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
                finally
                {
                    _dbConnection.Close();
                    _dbConnection.Dispose();
                }
            }
            return result;
        }
        public override int Update(BaseEntity entity, Guid entityId)
        {
            Material material = entity as Material;

            int result = 0;
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    material.MaterialId = entityId;
                    // Sửa NVL
                    result += Update(material, transaction);
                    if (result > 0 && material.Conversions.Count > 0)
                    {
                        // Cập nhật danh sách Đơn vị chuyển đổi
                        foreach (Conversion item in material.Conversions)
                        {
                            item.MaterialId = material.MaterialId;

                            switch (item.State)
                            {
                                // Nếu conversion có trạng thái = insert -> thêm mới
                                case Core.Enum.State.Insert:
                                    result += Insert(item, transaction);
                                    break;
                                // Nếu conversion có trạng thái = update -> sửa
                                case Core.Enum.State.Update:
                                    result += Update(item, transaction);
                                    break;
                                // Nếu conversion có trạng thái = delete -> xóa
                                case Core.Enum.State.Delete:
                                    result += Delete(item, transaction);
                                    break;
                                default:
                                    break;
                            }
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
