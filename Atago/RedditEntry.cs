using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atago
{
    class RedditEntry
    {
        //Post Title
        public string Id { get; set; }

        //Post Title
        public string Title { get; set; }

        //Post Date
        public string Date { get; set; }

        //Post Body
        public string Body { get; set; }

        //Post Url
        public string Url { get; set; }

        //Post upvote count
        public int Upvotes { get; set; }

    }
}
