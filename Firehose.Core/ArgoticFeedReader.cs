using System;
using System.Linq;
using Argotic.Syndication;

namespace Firehose.Core
{
    public class ArgoticFeedReader
    {
        private readonly DataAccess dataAccess;

        public ArgoticFeedReader(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }


        public Feed GetFeed(Feed feed)
        {
            switch (feed.FeedType)
            {
                case FeedType.Rss:
                    var rss = RssFeed.Create(new Uri(feed.Address));
                    return new Feed(rss);
                case FeedType.Atom:
                    var atom = AtomFeed.Create(new Uri(feed.Address));
                    return new Feed(atom);
                default: 
                    var unknown = GenericSyndicationFeed.Create(new Uri(feed.Address));
                    return new Feed(unknown);
            }

            return null;
        }
        
    }
}