using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS.Interfaces
{

    /// <summary>
    /// Repository interface. 
    /// </summary>
    /// <typeparam name="T">The type to be enumerated and operated upon</typeparam>
    /// <typeparam name="T1">Typically, the database context type</typeparam>
    public interface IRepository<T, TContext> where T : class where TContext : class
    {
        IEnumerable<T> List { get; }
        object GetState(T target);
        void SetState(T target, object state);
        object Add(T target);
        T Delete(T target);
        T Delete(object id);
        T Find(object id);
        T Update(T target);
        int SaveChanges();
        int DiscardChanges();

    }
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> List { get; }
        object GetState(T target);
        void SetState(T target, object state);
        object Add(T target);
        T Delete(T target);
        T Delete(object id);
        T Find(object id);
        T Update(T target);
        int SaveChanges();
        int DiscardChanges();

    }
}
