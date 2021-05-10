using DataObjectsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IAccountAccessor
    {
        int VerifyUserNameAndPassword(string email, string passwordHash);

        Account SelectUserByEmail(string email);

        int UpdatePasswordHash(string email, string newPasswordHash, string oldPasswordHash);
    }
}
