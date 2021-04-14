using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace yuiime.Repo
{
    public interface IUserRepo<T>
    {
        Task<bool> AddUserAsync(T user);
        Task<bool> UpdateUserAsync(T user);
        Task<bool> DeleteUserAsync(string username);

        Task<T> GetUserAsync(string username);

        Task<IEnumerable<T>> GetUsersAsync(bool forceRefresh = false);
    }
}
