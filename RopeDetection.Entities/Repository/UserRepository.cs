using RopeDetection.CommonData.ViewModels.UserViewModel;
using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Models;
using RopeDetection.Entities.Repository.Base;
using RopeDetection.Entities.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Entities.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ModelContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetOnlyUserByIdAsync(Guid userId)
        {
            var category = (await GetAsync(m => m.Id == userId)).FirstOrDefault();
            return category;
        }

        //Реализация входа в приложение
        public async Task<User> GetOnlyUserByUsernameAsync(string userName)
        {
            var category = (await GetAsync(m => m.UserName.ToLower() == userName.ToLower())).FirstOrDefault();
            return category;
        }

        public async Task<Guid> GetUserIdByUserNameAsync(string userName)
        {
            var user = (await GetAsync(m => m.UserName.ToLower() == userName.ToLower())).FirstOrDefault();
            if (user != null)
                return user.Id;
            else return Guid.Empty;
        }

        //public async Task<IEnumerable<User>> GetUserByUserFIO(string userFIO)
        //{
        //    return await _dbContext.Users
        //         .Where(x => x.UserFIO.ToLower().StartsWith(userFIO.ToLower())).Take(15)
        //         .ToListAsync();
        //}

        //Регистрация пользователя
        public async Task<User> RegisterUser(UserModel model, byte[] passwordHash, byte[] passwordSalt)
        {
            var GetData = GetAsyncIQueryable(m => m.UserName.ToLower() == model.UserName.ToLower());
            if (GetData.Count() != 0)
            {
                throw new Exception("Пользователь с данным Email уже существует.");
            }
            var mappedEntity = User.Create(model.UserFIO, model.Email, passwordHash, passwordSalt);

            var newEntity = await AddAsync(mappedEntity);
            return newEntity;
        }
    }
}
