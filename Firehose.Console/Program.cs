using System;
using Firehose.Core;

namespace Firehose.Console
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            DataAccess da = new DataAccess();
            ArgoticFeedReader reader = new ArgoticFeedReader(da);
            //var feed = new Feed(1, "Plasma", "http://plasmasturm.org/feeds/plasmasturm/", FeedType.Atom);
            //var feed = new Feed(1, "Slashdot", "http://rss.slashdot.org/Slashdot/slashdotMain", FeedType.Rss);
            var feed = new Feed(1, "Adafruit", "http://adafruit.com/blog/feed", FeedType.Rss);
            reader.GetFeed(feed);

        }
    }
}
