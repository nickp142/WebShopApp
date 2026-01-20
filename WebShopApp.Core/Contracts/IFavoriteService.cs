using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopApp.Infrastructure.Data.Entities;

namespace WebShopApp.Core.Contracts
{
    public interface IFavoriteService
    {
        Task<bool> AddFavoriteAsync(string userId, int productId);
        Task<bool> RemoveFavoriteAsync(string userId, int productId);
        Task<List<Product>> GetFavoritesAsync(string userId);
        Task<bool> IsFavoriteAsync(string userId, int productId);
    }
}