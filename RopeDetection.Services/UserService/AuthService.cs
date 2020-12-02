using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RopeDetection.CommonData.ViewModels.Base;
using RopeDetection.CommonData.ViewModels.UserViewModel;
using RopeDetection.Entities.Configuration;
using RopeDetection.Entities.Models;
using RopeDetection.Entities.Repository.Interfaces;
using RopeDetection.Services.Interfaces;
using RopeDetection.Services.Mapper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Services.UserService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAppLogger<AuthService> _logger;
        private readonly AppSettings _appSettings;

        public AuthService(IUserRepository userRepository,
            IAppLogger<AuthService> logger, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings.Value;
        }
       
        public async Task<IEnumerable<UserModel>> GetUserList()
        {
            try
            {
                var category = await _userRepository.GetAllAsync();
                var mapped = ObjectMapper.Mapper.Map<IEnumerable<UserModel>>(category);
                return mapped;
            }
            catch (Exception exp)
            {
                _logger.LogWarning("При поиске пользователей возникла ошибка", exp);
                return new List<UserModel>();
            }
        }

        public async Task<UserShortModel> Register(UserModel model)
        {
            try
            {
                // validation
                if (string.IsNullOrWhiteSpace(model.Password))
                    throw new Exception("Password is required");

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

                var mappedEntity = await _userRepository.RegisterUser(model, passwordHash, passwordSalt);
                _logger.LogInformation($"Entity successfully added - BaseProject");
                var newMappedEntity = ObjectMapper.Mapper.Map<UserShortModel>(mappedEntity);
                newMappedEntity.SuccessInfo = "Пользователь успешно зарегестрирован. Просьба выполнить вход.";
                return newMappedEntity;
            }
            catch (Exception exp)
            {
                _logger.LogWarning("При создании пользователя возникла ошибка", exp);
                return BaseModelUtilities<UserShortModel>.ErrorFormat(exp);
            }
        }

        //public async Task<IEnumerable<UserShortModel>> GetUserByUserFIO(string userFIO)
        //{
        //    try
        //    {
        //        var mappedEntity = await _userRepository.GetUserByUserFIO(userFIO);

        //        var newMappedEntity = ObjectMapper.Mapper.Map<IEnumerable<UserShortModel>>(mappedEntity);
        //        return newMappedEntity;
        //    }
        //    catch (Exception exp)
        //    {
        //        _logger.LogWarning("При поиске пользователей возникла ошибка", exp);
        //        return new List<UserShortModel>();
        //    }

        //}

       
        public async Task<UserShortModel> LoginUser(string userName, string userPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPassword))
                    return null;

                var user = await _userRepository.GetOnlyUserByUsernameAsync(userName);
                if (user != null)
                {
                    // check if password is correct
                    if (!VerifyPasswordHash(userPassword, user.PasswordHash, user.PasswordSalt))
                    {
                        return BaseModelUtilities<UserShortModel>.ErrorFormat(new Exception("Не удалось выполнить вход, пароль указан не верно!"));
                    }
                    else
                    {
                        var token = generateJwtToken(user);
                        var newMappedEntity = ObjectMapper.Mapper.Map<UserShortModel>(user);
                        newMappedEntity.Token = token;
                        newMappedEntity.SuccessInfo = "Успешно выполнен вход.";
                        //if (!newMappedEntity.IsAllowed)
                        //{
                        //    return BaseModelUtilities<UserShortModel>.ErrorFormat(new Exception("Не удалось выполнить вход, Вашей учетной записи еще не открыт доступ к Системе"));
                        //}
                        return newMappedEntity;
                    }
                }
                else
                {
                    return BaseModelUtilities<UserShortModel>.ErrorFormat(new Exception("Не удалось выполнить вход, пользователь не найден!"));
                }

            }
            catch (Exception exp)
            {
                return BaseModelUtilities<UserShortModel>.ErrorFormat(exp);
            }
        }

        private async Task ValidateIfExist(UserModel userModel)
        {
            var existingEntity = await _userRepository.GetByIdAsync(userModel.Id);
            if (existingEntity != null)
                throw new ApplicationException($"{userModel.ToString()} with this id already exists");
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                 {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                 }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        //public async Task<DataTableModel.DtResponse<UserModel>> GetUsers(string search, string start, string length, DataTableModel.DtOrder[] order, DataTableModel.DtColumn[] columns)
        //{
        //    try
        //    {
        //        Expression<Func<User, bool>> predicate = null;
        //        Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null;
        //        int? skip = null, take = null;

        //        if (!string.IsNullOrEmpty(search))
        //        {
        //            search = search.ToLower().Trim();
        //            predicate = x => !string.IsNullOrEmpty(x.UserFIO) && x.UserFIO.ToLower().Contains(search);
        //        }
        //        foreach (var o in order)
        //        {
        //            var sidx = columns[o.column].data;
        //            if (o.dir == "asc")
        //            {
        //                switch (sidx)
        //                {
        //                    case "UserFIO":
        //                        orderBy = x => x.OrderBy(m => m.UserFIO);
        //                        break;
        //                    case "UserName":
        //                        orderBy = x => x.OrderBy(m => m.UserName);
        //                        break;
        //                    case "Email":
        //                        orderBy = x => x.OrderBy(m => m.Email);
        //                        break;
        //                    case "Password":
        //                        orderBy = x => x.OrderBy(m => m.Password);
        //                        break;
        //                    case "IsAllowed":
        //                        orderBy = x => x.OrderBy(m => m.IsAllowed);
        //                        break;
        //                    case "Description":
        //                        orderBy = x => x.OrderBy(m => m.Description);
        //                        break;
        //                }
        //            }
        //            else
        //            {
        //                switch (sidx)
        //                {
        //                    case "UserFIO":
        //                        orderBy = x => x.OrderByDescending(m => m.UserFIO);
        //                        break;
        //                    case "UserName":
        //                        orderBy = x => x.OrderByDescending(m => m.UserName);
        //                        break;
        //                    case "Email":
        //                        orderBy = x => x.OrderByDescending(m => m.Email);
        //                        break;
        //                    case "Password":
        //                        orderBy = x => x.OrderByDescending(m => m.Password);
        //                        break;
        //                    case "IsAllowed":
        //                        orderBy = x => x.OrderByDescending(m => m.IsAllowed);
        //                        break;
        //                    case "Description":
        //                        orderBy = x => x.OrderByDescending(m => m.Description);
        //                        break;
        //                }
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(length))
        //        {
        //            if (int.TryParse(start, out int tmp))
        //            {
        //                skip = tmp;
        //            }
        //            if (int.TryParse(length, out tmp))
        //            {
        //                take = tmp;
        //            }
        //        }

        //        var users = await _userRepository.GetAsync(predicate, orderBy, skip, take);
        //        var total = await _userRepository.CountAsync(predicate, orderBy);

        //        return new DataTableModel.DtResponse<UserModel>()
        //        {
        //            data = ObjectMapper.Mapper.Map<IReadOnlyList<UserModel>>(users),
        //            recordsFiltered = total,
        //            recordsTotal = total
        //        };
        //    }
        //    catch (Exception exp)
        //    {
        //        return DataTableModel.DtResponse<UserModel>.Error(exp);
        //    }
        //}

        public async Task<UserModel> GetUser(Guid Id)
        {
            try
            {
                var data = await _userRepository.GetByIdAsync(Id);
                var newMappedEntity = ObjectMapper.Mapper.Map<UserModel>(data);
                return newMappedEntity;
            }
            catch (Exception exp)
            {
                return BaseModelUtilities<UserModel>.ErrorFormat(exp);
            }
        }

        public async Task<UserModel> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userRepository.GetOnlyUserByUsernameAsync(userName);
                var newMappedEntity = ObjectMapper.Mapper.Map<UserModel>(user);
                return newMappedEntity;
            }
            catch (Exception exp)
            {
                return BaseModelUtilities<UserModel>.ErrorFormat(exp);
            }
        }

        public async Task<Guid> GetUserIdByUserName(string userName)
        {
            try
            {
                var data = await _userRepository.GetUserIdByUserNameAsync(userName);
                return data;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public async Task UpdateUser(UserModel userModel)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userModel.Id);
                if (userModel.Id == Guid.Empty || user == null)
                {
                    await _userRepository.AddAsync(
                        ObjectMapper.Mapper.Map<User>(userModel));
                }
                else
                {
                    user.UserFIO = userModel.UserFIO;
                    user.UserName = userModel.UserName;
                    //user.Password = userModel.Password;
                    user.Email = userModel.Email;
                    user.Description = userModel.Description;
                    user.IsAllowed = userModel.IsAllowed;
                    await _userRepository.UpdateAsync(user);
                }
            }
            catch { throw; }
        }

        public async Task DeleteUser(Guid Id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(Id);
                if (user != null)
                {
                    await _userRepository.DeleteAsync(user);
                }
            }
            catch { throw; }
        }
    }
}
