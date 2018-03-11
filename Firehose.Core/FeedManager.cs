using System;
using System.Collections.Generic;

namespace Firehose.Core
{
    public class FeedManager
    {
        private readonly DataAccess dataAccess;
        private Dictionary<int, Feed> feeds = new Dictionary<int, Feed>();
        
        
        public FeedManager(DataAccess dataAccess)
        { this.dataAccess = dataAccess; }


        public Feed AddFeed(Feed feedData)
        {
            int? id = dataAccess.AddFeed(feedData.Name, feedData.Address);
            if (id == null)
                return null;
            var feed = new Feed(id.Value, feedData.Name, feedData.Address, feedData.FeedType);
            feeds.Add(id.Value, feed);
            return feed;
        }

        
        public bool RemoveFeed(Feed feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));
            return dataAccess.DeleteFeed(feed.Id);
        }
        

        public bool RenameFeed(Feed feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));
            return dataAccess.RenameFeed(feed.Id, feed.Name);
        }
    }
}