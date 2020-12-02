using RopeDetection.CommonData.ViewModels.UserViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IEnumerable<UserModel>> GetUserList();
        Task<UserShortModel> Register(UserModel model);
        Task<Guid> GetUserIdByUserName(string userName);
        Task<UserModel> GetUserByUserNameAsync(string userName);
        //Task<IEnumerable<UserShortModel>> GetUserByUserName(string userName);
        //Task<ContentItemModel> CreateContentItem(Guid? modelId, Guid? userId, Guid? sectioCode, string sectionName, string title, string content, string url);
        //Task<IEnumerable<SectionModel>> GetSections(string sectionName);
        //Task<IEnumerable<SectionRowModel>> GetSectionsAndContent();
        //Task<ContentItemModel> GetContentItem(Guid id);
        Task<UserShortModel> LoginUser(string userName, string userPassword);

        //Task<DataTableModel.DtResponse<UserModel>> GetUsers(string search, string start, string length, DataTableModel.DtOrder[] order, DataTableModel.DtColumn[] columns);

        Task<UserModel> GetUser(Guid Id);
        Task UpdateUser(UserModel userModel);
        Task DeleteUser(Guid Id);
    }
}
