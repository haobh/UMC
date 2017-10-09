using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using UMC.Common.Exceptions;
using UMC.Data.Infrastructure;
using UMC.Model.Models;
using UMC.Service;
using UMC.Web.App_Start;
using UMC.Web.Infrastructure.Core;
using UMC.Web.Infrastructure.Extensions;
using UMC.Web.Models;

namespace UMC.Web.Api
{
    [RoutePrefix("api/applicationGroup")]
    [Authorize]
    public class ApplicationGroupController : ApiControllerBase
    {
        private readonly IApplicationGroupService _appGroupService;
        private readonly IApplicationRoleService _appRoleService;
        private readonly ApplicationUserManager _userManager;

        public ApplicationGroupController(IErrorService errorService,
            IApplicationRoleService appRoleService,
            ApplicationUserManager userManager,
            IApplicationGroupService appGroupService) : base(errorService)
        {
            _appGroupService = appGroupService;
            _appRoleService = appRoleService;
            _userManager = userManager;
        }
        /// <summary>
        /// Hiển thị ra danh sách Group có phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route("getlistpaging")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            int totalRow = 0;
            var model = _appGroupService.GetAll(page, pageSize, out totalRow, filter);
            IEnumerable<ApplicationGroupViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(model);
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;           
                PaginationSet<ApplicationGroupViewModel> pagedSet = new PaginationSet<ApplicationGroupViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = modelVm
                };
                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }

        /// <summary>
        /// Khi thêm mới User cho phép check vào Group nào
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("getlistall")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAll(HttpRequestMessage request)
        {
            var model = await _appGroupService.GetAll();
            IEnumerable<ApplicationGroupViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(model);
            return await CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                response = request.CreateResponse(HttpStatusCode.OK, modelVm);
                return response;
            });
        }
       
        [HttpPost]
        [Route("add")]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, ApplicationGroupViewModel appGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppGroup = new ApplicationGroup();
                newAppGroup.Name = appGroupViewModel.Name;
                newAppGroup.Description = appGroupViewModel.Description;
                try
                {
                    var appGroup = await _appGroupService.Add(newAppGroup);
                    await _appGroupService.Save();

                    //save group
                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    foreach (var role in appGroupViewModel.Roles)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup()
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.Id
                        });
                    }
                    _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    await _appRoleService.Save();

                    return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("detail/{id:int}")]
        [HttpGet]
        public async Task<HttpResponseMessage> Details(HttpRequestMessage request, int id)
        {
            if (id == 0)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " is required.");
            }
            ApplicationGroup appGroup = await _appGroupService.GetDetail(id);
            var appGroupViewModel = Mapper.Map<ApplicationGroup, ApplicationGroupViewModel>(appGroup);
            if (appGroup == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "No group");
            }
            //var listRole = _appRoleService.GetListRoleByGroupId(appGroupViewModel.ID);
            //appGroupViewModel.Roles = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(listRole);
            return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
        }
     
        [HttpPut]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, ApplicationGroupViewModel appGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var appGroup = await _appGroupService.GetDetail(appGroupViewModel.ID);
                try
                {
                    appGroup.UpdateApplicationGroup(appGroupViewModel);
                    await _appGroupService.Update(appGroup);
                    await _appGroupService.Save();

                    //save group
                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    foreach (var role in appGroupViewModel.Roles)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup()
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.Id
                        });
                    }
                    _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    await _appRoleService.Save();

                    //add role to user
                    var listRole = _appRoleService.GetListRoleByGroupId(appGroup.ID);
                    var listUserInGroup = await _appGroupService.GetListUserByGroupId(appGroup.ID);
                    foreach (var user in listUserInGroup)
                    {
                        var listRoleName = listRole.Select(x => x.Name).ToArray();
                        foreach (var roleName in listRoleName)
                        {
                            await _userManager.RemoveFromRoleAsync(user.Id, roleName);
                            await _userManager.AddToRoleAsync(user.Id, roleName);
                        }
                    }
                    return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }

            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, int id)
        {
            var appGroup = await _appGroupService.Delete(id);
            await _appGroupService.Save();
            return request.CreateResponse(HttpStatusCode.OK, appGroup);
        }

        [Route("deletemulti")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteMulti(HttpRequestMessage request, string checkedList)
        {
            HttpResponseMessage response = null;
            if (!ModelState.IsValid)
            {
                response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
            else
            {
                var listItem = new JavaScriptSerializer().Deserialize<List<int>>(checkedList);
                foreach (var item in listItem)
                {
                    await _appGroupService.Delete(item);
                }
                await _appGroupService.Save();
                response = request.CreateResponse(HttpStatusCode.OK, listItem.Count);
            }
            return response;
        }
    }
}
