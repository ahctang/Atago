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
using System.Reflection;
using Atago.GitExcludedConstants;

using RedditSharp;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;


namespace Atago
{
    public partial class Service1 : ServiceBase
    {

        #region Fields

        /// <summary>
        /// Reddit related fields
        /// </summary>
        //Page check interval in ms
        private int redditCheckInterval = 30000;
        private Timer timer = new Timer();
        private string subredditName = "/r/leagueoflegends";
        //Reddit crawler target keywords
        private string[] keywords = { "SKT", "FAKER", "BANG", "WOLF", "PEANUT", "HUNI" };

        /// <summary>
        /// MongoDB related fields
        /// </summary>            
        private MongoClient dbClient = new MongoClient(PrivateConstants.ConnectionString);
        
        List<RedditEntry> frontPageList = new List<RedditEntry>();

        #endregion

        #region Init
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\littl\Documents\Visual Studio 2015\Projects\Atago\log.txt"))
            {
                file.WriteLine("On start!");
            }
            //Register the tick with the timer
            timer.Interval = redditCheckInterval;
            timer.Elapsed += new ElapsedEventHandler(checkReddit);
            timer.Start();

            //Bsonclass map for mongodb
            BsonClassMap.RegisterClassMap<RedditEntry>();
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
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\littl\Documents\Visual Studio 2015\Projects\Atago\log.txt", true))
            {
                file.WriteLine("Tick!");
            }

            //Query Reddit for all front page stories on /r/lol
            List<RedditEntry> newFrontPageList = getFrontPage();
            
            List<RedditEntry> targetPosts = getTargetPosts(newFrontPageList);

            if (targetPosts.Count != 0)
            {
                //Send Windows 10 Notification Here

                //Save to MongoDB
                debugDump(targetPosts, "savedposts.txt", true);
                var database = dbClient.GetDatabase(PrivateConstants.dbName);

                var collection = database.GetCollection<RedditEntry>("RedditPosts");

                collection.InsertManyAsync(targetPosts);
            }
        }

        //Gets the current front page of /r/leagueoflegends
        private List<RedditEntry> getFrontPage()
        {
            List<RedditEntry> newFrontPageList = new List<RedditEntry>();


            var reddit = new Reddit();
            var redditUser = reddit.LogIn(User.UserName, User.Password);

            var subreddit = reddit.GetSubreddit(subredditName);

            foreach (var post in subreddit.Hot.Take(25))
            {
                RedditEntry buffer = new RedditEntry();
                buffer.Id = post.Id;
                buffer.Title = post.Title;
                buffer.Date = post.Created.ToShortDateString();
                buffer.Body = post.SelfText;
                buffer.Url = post.Url.ToString();
                buffer.Upvotes = post.Upvotes;

                newFrontPageList.Add(buffer);
            }

            return newFrontPageList;
        }

        private List<RedditEntry> getTargetPosts(List<RedditEntry> newFrontPageList)
        {
            //Get new entries only and check titles of new posts for keywords
            var newPosts = from post in newFrontPageList
                                where !(frontPageList.Any(p2 => p2.Id == post.Id))                                
                                select post;

            var targetPosts = from post in newFrontPageList
                              where keywords.Any(keyword => post.Title.ToUpper().Contains(keyword))
                              select post;
            
            //Save the current front page for comparison to the next query
            frontPageList = newFrontPageList;

            List<RedditEntry> returnList = targetPosts.ToList<RedditEntry>();

            return returnList;
        }

        private void debugDump(List<RedditEntry> RedditFrontPage, string debugFileName, bool append = false)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\littl\Documents\Visual Studio 2015\Projects\Atago\" + debugFileName, append))
            {
                foreach (var post in RedditFrontPage)
                {                    
                    Type type = post.GetType();
                    PropertyInfo[] properties = type.GetProperties();
                    
                    foreach(PropertyInfo property in properties)
                    {
                        file.WriteLine(property.GetValue(post, null));
                    }
                }
            }
        }

        #endregion
    }

}
