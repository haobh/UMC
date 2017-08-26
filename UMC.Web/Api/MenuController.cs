using AutoMapper;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Infrastructure.Extensions;
using UMC.Model.Models;
using UMC.Service;
using UMC.Web.Models;

namespace UMC.Web.Api
{
    [RoutePrefix("api/menu")]
    public class MenuController : ApiControllerBase
    {
        IMenuService _menuService;
        public MenuController(IErrorService errorService, IMenuService menuService) :
            base(errorService)
        {
            this._menuService = menuService;
        }
        [Route("getall")]
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request)
        {
            var listMenu = await _menuService.GetAll();
            var listMenuVm = Mapper.Map<List<MenuViewModel>>(listMenu);
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listMenuVm); //Map
                return response;
            });
        }

        [Route("add")]
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request, MenuViewModel menuVm)
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
