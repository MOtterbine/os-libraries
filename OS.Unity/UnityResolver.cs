using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using log4net;

namespace OS.Unity
{
    /// <summary>
    /// Singleton class used for an applcation to resolve all unity configured dependencies
    /// </summary>
    public class UnityResolver : IDisposable
    {

        private static volatile UnityResolver instance;
        private static object syncRoot = new Object();

        // Private singleton constructor
        private UnityResolver()
        {
            this._Container.LoadConfiguration();
        }

        public static UnityResolver Instance
        {
            get 
            {
                if (instance == null) 
                {
                lock (syncRoot) 
                {
                    if (instance == null)
                        instance = new UnityResolver();
                }
                }

                return instance;
            }
        }


        protected readonly ILog _logger = LogManager.GetLogger(typeof(UnityResolver));

        //Create the unity container
        private IUnityContainer _Container = new UnityContainer();

        /// <summary>
        /// Gets an object based on the unity confiuration container 'name'. Runtime objects may be injected as 
        /// key/value pairs via a <see cref="Dictionary"/> object
        /// </summary>
        /// <typeparam name="T">Type of object expected</typeparam>
        /// <param name="alias">The unity container registerd name (from configuration)</param>
        /// <param name="constructorArguments">Runtime objects to be injected</param>
        /// <returns></returns>
        public T GetObjectByAlias<T>(String alias, Dictionary<string, object> constructorArguments ) where T : class
        {
            ParameterOverrides constArgs = new ParameterOverrides();
            if (constructorArguments != null)
            {
                foreach (KeyValuePair<string, object> kvp in constructorArguments)
                {
                    constArgs.Add(kvp.Key, kvp.Value);
                }
            }
            return this.GetObjectByAlias<T>(alias, constArgs);
        }

        private T GetObjectByAlias<T>(String alias, params ResolverOverride[] overrides) where T : class
        {
            return this.ResolveDIObject<T>(this._Container, alias, overrides);
        }

        /// <summary>
        /// Instantiates a container that contains injected componenents. The internal 
        /// components must first be resolved using ResolveDIObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <returns></returns>
        private T InstantiateContainer<T>(IUnityContainer container)
            where T : class
        {
            T containingClass = null;
            try
            {
                //Resolve for OrderController and the dependecy gets injected automatically
                containingClass = container.Resolve<T>();
            }
            catch (Exception ex)
            {
                this._logger.ErrorFormat("Error resolving DI container [{0}] - {1}", typeof(T).Name, ex.InnerException==null?ex.Message:" - " + ex.InnerException.Message);
                return null;
            }
            return containingClass;
        }

        /// <summary>
        /// Resolves DI objects based on unity configuration
        /// </summary>
        /// <typeparam name="T">Type to instantiate</typeparam>
        /// <param name="container">Unity Container</param>
        /// <param name="uniqueObjectName">Alias of object - in case there are multiples of the same interface</param>
        /// <param name="overrides">for runtime injection of objecst such as construction objects</param>
        /// <returns></returns>
        private T ResolveDIObject<T>(IUnityContainer container, String uniqueObjectName = "", params ResolverOverride[] overrides)
            where T : class
        {
            T concreteObject = null;
            try
            {
                //Resolve the dependency
                concreteObject = container.Resolve<T>(uniqueObjectName, overrides);
                //Register the instance
                container = container.RegisterInstance<T>(concreteObject as T);
            }
            catch(Microsoft.Practices.Unity.ResolutionFailedException rex)
            {
                this._logger.ErrorFormat("Error resolving an '{0}' via config alias '{1}' - {2}", typeof(T).FullName, uniqueObjectName, rex.InnerException == null ? rex.Message : rex.InnerException.Message);
                throw;
            }
            catch (Exception ex)
            {
                this._logger.ErrorFormat("Error resolving an '{0}' via config alias '{1}' - {2}", typeof(T).FullName, uniqueObjectName, ex.InnerException == null ? ex.Message :ex.InnerException.Message);
                throw;
            }
            this._logger.DebugFormat("Resolved: '{0}' implemented by '{1}', registered name: '{2}'", typeof(T).FullName, concreteObject.GetType().FullName, uniqueObjectName);
            return concreteObject;
        }

        #region IDisposable Members

        private bool disposed = false;
		public void Dispose()
		{
			Dispose(true);
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}
		// Dispose(bool disposing) executes in two distinct scenarios.
		// If disposing equals true, the method has been called directly
		// or indirectly by a user's code. Managed and unmanaged resources
		// can be disposed.
		// If disposing equals false, the method has been called by the
		// runtime from inside the finalizer and you should not reference
		// other objects. Only unmanaged resources can be disposed.
		private void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!this.disposed)
			{
                if(this._Container != null)
                {
                    this._Container.Dispose();
                }
				// If disposing equals true, dispose all managed
				// and unmanaged resources.
				if (disposing)
				{

                }
				// Note disposing has been done.
				disposed = true;
			}
		}
        ~UnityResolver()
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			Dispose(false);
        }

        #endregion IDisposable Members

    }
}
