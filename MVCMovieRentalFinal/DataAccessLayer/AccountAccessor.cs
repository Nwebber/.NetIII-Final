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
    public class AccountAccessor : IAccountAccessor
    {
        public Account SelectUserByEmail(string email)
        {
            Account account = null;

            // Get connection
            var conn = DBConnection.GetDBConnection();

            // Create command
            var cmd = new SqlCommand("sp_select_user_by_email", conn);

            // Set command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Create parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150);

            // Set parameter values
            cmd.Parameters["@Email"].Value = email;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Execute command
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    var accountID = reader.GetInt32(0);
                    // We already have the email
                    var firstName = reader.GetString(2);
                    var lastName = reader.GetString(3);
                    var active = reader.GetBoolean(4);

                    reader.Close();

                    // Get the user roles
                    var userAccessor = new UserAccessor();
                    var roles = userAccessor.SelectRolesByUserID(accountID);

                    // Add to an account object
                    account = new Account(accountID, firstName, lastName, email, roles);

                }
                else
                {
                    throw new ApplicationException("User not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return account;
        }

        public int UpdatePasswordHash(string email, string newPasswordHash, string oldPasswordHash)
        {
            int result = 0;

            // Connection
            var conn = DBConnection.GetDBConnection();

            // Command
            var cmd = new SqlCommand("sp_update_passwordhash", conn);

            // Command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@OldPassword", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPassword", SqlDbType.NVarChar, 100);

            // Values
            cmd.Parameters["@Email"].Value = email;
            cmd.Parameters["@OldPassword"].Value = oldPasswordHash;
            cmd.Parameters["@NewPassword"].Value = newPasswordHash;

            // Execute
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

        public int VerifyUserNameAndPassword(string email, string passwordHash)
        {
            int result = 0; // Verification will falsify this

            var conn = DBConnection.GetDBConnection();

            var cmd = new SqlCommand("sp_authenticate_user", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150);
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 100);

            cmd.Parameters["@Email"].Value = email;
            cmd.Parameters["@Password"].Value = passwordHash;

            try
            {
                conn.Open();

                result = Convert.ToInt32(cmd.ExecuteScalar());
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
    }
}
