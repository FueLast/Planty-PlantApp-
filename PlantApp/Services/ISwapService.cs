using PlantApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Services
{
    public interface ISwapService
    {
        Task CreateOfferAsync(int ownerId, int plantId, string? desired);

        Task<List<SwapOffer>> GetAllOffersAsync(bool onlyPreview = false, int skip = 0, int take = 20);

        Task SendRequestAsync(int offerId, int fromUserId, int plantId);

        Task UpdateRequestStatusAsync(int requestId, SwapStatus status);

        Task<List<SwapRequest>> GetIncomingRequestsAsync(int ownerId);
    }
}