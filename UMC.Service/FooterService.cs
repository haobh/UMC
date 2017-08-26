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
    public interface IFooterService
    {
        Task<IEnumerable<Footer>> GetAll();
        Task<Footer> Add(Footer footer);
        Task Update(Footer footer);
        Task<Footer> GetById(string id);
        Task<Footer> Delete(int id);
        Task SaveAsync();
    }
    public class FooterService : IFooterService
    {
        private readonly IFooterRepository _footerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public FooterService(IFooterRepository footerRepository, IUnitOfWork unitOfWork)
        {
            this._footerRepository = footerRepository;
            this._unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Footer>> GetAll()
        {
            return await _footerRepository.GetAll();
        }
        public async Task<Footer> Add(Footer footer)
        {
            return await Task.FromResult(_footerRepository.Add(footer));
        }
        public async Task Update(Footer footer)
        {
            _footerRepository.Update(footer);
            await Task.FromResult<object>(null);
        }
        public async Task<Footer> GetById(string id)
        {
            return await Task.FromResult(_footerRepository.GetSingleById(id));
        }
        public async Task<Footer> Delete(int id)
        {
            return await Task.FromResult(_footerRepository.Delete(id));
        }
        public async Task SaveAsync()
        {
            await _unitOfWork.CommitAsync();
        }
    }
}
