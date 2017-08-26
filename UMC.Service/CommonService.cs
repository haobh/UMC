using System.Collections.Generic;
using System.Threading.Tasks;
using UMC.Common;
using UMC.Data.Infrastructure;
using UMC.Data.Repositories;
using UMC.Model.Models;

namespace UMC.Service
{
    public interface ICommonService
    {
        Task<Footer> GetFooter();
        Task<IEnumerable<Slide>> GetSlides();
        Task<SystemConfig> GetSystemConfig(string code);
    }
    public class CommonService : ICommonService
    {
        private readonly IFooterRepository _footerRepository;
        private readonly ISystemConfigRepository _systemConfigRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISlideRepository _slideRepository;
        public CommonService(IFooterRepository footerRepository,ISystemConfigRepository systemConfigRepository,IUnitOfWork unitOfWork,ISlideRepository slideRepository)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _systemConfigRepository = systemConfigRepository;
            _slideRepository = slideRepository;
        }

        public async Task<Footer> GetFooter()
        {
            return await _footerRepository.GetSingleByCondition(x => x.ID == CommonConstants.DefaultFooterId);
        }

        public async Task<IEnumerable<Slide>> GetSlides()
        {
            return await _slideRepository.GetMulti(x=>x.Status);
        }

        public async Task<SystemConfig> GetSystemConfig(string code)
        {
            return await _systemConfigRepository.GetSingleByCondition(x => x.Code == code);
        }
    }
}
