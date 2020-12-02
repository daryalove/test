using RopeDetection.CommonData.ViewModels.UserViewModel;
using RopeDetection.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        //Task<User> GetUserWithDailyRowsAsync(Guid userId);
        Task<User> GetOnlyUserByIdAsync(Guid userId);
        Task<User> RegisterUser(UserModel model, byte[] paswordHash, byte[] passwordSalt);
        Task<Guid> GetUserIdByUserNameAsync(string userName);
        //Task<IEnumerable<User>> GetUserByUserFIO(string userFIO);
        Task<User> GetOnlyUserByUsernameAsync(string userName);
    }
}
