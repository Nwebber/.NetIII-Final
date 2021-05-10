using DataObjectsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IAccountManager
    {
        Account AuthenticateAccount(string email, string password);

        bool UpdatePassword(Account account, string oldPassword, string newPassword);
    }
}
