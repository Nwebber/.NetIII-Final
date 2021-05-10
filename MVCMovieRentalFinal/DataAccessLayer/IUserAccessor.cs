using DataObjectsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IUserAccessor
    {
        List<UserViewModel> SelectUserByActive(bool active = true);
        List<string> SelectRolesByUserID(int userID);
        List<string> SelectAllRoles();
        int UpdateUserProfile(User oldUser, User newUser);
        int DeactivateUser(int userID);
        int ReactivateUser(int userID);
        int DeleteUserRole(int userID, string role);
        int InsertUserRole(int userID, string role);
        int InsertNewUser(User user);
        int InsertWebUser(User user, string password);
        bool DeleteUser(int id);
    }
}
