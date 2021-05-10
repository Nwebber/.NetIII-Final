using DataAccessLayer;
using DataObjectsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class AccountManager : IAccountManager
    {
        private IAccountAccessor accountAccessor;

        public AccountManager()
        {
            accountAccessor = new AccountAccessor();
        }

        public AccountManager(IAccountAccessor suppliedAccountAccessor)
        {
            accountAccessor = suppliedAccountAccessor;
        }

        public Account AuthenticateAccount(string email, string password)
        {
            Account account = null;

            // Hash the password
            password = hashSHA256(password);

            bool isNewUser = (password == "newuser");

            // This calls a method that throws exceptions
            try
            {
                // Was the user found?
                if (1 == accountAccessor.VerifyUserNameAndPassword(email, password))
                {
                    // If so, we need to get a user object
                    account = accountAccessor.SelectUserByEmail(email);

                }
                else
                {
                    throw new ApplicationException("Bad username or password");
                }
                // Return the user object

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Login Failed.", ex);
            }

            return account;
        }

        public bool UpdatePassword(Account account, string oldPassword, string newPassword)
        {
            bool result = false;

            oldPassword = oldPassword.SHA256Value();
            newPassword = newPassword.SHA256Value();

            try
            {
                result = (1 == accountAccessor.UpdatePasswordHash(account.Email, newPassword, oldPassword));

                if (result == false)
                {
                    throw new ApplicationException("Update Failed.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Bad username or password.", ex);
            }

            return result;
        }

        // Helper method to hash passwords
        private string hashSHA256(string source)
        {
            string result = "";

            // Create a byte array - cryptography is byte oriented
            byte[] data;

            // Create a .NET hash provider object
            using (SHA256 sha256hash = SHA256.Create())
            {
                // Hash the source
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            // Now to build the result string
            var s = new StringBuilder();

            // Loop through the byte array
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            // Convert the string builder to a string
            result = s.ToString();

            return result;
        }
    }
}
