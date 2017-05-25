using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using RedditSharp;

namespace Atago
{
    public partial class Service1 : ServiceBase
    {
        
        #region Fields

        private int redditCheckInterval = 30000;
        private Timer timer = new Timer();
        private string subredditName = "/r/leagueoflegends";

        List<RedditEntry> frontPageList = new List<RedditEntry>();

        #endregion

        #region Init
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\littl\Documents\Visual Studio 2015\Projects\Atago\debugtext.txt"))
            {
                file.WriteLine("On start!");
            }
            //Register the tick with the timer
            timer.Interval = redditCheckInterval;
            timer.Elapsed += new ElapsedEventHandler(checkReddit);
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
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\littl\Documents\Visual Studio 2015\Projects\Atago\debugtext.txt"))
            {
                file.WriteLine("Tick!");
            }

            //Query Reddit for all front page stories on /r/lol
            List<RedditEntry> newFrontPageList = getFrontPage();

            //For debug for now
            debugDump(newFrontPageList);
        }

        private List<RedditEntry> getFrontPage()
        {
            var reddit = new Reddit();
            var redditUser = reddit.LogIn(User.UserName, User.Password);

            var subreddit = reddit.GetSubreddit(subredditName);

            //Gets the current front page of /r/leagueoflegends
            List<RedditEntry> newFrontPageList = new List<RedditEntry>();

            foreach (var post in subreddit.Hot.Take(25))
            {
                RedditEntry buffer = new RedditEntry();
                buffer.Title = post.Title;
                buffer.Body = post.SelfText;
                buffer.Url = post.Url.ToString();

                newFrontPageList.Add(buffer);
            }

            return newFrontPageList;
        }

        private void debugDump(List<RedditEntry> RedditFrontPage)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\littl\Documents\Visual Studio 2015\Projects\Atago\debugtext.txt"))
            {
                foreach(var post in RedditFrontPage)
                {
                    file.WriteLine(post.Title);
                    file.WriteLine(post.Body);
                    file.WriteLine(post.Url);
                    file.WriteLine();
                }
            }                           
        }

        #endregion
    }

}
