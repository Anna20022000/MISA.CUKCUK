using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.CukCuk.Core.Entities;
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
        /// <summary>
        /// connection to db type IDbconnection
        /// </summary>
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
        protected string ProcDelete = Core.Properties.Resources.Proc_Delete;
        protected string ProcGetSingle = Core.Properties.Resources.Proc_GetSingle;

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
            parameters.Add($"@{_className}Id", entityId);
            string sql = $"DELETE FROM {_className} WHERE {_className}Id = @{_className}Id";
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
        /// <summary>
        /// Thực hiện xóa một bản ghi với transaction
        /// </summary>
        /// <param name="entity">đối tượng cần xóa</param>
        /// <param name="transaction">transaction</param>
        /// <returns>Số lượng bản ghi xóa thành công</returns>
        /// CreatedBy: CTKimYen (18/1/2022)
        protected int Delete(BaseEntity entity, IDbTransaction transaction)
        {
            var parameters = SetParam(entity);
            var connection = transaction.Connection;
            var rowAffects = 0;
            string sql = string.Format(ProcDelete, entity.GetType().Name);

            if (connection?.State == ConnectionState.Open)
            {
                // thực thi commandtext
                rowAffects = connection.Execute(sql, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            }
            //số bản ghi delete thành công
            return rowAffects;
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
        protected DynamicParameters SetParam(BaseEntity entity)
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
        protected int Insert(BaseEntity entity, IDbTransaction transaction)
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
        /// <summary>
        /// Thực hiện cập nhật một bản ghi với transaction cho trước
        /// </summary>
        /// <param name="entity">Đối tượng cần sửa</param>
        /// <param name="transaction">transaction</param>
        /// <returns>Số lượng bản ghi cập nhật thành công</returns>
        /// CreatedBy: CTKimYen (18/1/2022)
        protected int Edit(BaseEntity entity, IDbTransaction transaction)
        {
            var parameters = SetParam(entity);
            var connection = transaction.Connection;
            var rowAffects = 0;
            string sql = string.Format(ProcUpdate, entity.GetType().Name);

            if (connection?.State == ConnectionState.Open)
            {
                // thực thi commandtext
                rowAffects = connection.Execute(sql, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
            }
            //số bản ghi update thành công
            return rowAffects;
        }

        #endregion

    }
}
