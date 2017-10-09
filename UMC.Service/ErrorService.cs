using System.Threading.Tasks;
using UMC.Data.Infrastructure;
using UMC.Data.Repositories;
using UMC.Model.Models;

namespace UMC.Service
{
    public interface IErrorService
    {
        Error Create(Error error);
        Task SaveAsync();
    }
    public class ErrorService : IErrorService
    {
        private readonly IErrorRepository _errorRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ErrorService(IErrorRepository errorRepository,IUnitOfWork unitOfWork)
        {
            this._errorRepository = errorRepository;
            this._unitOfWork = unitOfWork;
        }
        public Error Create(Error error)
        {
            return _errorRepository.Add(error);
        }

        public async Task SaveAsync()
        {
            await _unitOfWork.CommitAsync();
        }
    }
}
