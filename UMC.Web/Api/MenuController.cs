using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UMC.Web.Infrastructure.Core;
using UMC.Web.Infrastructure.Extensions;
using UMC.Model.Models;
using UMC.Service;
using UMC.Web.Models;
using System;

namespace UMC.Web.Api
{
    [RoutePrefix("api/menu")]
    public class MenuController : ApiControllerBase
    {
        IMenuService _menuService;
        IMenuGroupService _menuGroupService;
        public MenuController(IErrorService errorService, IMenuService menuService, IMenuGroupService menuGroupService) :
            base(errorService)
        {
            this._menuService = menuService;
            this._menuGroupService = menuGroupService;
        }
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
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request, string keyword,int page, int pageSize = 20)
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

        public async Task<HttpResponseMessage> Put(HttpRequestMessage request, MenuViewModel menuVm)
        {
            //Khoi tao
            var menuDb = await _menuService.GetById(menuVm.ID);
            //AutoMapper
            menuDb.UpdateMenu(menuVm);
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    _menuService.Update(menuDb);
                    _menuService.SaveAsync();
                    response = request.CreateResponse(HttpStatusCode.OK);
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
