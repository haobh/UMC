using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UMC.Model.Models;
using UMC.Service;
using UMC.Web.Infrastructure.Core;
using UMC.Web.Infrastructure.Extensions;
using UMC.Web.Models;

namespace UMC.Web.Api
{
    [RoutePrefix("api/menu")]
    public class MenuController : ApiControllerBase
    {
        #region Initialize

        private IMenuService _menuService;
        private IMenuGroupService _menuGroupService;

        public MenuController(IErrorService errorService, IMenuService menuService, IMenuGroupService menuGroupService) :
            base(errorService)
        {
            this._menuService = menuService;
            this._menuGroupService = menuGroupService;
        }

        #endregion Initialize

        #region GetList ang Search

        /// <summary>
        /// Hàm này dùng để Get tất cả tên MenuId, bên bảng MenuGroup, API chỉ quan tâm đến Route, nó ko quan tâm đến tên hàm
        /// </summary>
        /// <param name="request"> nhận request từ Client</param>
        /// <returns>Trả ra Response về cho Client</returns>
        [Route("getmenugroupid")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            var model = await _menuGroupService.GetAll();
            var responseData = Mapper.Map<IEnumerable<MenuGroup>, IEnumerable<MenuGroupViewModel>>(model);
            return await CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        /// <summary>
        /// Hàm này dùng để GetList toàn bộ, có phân trang
        /// http://localhost:59445/api/menu/getall?keyword=a&page=1&pagesize=1
        /// </summary>
        /// <param name="request"></param>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route("getall")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            int totalRow = 0;
            //Đay chinh la ham tai boi(OverLoading), cung ten nhung khac tham so truyen vao
            var model = await _menuService.GetAll(keyword);
            totalRow = model.Count();
            var query = model.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);
            var responseData = Mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(query);
            var paginationSet = new PaginationSet<MenuViewModel>()
            {
                Items = responseData,
                Page = page,
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
            };
            return await CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        #endregion GetList ang Search

        /// <summary>
        /// Nhận parameter gán vào ViewModel thông qua scope
        /// </summary>
        /// <param name="request">Chứa phương thức Post</param>
        /// <param name="menuVm">Các Parameter dưới View gửi lên sẽ gán vào đây</param>
        /// Sau đó nó sẽ được Mapping vào newMenu thông qua AutoMapper hàm UpdateMenu
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request, MenuViewModel menuVm)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    //Khoi tao
                    Menu newMenu = new Menu();
                    newMenu.UpdateMenu(menuVm); //Gan ViewModel sang Model de Insert DB, this
                    //Goi Insert
                    var menu = _menuService.Add(newMenu);
                    _menuService.SaveAsync();
                    response = request.CreateResponse(HttpStatusCode.Created, menu);
                }
                return response;
            });
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetById(HttpRequestMessage request, int id)
        {
            var model = await _menuService.GetById(id);
            var responseData = Mapper.Map<Menu, MenuViewModel>(model);
            return await CreateHttpResponse(request, () =>
            {
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        /// <summary>
        /// Hàm Update dữ liệu
        /// </summary>
        /// <param name="request">Chứa phương thức client cần gọi</param>
        /// <param name="menuVm">Các Parameter dưới View gửi lên sẽ gán vào đây</param>
        /// dbMenu: sẽ tìm bản ghi dựa vào Id Client gửi lên
        /// Sau đó dbMenu sẽ được gán dữ liệu từ Vm vào và Update DataBase
        /// <returns></returns>
        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Put(HttpRequestMessage request, MenuViewModel menuVm)
        {
            //Tìm ID của bản ghi
            var dbMenu = await _menuService.GetById(menuVm.ID);
            //Mapping vào Menu
            dbMenu.UpdateMenu(menuVm);
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    _menuService.Update(dbMenu);
                    _menuService.SaveAsync();

                    var responseData = Mapper.Map<Menu, MenuViewModel>(dbMenu);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    _menuService.Delete(id);
                    _menuService.SaveAsync();
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                return response;
            });
        }
    }
}