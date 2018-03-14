using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Firehose.Core.Properties
{
    public class FeedController
    {
        private readonly DataAccess dataAccess;
        private Timer timer_;
        List<Feed> feeds_ = new List<Feed>();
        ArgoticFeedReader reader_;
        
        public FeedController(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            reader_ = new ArgoticFeedReader(dataAccess);
            timer_ = new Timer(OnTimerElapsed_, null, Timeout.Infinite, Timeout.Infinite);
        }

        private async Task OnTimerElapsed_(object state)
        {
            foreach (var feed in feeds_)
            {
                if (feed.LastUpdate < (DateTime.Now - feed.Frequency))
                {
                    var feedItem = await reader_.UpdateFeedAsync(feed);
                    await dataAccess.UpdateFeedAsync(feedItem);
                }
            }
            
        }
    }
}