using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Atago
{
    public partial class Service1 : ServiceBase
    {

        #region Fields

        private TimeSpan redditCheckInterval = new TimeSpan(1, 0, 0);
        private DispatcherTimer timer = new DispatcherTimer();

        private string subredditName = "/r/leagueoflegends";

        #endregion

        #region Init
        public Service1()
        {
            InitializeComponent();

            //Register the tick with the timer
            timer.Interval = redditCheckInterval;
            timer.Tick += new EventHandler(checkReddit);
        }

        protected override void OnStart(string[] args)
        {
            timer.Start();   
        }

        protected override void OnStop()
        {
            timer.Stop();
        }

        #endregion

        #region Methods

        //  System.Windows.Threading.DispatcherTimer.Tick handler               
        private void checkReddit(object sender, EventArgs e)
        {
            //Query Reddit for all front page stories on /r/lol
            
        }

        private List<RedditEntry> getFrontPage()
        {
            return new List<RedditEntry>();
        }

        #endregion
    }

}
