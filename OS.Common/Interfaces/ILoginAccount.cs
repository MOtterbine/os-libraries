using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OS.Interfaces;

namespace OS.Interfaces
{
    public interface ILoginAccount<T, T1> where T : class
    {
        IRepository<T> getUsers();
        T AddUser(T user);
    }
}
