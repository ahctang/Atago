using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Atago
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Atago is a Windows Service that scrubs target subreddits for Donald Trump and corruption related information
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
