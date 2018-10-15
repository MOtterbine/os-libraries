using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OS.Application;


namespace OS.WPF
{
    /// <summary>
    /// Placeholder to enforce interface procedure when accessing the data model
    /// </summary>
    public interface IAppViewModel : IInitialize, IDisposable
    {
        event ModelEvent ModelEvent;
        ICommand CloseCommand { get; }
    }
}
