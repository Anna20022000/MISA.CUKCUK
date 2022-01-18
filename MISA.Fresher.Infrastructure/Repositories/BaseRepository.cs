using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Entities.Dtos;
using MISA.CukCuk.Core.Interfaces.Repository;
using MISA.CukCuk.Core.Properties;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.CukCuk.Core.CukcukAttribute.ValidationAttribute;

namespace MISA.CukCuk.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        #region Declare
        /// <summary>
        /// Chuỗi kết nối đến csdl
        /// </summary>
        protected string _connectionString = string.Empty;
        protected IDbConnection _dbConnection = null;
        /// <summary>
        /// Tên của đối tượng
        /// </summary>
        protected string _className;
        /// <summary>
        /// Tên các store proceduce
        /// </summary>
        protected string ProcInsert = Core.Properties.Resources.Proc_Insert;
        protected string ProcUpdate = Core.Properties.Resources.Proc_Update;

        #endregion

        #region Contructor
        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CukCuk");
            _dbConnection = new MySqlConnection(_connectionString);
            _className = typeof(T).Name;
        }
        #endregion

        #region Functions
        public bool CheckExist(string propName, string propValue, string entityId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{propName}", propValue);
            parameters.Add($"@{_className}Id", entityId);

            string sql = $"SELECT * FROM {_className} WHERE {propName} = @{propName} AND {_className}Id != @{_className}Id ";

            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                // Thực thi lấy dữ liệu trong db:
                var rowAffected = sqlConnection.Query(sql, param: parameters);
                if (rowAffected.Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public int Delete(Guid entityId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{_className}ID", entityId);
            string sql = $"DELETE FROM {_className} WHERE {_className}ID = @{_className}ID";
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {

                    var rowAffected = sqlConnection.Execute(sql, param: parameters, transaction: transaction);
                    transaction.Commit();
                    return rowAffected;
                }
            }
        }

        public object Filter(int pageIndex, int pageSize, List<ObjectFilter> objectFilters)
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
                    // thêm tên cột vào chuỗi columns
                    //columns += $"{ item.Column},";
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
                    // Thêm tên cột và điều kiện sắp xếp vào chuỗi sort
                    if (item.Sort != string.Empty)
                    {
                        sort += $"{item.Column} {item.Sort},";
                    }

                }
                // Cắt bỏ dấu phẩy thừa ở cuối chuỗi
                //columns = columns.Substring(0, columns.Length - 1);
                if (sort.Length > 0)
                    sort = sort.Substring(0, sort.Length - 1);
                // Cắt bỏ điều kiện AND/OR thừa ở cuối chuỗi
                if (where.Length > 0)
                    where = where.Substring(0, where.LastIndexOf(" "));
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
                var entities = sqlConnection.Query<T>(sql, param: parameters, commandType: CommandType.StoredProcedure);
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

        public virtual IEnumerable<T> GetAll()
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                // Thực thi lấy dữ liệu trong db:
                var entities = sqlConnection.Query<T>(sql: $"SELECT * FROM {_className}");
                return entities;
            }
        }

        public virtual T GetById(Guid entityId)
        {
            // khởi tạo kết nối với db:
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                // Thực thi lấy dữ liệu trong db:
                var entitiy = sqlConnection.QueryFirstOrDefault<T>(sql: $"SELECT * FROM {_className} WHERE {_className}Id = '{entityId}'");

                return entitiy;
            }
        }

        /// <summary>
        /// Tạo paramesters cho đối tượng
        /// </summary>
        /// <param name="entity">đối tượng</param>
        /// <returns>Paramesters của đối tượng đó</returns>
        public DynamicParameters SetParam(BaseEntity entity)
        {
            DynamicParameters parameters = new DynamicParameters();
            // Lấy ra các property của đối tượng:
            var props = entity.GetType().GetProperties().Where(p => !p.IsDefined(typeof(ReadOnly), true) && p.Name != nameof(BaseEntity.CreatedDate) && p.Name != nameof(BaseEntity.ModifiedDate)).ToList();
            // Duyet tung property
            var entityTypeName = entity.GetType().Name;
            foreach (var prop in props)
            {
                // Lay ra ten cua prop
                var propName = prop.Name;
                // Lay ra gia tri cua prop tuong ung voi doi tuong:
                var propValue = prop.GetValue(entity);
                // Nếu property là Khóa chính và có giá trị mặc định thì set = mã Guid mới
                if (propName == $"{entityTypeName}Id" && prop.PropertyType == typeof(Guid) &&
                    propValue.ToString().CompareTo(Resources.Defauld_Guid) == 0)
                {
                    propValue = Guid.NewGuid();
                }
                parameters.Add($"m_{propName}", propValue);
            }
            return parameters;
        }

        public virtual int Insert(BaseEntity entity)
        {
            DynamicParameters parameters = SetParam(entity);

            var sql = string.Format(ProcInsert, _className);

            // khởi tạo kết nối
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    var rowAffected = sqlConnection.Execute(sql, param: parameters, transaction: transaction, commandType: CommandType.StoredProcedure);
                    transaction.Commit();
                    return rowAffected;
                }
            }
        }
        /// <summary>
        /// Thêm một enity với transaction cho trước
        /// </summary>
        /// <param name="entity">Đối tượng thêm mới</param>
        /// <param name="transaction">transaction</param>
        /// <returns>Số lượng bản ghi thêm mới thành công</returns>
        /// CreatedBy: CTKimYen (18/1/2022)
        public int Add(BaseEntity entity, IDbTransaction transaction)
        {
            var parameters = SetParam(entity);
            var connection = transaction.Connection;
            var rowAffects = 0;
            if (connection?.State == ConnectionState.Open)
            {
                var entityTypeName = entity.GetType().Name;
                string sql = string.Format(ProcInsert, entityTypeName);
                // thực thi commandtext 
                rowAffects = connection.Execute(sql, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            }
            //số bản ghi thêm mới thành công
            return rowAffects;
        }
        public virtual int Update(BaseEntity entity, Guid entityId)
        {
            DynamicParameters parameters = SetParam(entity);
            // Tên store proceduce
            var sql = string.Format(ProcUpdate, _className);

            // khởi tạo kết nối với db:
            using (MySqlConnection mySqlConnection = new MySqlConnection(_connectionString))
            {
                mySqlConnection.Open();
                MySqlTransaction transaction = mySqlConnection.BeginTransaction();

                var rowAffected = mySqlConnection.Execute(sql, param: parameters, transaction: transaction);

                transaction.Commit();

                return rowAffected;
            }
        }
        #endregion

    }
}
