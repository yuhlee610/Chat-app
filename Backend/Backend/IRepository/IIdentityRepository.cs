using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.IRepository
{
    public interface IIdentityRepository
    {
        string Authenticate(string email, string guid);
    }
}
