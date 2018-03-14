using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firehose.Core
{
    public class FeedManager
    {
        private readonly DataAccess dataAccess;
        private Dictionary<int, Feed> feeds = new Dictionary<int, Feed>();
        
        
        public FeedManager(DataAccess dataAccess)
        { this.dataAccess = dataAccess; }


        public async Task<Feed> AddFeedAsync(Feed feedData)
        {
            int? id = await dataAccess.AddFeedAsync(feedData.Name, feedData.Address);
            if (id == null)
                return null;
            var feed = new Feed(id.Value, feedData.Name, feedData.Address, feedData.FeedType);
            feeds.Add(id.Value, feed);
            return feed;
        }

        
        public async Task<bool> RemoveFeedAsync(Feed feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));
            return await dataAccess.DeleteFeedAsync(feed.Id);
        }
        

        public async Task<bool> RenameFeedAsyncAsync(Feed feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));
            return await dataAccess.RenameFeedAsync(feed.Id, feed.Name);
        }
    }
}