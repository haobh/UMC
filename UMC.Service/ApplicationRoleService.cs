using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMC.Common.Exceptions;
using UMC.Data.Infrastructure;
using UMC.Data.Repositories;
using UMC.Model.Models;

namespace UMC.Service
{
    public interface IApplicationRoleService
    {
        IEnumerable<ApplicationRole> GetAll(int page, int pageSize, out int totalRow, string filter);

        Task<IEnumerable<ApplicationRole>> GetAll();

        Task<ApplicationRole> Add(ApplicationRole appRole);

        Task<ApplicationRole> GetDetail(string id);

        Task Update(ApplicationRole AppRole);

        Task Delete(string id);

        //Add roles to a sepcify group
        bool AddRolesToGroup(IEnumerable<ApplicationRoleGroup> roleGroups, int groupId);

        //Get list role by group id
        IEnumerable<ApplicationRole> GetListRoleByGroupId(int groupId);

        Task Save();
    }

    public class ApplicationRoleService : IApplicationRoleService
    {
        private readonly IApplicationRoleRepository _appRoleRepository;
        private readonly IApplicationRoleGroupRepository _appRoleGroupRepository;
        private IUnitOfWork _unitOfWork;

        public ApplicationRoleService(IUnitOfWork unitOfWork,
            IApplicationRoleRepository appRoleRepository,
            IApplicationRoleGroupRepository appRoleGroupRepository
        )
        {
            this._appRoleRepository = appRoleRepository;
            this._appRoleGroupRepository = appRoleGroupRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<ApplicationRole> Add(ApplicationRole appRole)
        {
            if (await _appRoleRepository.CheckContains(x => x.Description == appRole.Description))
                throw new NameDuplicatedException("Tên không được trùng");
            return _appRoleRepository.Add(appRole);
        }

        public async Task<IEnumerable<ApplicationRole>> GetAll()
        {
            return await _appRoleRepository.GetAll();
        }

        public IEnumerable<ApplicationRole> GetAll(int page, int pageSize, out int totalRow, string filter = null)
        {
            var query = Task.Run(async () => await _appRoleRepository.GetAll()).Result;
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Description.Contains(filter));
            totalRow = query.Count();
            return query.OrderBy(x => x.Description).Skip(page * pageSize).Take(pageSize);
        }

        public async Task<ApplicationRole> GetDetail(string id)
        {
            return await _appRoleRepository.GetSingleByCondition(x => x.Id == id);
        }

        public async Task Update(ApplicationRole AppRole)
        {
            if (await _appRoleRepository.CheckContains(x => x.Description == AppRole.Description && x.Id != AppRole.Id))
                throw new NameDuplicatedException("Tên không được trùng");
            _appRoleRepository.Update(AppRole);
        }

        public async Task Save()
        {
            await _unitOfWork.CommitAsync();
        }
        public async Task Delete(string id)
        {
            await Task.FromResult<object>(null);
            _appRoleRepository.DeleteMulti(x => x.Id == id);
        }

        public IEnumerable<ApplicationRole> GetListRoleByGroupId(int groupId)
        {
            return _appRoleRepository.GetListRoleByGroupId(groupId);
        }

        public bool AddRolesToGroup(IEnumerable<ApplicationRoleGroup> roleGroups, int groupId)
        {
            _appRoleGroupRepository.DeleteMulti(x => x.GroupId == groupId);
            foreach (var roleGroup in roleGroups)
            {
                _appRoleGroupRepository.Add(roleGroup);
            }
            return true;
        }
    }
}