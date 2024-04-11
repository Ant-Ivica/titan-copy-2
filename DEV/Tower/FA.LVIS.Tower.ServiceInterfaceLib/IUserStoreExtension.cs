using FA.LVIS.Tower.DataContracts;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.Services
{
    public interface IUserStoreExtension<TUser> where TUser : IdentityUser
    {
        /// <summary>
        /// Find a user by name and password
        /// </summary>
        /// <param name="user">The user details to retrieve based on AD un/pwd</param>
        /// <returns>The user</returns>
        Task<TUser> FindUserAsync(TUser user);

    }
}