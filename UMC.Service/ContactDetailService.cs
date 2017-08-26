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
    public interface IContactDetailService
    {
        Task<ContactDetail> GetDefaultContact();
    }
    public class ContactDetailService : IContactDetailService
    {
        private readonly IContactDetailRepository _contactDetailRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ContactDetailService(IContactDetailRepository contactDetailRepository, IUnitOfWork unitOfWork)
        {
            this._contactDetailRepository = contactDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<ContactDetail> GetDefaultContact()
        {
            return await _contactDetailRepository.GetSingleByCondition(x => x.Status);
        }
    }
}
