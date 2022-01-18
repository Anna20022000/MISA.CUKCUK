using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Interfaces.Repository;
using MISA.CukCuk.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        #region Declare and contructor
        IBaseRepository<T> _baseRepository;
        IBaseService<T> _baseService;

        public BaseController(IBaseRepository<T> baseRepository, IBaseService<T> baseService)
        {
            _baseRepository = baseRepository;
            _baseService = baseService;
        }

        #endregion

        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// CreatedBy: CTKimYen (13/1/2022)
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _baseRepository.GetAll();
            return Ok(entities);
        }

        /// <summary>
        /// Lấy đối tượng theo khóa chính
        /// </summary>
        /// <param name="Id">Khóa chính</param>
        /// <returns>Đối tượng có khóa chính cần lấy</returns>
        /// CreatedBy: CTKimYen (13/1/2022)
        [HttpGet("{Id}")]
        public IActionResult GetById(Guid Id)
        {
            var entity = _baseRepository.GetById(Id);
            if (entity != null)
            {
                return Ok(entity);
            }
            return StatusCode(204, null);
        }

        /// <summary>
        /// Thêm mới bản ghi vào trong cơ sở dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// <returns>Số bản ghi thêm mới thành công</returns>
        /// CreatedBy: CTKimYen (13/1/2022)
        [HttpPost]
        public virtual IActionResult Insert(T entity)
        {
            var res = _baseService.Insert(entity);
            return StatusCode(201, res);
        }

        /// <summary>
        /// Cập nhật bản ghi vào cơ sở dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần sửa</param>
        /// <param name="entityId">Khóa chính của đối tượng</param>
        /// <returns>Số bản ghi cập nhật thành công</returns>
        /// CreatedBy: CTKimYen (13/1/2022)
        [HttpPut("{Id}")]
        public IActionResult Update(T entity, Guid Id)
        {
            var res = _baseService.Update(entity, Id);
            if (res > 0)
            {
                return StatusCode(200, res);
            }
            return null;
        }

        /// <summary>
        /// Xóa bản ghi dựa vào khóa chính
        /// </summary>
        /// <param name="Id">Khóa chính</param>
        /// <returns>Số lượng bản ghi xóa thành công</returns>
        /// CreatedBy: CTKimYen (13/1/2022)
        [HttpDelete("{Id}")]
        public IActionResult Delete(Guid Id)
        {
            var res = _baseRepository.Delete(Id);
            if (res > 0)
            {
                return StatusCode(200, res);
            }
            return null;
        }
    }
}
