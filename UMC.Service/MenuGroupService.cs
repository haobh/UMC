using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMC.Data.Infrastructure;
using UMC.Data.Repositories;
using UMC.Model.Models;

namespace UMC.Service
{
    public interface IMenuGroupService
    {
        Task<IEnumerable<MenuGroup>> GetAll();
        Task<MenuGroup> Add(MenuGroup menuGroup);
        Task Update(MenuGroup menuGroup);
        Task<MenuGroup> GetById(int id);
        Task<MenuGroup> Delete(int id);
        Task SaveAsync();
    }
    public class MenuGroupService : IMenuGroupService
    {
        private readonly IMenuGroupRepository _menuGroupRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MenuGroupService(IMenuGroupRepository menuGroupRepository, IUnitOfWork unitOfWork)
        {
            this._menuGroupRepository = menuGroupRepository;
            this._unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<MenuGroup>> GetAll()
        {
            return await _menuGroupRepository.GetAll();
        }
        public async Task<MenuGroup> Add(MenuGroup menuGroup)
        {
            return await Task.FromResult(_menuGroupRepository.Add(menuGroup));
        }
        public async Task Update(MenuGroup menuGroup)
        {
            _menuGroupRepository.Update(menuGroup);
            await Task.FromResult<object>(null);
        }
        public async Task<MenuGroup> GetById(int id)
        {
            return await Task.FromResult(_menuGroupRepository.GetSingleById(id));
        }
        public async Task<MenuGroup> Delete(int id)
        {
            return await Task.FromResult(_menuGroupRepository.Delete(id));
        }
        public async Task SaveAsync()
        {
            await _unitOfWork.CommitAsync();
        }
    }
}
