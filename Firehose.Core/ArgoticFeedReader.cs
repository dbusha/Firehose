using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
        }


        public async Task<IEnumerable<FeedItem>> UpdateFeedAsync(Feed feed)
        {
            var list = new List<FeedItem>();
            switch (feed.FeedType)
            {
                case FeedType.Rss:
                    var rss = RssFeed.Create(new Uri(feed.Address));
                    foreach (var item in rss.Channel.Items)
                        list.Add(new FeedItem(item));
                     break;
                case FeedType.Atom:
                    var atom = AtomFeed.Create(new Uri(feed.Address));
                    foreach(var entry in atom.Entries)
                        list.Add(new FeedItem(entry));
                    break;
                default: 
                    var unknown = GenericSyndicationFeed.Create(new Uri(feed.Address));
                    foreach(var item in unknown.Items)
                        list.Add(new FeedItem(item));
                    break;
            }

            return list;
        }
        
    } 
}