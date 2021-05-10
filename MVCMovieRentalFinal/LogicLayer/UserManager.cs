using DataAccessLayer;
using DataObjectsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class UserManager : IUserManager
    {
        private IUserAccessor _userAccessor;

        public UserManager()
        {
            _userAccessor = new UserAccessor();
        }

        public UserManager(IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        public bool AddNewUser(UserViewModel newUser)
        {
            bool result = false;
            int newEmployeeID = 0; // Invalid UserID

            try
            {
                newEmployeeID = _userAccessor.InsertNewUser(newUser);

                if (newEmployeeID == 0)
                {
                    throw new ApplicationException("New user was not added.");
                }

                // Add newly assigned roles
                foreach (var role in newUser.Roles)
                {
                    _userAccessor.InsertUserRole(newEmployeeID, role);
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Add User Failed", ex);
            }

            return result;
        }
        public bool AddWebUser(UserViewModel newUser, string password)
        {
            password = password.SHA256Value();
            bool result = false;
            int newUserID = 0;

            try
            {
                newUserID = _userAccessor.InsertWebUser(newUser, password);

                if (newUserID == 0)
                {
                    throw new ApplicationException("New user was not added.");
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Add User Failed", ex);
            }

            return result;
        }

        public bool EditUserProfile(UserViewModel olduser, UserViewModel newuser, List<string> oldUnassignedRoles, List<string> newUnassignedRoles)
        {
            bool result = false;

            try
            {
                result = (1 == _userAccessor.UpdateUserProfile(olduser, newuser));

                if (result == false)
                {
                    throw new ApplicationException("Profile Data not Changed");
                }

                // Remove new removed roles
                foreach (var role in newUnassignedRoles)
                {
                    if (!oldUnassignedRoles.Contains(role))
                    {
                        _userAccessor.DeleteUserRole(olduser.UserID, role);
                    }
                }

                // Add newly assigned roles
                foreach (var role in newuser.Roles)
                {
                    if (!olduser.Roles.Contains(role))
                    {
                        _userAccessor.InsertUserRole(olduser.UserID, role);
                    }
                }

                if (olduser.Active != newuser.Active)
                {
                    if (newuser.Active == true)
                    {
                        _userAccessor.ReactivateUser(olduser.UserID);
                    }
                    else
                    {
                        _userAccessor.DeactivateUser(olduser.UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update Failed", ex);
            }

            return result;
        }

        public bool UpdateUserProfile(UserViewModel oldUser, UserViewModel newUser)
        {
            bool result = false;

            try
            {
                result = (1 == _userAccessor.UpdateUserProfile(oldUser, newUser));

                if (result == false)
                {
                    throw new ApplicationException("Profile Data not Changed");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Update Failed", ex);
            }

            return result;
        }

        public List<string> RetrieveRolesByUserID(int userID)
        {
            List<string> roles = null;

            try
            {
                roles = _userAccessor.SelectRolesByUserID(userID);

                if (roles == null)
                {
                    roles = new List<string>();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data Unavailable", ex);
            }

            return roles;
        }

        public List<UserViewModel> RetrieveUsersByActive(bool active = true)
        {
            List<UserViewModel> users = null;

            try
            {
                users = _userAccessor.SelectUserByActive(active);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("User list not available.", ex);
            }
            finally
            {

            }

            return users;
        }

        public List<string> RetrieveAllRoles()
        {
            List<string> roles = null;

            try
            {
                roles = _userAccessor.SelectAllRoles();

                if (roles == null)
                {
                    roles = new List<string>();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data Unavailable", ex);
            }

            return roles;
        }
        public bool DeleteUser(int id)
        {
            bool result = false;

            try
            {
                result = _userAccessor.DeleteUser(id);
                if (result == false)
                {
                    throw new ApplicationException("User could not be deleted");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("User could not be deleted", ex);
            }

            return result;
        }

        public void SynchronizeRoles(int userId, List<string> roles)
        {
            var existingRoles = _userAccessor.SelectRolesByUserID(userId);

            var rolesToRemove = existingRoles.Except(roles);

            // remove any unassigned roles
            foreach (var role in rolesToRemove)
            {
                _userAccessor.DeleteUserRole(userId, role);
            }

            var rolesToAdd = roles.Except(existingRoles);

            // add any assigned roles
            foreach (var role in rolesToAdd)
            {
                _userAccessor.InsertUserRole(userId, role);
            }
        }
    }
}
