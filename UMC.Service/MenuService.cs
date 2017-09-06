﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMC.Data.Infrastructure;
using UMC.Data.Repositories;
using UMC.Model.Models;

namespace UMC.Service
{
    public interface IMenuService
    {
        Task<IEnumerable<Menu>> GetAll();
        Task<IEnumerable<Menu>> GetAll(string keyword);
        Task<Menu> Add(Menu menu);
        Task Update(Menu menu);
        Task<Menu> GetById(int id);
        Task<Menu> Delete(int id);
        Task SaveAsync();
    }
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MenuService(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
        {
            this._menuRepository = menuRepository;
            this._unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Menu>> GetAll()
        {
            return await _menuRepository.GetAll();
        }
        //Nếu có keyword thì vào hàm tìm kiếm, không thì nó vào hàm trên
        public async Task<IEnumerable<Menu>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _menuRepository.GetMulti(x => x.Name.Contains(keyword) || x.Target.Contains(keyword));
            else
                return await _menuRepository.GetAll();
        }
        public async Task<Menu> Add(Menu menu)
        {
            return await Task.FromResult(_menuRepository.Add(menu));
        }
        public async Task Update(Menu menu)
        {
            await Task.FromResult<object>(null); 
            _menuRepository.Update(menu);
        }
        public async Task<Menu> GetById(int id)
        {
            return await Task.FromResult(_menuRepository.GetSingleById(id));
        }
        public async Task<Menu> Delete(int id)
        {
            return await Task.FromResult(_menuRepository.Delete(id));
        }
        public async Task SaveAsync()
        {
            await _unitOfWork.CommitAsync();
        }      
    }
}
