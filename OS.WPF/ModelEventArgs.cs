using System;

namespace OS.WPF
{
    public delegate object ModelEvent(object sender, ModelEventArgs e);

    public class ModelEventArgs : EventArgs, IDisposable
    {
        public object WorkerObject { get; private set; }
        public ModelEventTypes EventType {get; private set;}
        public String [] Messages { get; private set;}


        public ModelEventArgs(object workerObject, ModelEventTypes modelEvent, string message)
        {
            this.WorkerObject = workerObject;
            this.EventType = modelEvent;
            this.Messages = new String [] { message };
        }
        public ModelEventArgs(object workerObject, ModelEventTypes modelEvent, string[] messages)
        {
            this.WorkerObject = workerObject;
            this.EventType = modelEvent;
            this.Messages = messages;
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
				// If disposing equals true, dispose all managed
				// and unmanaged resources.
				if (disposing)
				{

                }
				// Note disposing has been done.
				disposed = true;
			}
		}
        ~ModelEventArgs()
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			Dispose(false);
        }

        #endregion IDisposable Members






    }
}
