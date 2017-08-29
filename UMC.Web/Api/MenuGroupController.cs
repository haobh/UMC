using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using UMC.Web.Infrastructure.Core;
using UMC.Web.Infrastructure.Extensions;
using UMC.Model.Models;
using UMC.Service;
using UMC.Web.Models;

namespace UMC.Web.Api
{
    [RoutePrefix("api/menugroup")]
    public class MenuGroupController : ApiControllerBase
    {
        IMenuGroupService _menuGroupService;
        //Ke thua tu lop ApiControllerBase, truyen Error vao
        public MenuGroupController(IErrorService errorService, IMenuGroupService menuGroupService) :
            base(errorService)
        {
            this._menuGroupService = menuGroupService;
        }

        //No se lay tien to Route [RoutePrefix("api/postcategory")] +  [Route("getall")] ra URL
        //No khong quan tam den ten phuong thuc
        [Route("getall")]
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request)
        {
            //return Ok(await _menuGroupService.GetAll()); //cach 2 dung IActionResult
            var listMenuGroup = await _menuGroupService.GetAll();
            var listMenuGroupVm = Mapper.Map<List<MenuGroupViewModel>>(listMenuGroup);
            return await CreateHttpResponse(request, () =>
            {                
               HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listMenuGroupVm); //Map
               return response;
             });
        }

        [Route("add")]
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request, MenuGroupViewModel menuGroupVm)
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
                    MenuGroup newMenuGroup = new MenuGroup();
                    newMenuGroup.UpdateMenuGroup(menuGroupVm); //Gan ViewModel sang Model de Insert DB, this
                    //Goi Insert
                    var menuGroup = _menuGroupService.Add(newMenuGroup);
                    _menuGroupService.SaveAsync();
                    response = request.CreateResponse(HttpStatusCode.Created, menuGroup);
                }
                return response;
            });
        }

        public async Task<HttpResponseMessage> Put(HttpRequestMessage request, MenuGroupViewModel menuGroupVm)
        {
            //Khoi tao
            var menuGroupDb = await _menuGroupService.GetById(menuGroupVm.ID);
            //AutoMapper
            menuGroupDb.UpdateMenuGroup(menuGroupVm);
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {                   
                    _menuGroupService.Update(menuGroupDb);
                    _menuGroupService.SaveAsync();
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
                    _menuGroupService.Delete(id);
                    _menuGroupService.SaveAsync();
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                return response;
            });
        }
    }
}
