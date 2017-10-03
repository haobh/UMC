using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMC.Common.Exceptions;
using UMC.Data.Infrastructure;
using UMC.Data.Repositories;
using UMC.Model.Models;

namespace UMC.Service
{
    public interface IApplicationGroupService
    {
        Task<ApplicationGroup> GetDetail(int id);
        IEnumerable<ApplicationGroup> GetAll(int page, int pageSize,out int totalRow, string filter);
        Task<IEnumerable<ApplicationGroup>> GetAll();
        Task<ApplicationGroup> Add(ApplicationGroup appGroup);
        Task Update(ApplicationGroup appGroup);
        Task<ApplicationGroup> Delete(int id);
        Task<bool> AddUserToGroups(IEnumerable<ApplicationUserGroup> groups, string userId);
        Task<IEnumerable<ApplicationGroup>> GetListGroupByUserId(string userId);

        //Task<IEnumerable<ApplicationUser>> GetListUserByGroupId(int groupId);

        Task Save();
    }
    public class ApplicationGroupService : IApplicationGroupService
    {
        private readonly IApplicationGroupRepository _appGroupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserGroupRepository _appUserGroupRepository;

        public ApplicationGroupService(IUnitOfWork unitOfWork,
            IApplicationUserGroupRepository appUserGroupRepository,
            IApplicationGroupRepository appGroupRepository)
        {
            this._appGroupRepository = appGroupRepository;
            this._appUserGroupRepository = appUserGroupRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<ApplicationGroup> Add(ApplicationGroup appGroup)
        {
            if(await _appGroupRepository.CheckContains(x => x.Name == appGroup.Name))
                throw new NameDuplicatedException("Tên không được trùng");
            return _appGroupRepository.Add(appGroup);
        }

        public async Task<ApplicationGroup> Delete(int id)
        {
            var appGroup = this._appGroupRepository.GetSingleById(id);
            return await Task.FromResult(_appGroupRepository.Delete(appGroup));
        }

        public async Task<IEnumerable<ApplicationGroup>> GetAll()
        {
            return await _appGroupRepository.GetAll();
        }

        public IEnumerable<ApplicationGroup> GetAll(int page, int pageSize, out int totalRow, string filter = null)
        {
            var query = Task.Run(async () => await _appGroupRepository.GetAll()).Result;
            if(!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));

            totalRow = query.Count();
            return query.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);
        }

        public async Task<ApplicationGroup> GetDetail(int id)
        {
            return await Task.FromResult(_appGroupRepository.GetSingleById(id));
        }

        public async Task Save()
        {
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(ApplicationGroup appGroup)
        {
            if(await _appGroupRepository.CheckContains(x => x.Name == appGroup.Name && x.ID != appGroup.ID))
                throw new NameDuplicatedException("Tên không được trùng");
            _appGroupRepository.Update(appGroup);
        }

        public async Task<bool> AddUserToGroups(IEnumerable<ApplicationUserGroup> userGroups, string userId)
        {
            _appUserGroupRepository.DeleteMulti(x => x.UserId == userId);
            foreach (var userGroup in userGroups)
            {
                await Task.FromResult(_appUserGroupRepository.Add(userGroup));
            }
            return true;
        }

        public async Task<IEnumerable<ApplicationGroup>> GetListGroupByUserId(string userId)
        {
            return await Task.FromResult(_appGroupRepository.GetListGroupByUserId(userId));
        }

        public IEnumerable<ApplicationUser> GetListUserByGroupId(int groupId)
        {
            return _appGroupRepository.GetListUserByGroupId(groupId);
        }
    }
}
