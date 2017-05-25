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
        /// Atago is a web scrapper that checks for topics related to SKT T1 on the front page of /r/LeagueofLegends
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
