using System.Threading.Tasks;
using UMC.Data.Infrastructure;
using UMC.Data.Repositories;
using UMC.Model.Models;

namespace UMC.Service
{
    public interface IFeedbackService
    {
        Feedback Create(Feedback feedback);
        Task SaveAsync();
    }

    public class FeedbackService : IFeedbackService
    {
        private IFeedbackRepository _feedbackRepository;
        private IUnitOfWork _unitOfWork;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork)
        {
            _feedbackRepository = feedbackRepository;
            _unitOfWork = unitOfWork;
        }

        public Feedback Create(Feedback feedback)
        {
            return _feedbackRepository.Add(feedback);
        }

        public async Task SaveAsync()
        {
            await _unitOfWork.CommitAsync();
        }
    }
}