using DataObjectsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IUserManager
    {
        List<UserViewModel> RetrieveUsersByActive(bool active = true);

        List<string> RetrieveRolesByUserID(int userID);

        List<string> RetrieveAllRoles();

        bool EditUserProfile(UserViewModel olduser, UserViewModel newuser, List<string> oldUnassignedRoles, List<string> newUnassignedRoles);

        bool AddNewUser(UserViewModel newUser);
        bool AddWebUser(UserViewModel newUser, string password);
        bool DeleteUser(int id);
    }
}
