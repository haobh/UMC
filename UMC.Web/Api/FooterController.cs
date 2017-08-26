using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [RoutePrefix("api/footer")]
    public class FooterController : ApiControllerBase
    {
        IFooterService _footerService;
        public FooterController(IErrorService errorService, IFooterService footerService) :
            base(errorService)
        {
            this._footerService = footerService;
        }
        [Route("getall")]
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request)
        {
            var listFooter = await _footerService.GetAll();
            var listFooterVm = Mapper.Map<List<FooterViewModel>>(listFooter);
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listFooterVm); //Map
                return response;
            });
        }

        [Route("add")]
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request, FooterViewModel footerVm)
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
                    Footer newFooter = new Footer();
                    newFooter.UpdateFooter(footerVm); //Gan ViewModel sang Model de Insert DB, this
                    //Goi Insert
                    var footer = _footerService.Add(newFooter);
                    _footerService.SaveAsync();
                    response = request.CreateResponse(HttpStatusCode.Created, footer);
                }
                return response;
            });
        }

        public async Task<HttpResponseMessage> Put(HttpRequestMessage request, FooterViewModel footerVm)
        {
            //Khoi tao
            var footerDb = await _footerService.GetById(footerVm.ID);
            //AutoMapper
            footerDb.UpdateFooter(footerVm);
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    _footerService.Update(footerDb);
                    _footerService.SaveAsync();
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
                    _footerService.Delete(id);
                    _footerService.SaveAsync();
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                return response;
            });
        }
}
