using DataObjectsLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserAccessor : IUserAccessor
    {
        public int DeactivateUser(int userID)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_deactivate_user", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new ApplicationException("User could not be deactivated.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public int DeleteUserRole(int userID, string role)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_safely_remove_Accountrole", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userID;
            cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 25);
            cmd.Parameters["@RoleID"].Value = role;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new ApplicationException(role + " role could not be removed");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public int InsertNewUser(User user)
        {
            int userID = 0;

            var conn = DBConnection.GetDBConnection();

            var cmd = new SqlCommand("sp_insert_new_user", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150);
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 100);

            cmd.Parameters["@Email"].Value = user.Email;
            cmd.Parameters["@FirstName"].Value = user.FirstName;
            cmd.Parameters["@LastName"].Value = user.LastName;
            cmd.Parameters["@Password"].Value = user.Password;

            try
            {
                conn.Open();
                userID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return userID;
        }

        public int InsertUserRole(int userID, string role)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_add_accountrole", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 25);
            cmd.Parameters["@RoleID"].Value = role;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new ApplicationException(role + " role could not be added");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public int ReactivateUser(int userID)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_reactivate_user", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public List<string> SelectAllRoles()
        {
            List<string> roles = new List<string>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_all_roles", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return roles;
        }

        public List<string> SelectRolesByUserID(int userID)
        {
            List<string> roles = new List<string>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_roles_by_accountID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return roles;
        }

        public List<UserViewModel> SelectUserByActive(bool active = true)
        {
            List<UserViewModel> users = new List<UserViewModel>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_users_by_active", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Active", active);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var user = new UserViewModel()
                        {
                            UserID = reader.GetInt32(0),
                            Email = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            Active = reader.GetBoolean(4),
                            Roles = null // Lazy instantiatiion - wait until needed
                        };
                        users.Add(user);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return users;
        }

        public int UpdateUserProfile(User oldUser, User newUser)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();

            var cmd = new SqlCommand("sp_update_account_profile_data", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.Int);

            cmd.Parameters.Add("@NewEmail", SqlDbType.NVarChar, 150);
            cmd.Parameters.Add("@NewFirstName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@NewLastName", SqlDbType.NVarChar, 100);

            cmd.Parameters.Add("@OldEmail", SqlDbType.NVarChar, 150);
            cmd.Parameters.Add("@OldFirstName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@OldLastName", SqlDbType.NVarChar, 100);

            cmd.Parameters["@UserID"].Value = oldUser.UserID;

            cmd.Parameters["@NewEmail"].Value = newUser.Email;
            cmd.Parameters["@NewFirstName"].Value = newUser.FirstName;
            cmd.Parameters["@NewLastName"].Value = newUser.LastName;

            cmd.Parameters["@OldEmail"].Value = oldUser.Email;
            cmd.Parameters["@OldFirstName"].Value = oldUser.FirstName;
            cmd.Parameters["@OldLastName"].Value = oldUser.LastName;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public bool DeleteUser(int id)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_delete_user", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = id;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new ApplicationException("User could not be deleted");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result == 1;
        }

        public int InsertWebUser(User user, string password)
        {
            int userID = 0;

            var conn = DBConnection.GetDBConnection();

            var cmd = new SqlCommand("sp_insert_new_user", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150);
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 100);

            cmd.Parameters["@Email"].Value = user.Email;
            cmd.Parameters["@FirstName"].Value = user.FirstName;
            cmd.Parameters["@LastName"].Value = user.LastName;
            cmd.Parameters["@Password"].Value = user.Password;

            try
            {
                conn.Open();
                userID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return userID;
        }
    }
}
